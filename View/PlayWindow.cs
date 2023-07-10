using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using ListenToMe.ViewModel;
using System;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Controls;

namespace ListenToMe
{
    public partial class PlayWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private Boolean isUsingSlider = false;

        public PlayWindow(PlayMediaViewModel playMediaViewModel)
        {
            this.InitializeComponent();

            WeakReferenceMessenger.Default.Register<CloseWindowMessage>(this, (r,m) =>
            {
                this.Hide();
            });

            DataContext = playMediaViewModel;

            this.InitializeMidia();

            playMediaViewModel.PlayEvent += (sender, e) =>
            {
                this.timer.Start();
                this.MediaElement.Play();
            };
            playMediaViewModel.PauseEvent += (sender, e) =>
            {
                this.timer.Stop();
                this.MediaElement.Pause();
            };
            playMediaViewModel.StopEvent += (sender, e) =>
            {
                this.timer.Stop();
                this.MediaElement.Stop();
            };
        }

        #region Volume Controller
        /// <summary>
        /// Controla o volume utilizando o scroll do mouse.
        /// </summary>
        private void VolumeMouseWheel(object sender, MouseWheelEventArgs e)
		{
			MediaElement.Volume += (e.Delta > 0) ? 0.1 : -0.1;
		}
        #endregion

        #region Slider Time Controller

        /// <summary>
        /// Define o relogio para controle do tempo da midia.
        /// </summary>
        private void InitializeMidia()
        {
			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += TimerTick;
        }

        /// <summary>
        /// Inicia o relogio, e inicia a midia automaticamento.
        /// </summary>
        private void MediaOpened(object sender, RoutedEventArgs e)
		{
            if (MediaElement.LoadedBehavior == MediaState.Play)
            {
                MediaElement.LoadedBehavior = MediaState.Manual;
                MediaElement.Play();
            }
            this.timer.Start();
		}

        /// <summary>
        /// Atualiza o slider mostrando o tempo da midia.
        /// </summary>
        private void TimerTick(object? sender, EventArgs? e)
		{
			if((MediaElement.Source != null) && (MediaElement.NaturalDuration.HasTimeSpan) && (!isUsingSlider))
			{
				SliderProgress.Minimum = 0;
				SliderProgress.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
				SliderProgress.Value = MediaElement.Position.TotalSeconds;
			}
		}

        /// <summary>
        /// Marca que o usuario comecou a mover a posicao do slider.
        /// </summary>
		private void SliderDragStarted(object sender, DragStartedEventArgs e)
		{
			isUsingSlider = true;
		}

        /// <summary>
        /// O usuario terminou de mover a posicao do slider, atualiza a posicao da midia.
        /// </summary>
		private void SliderDragCompleted(object sender, DragCompletedEventArgs e)
		{
			isUsingSlider = false;
			MediaElement.Position = TimeSpan.FromSeconds(SliderProgress.Value);
		}

        /// <summary>
        /// Atualiza a label que exibe o tempo atual da midia.
        /// </summary>
		private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			ProgressStatus.Text = TimeSpan.FromSeconds(SliderProgress.Value).ToString(@"hh\:mm\:ss");
		}
        #endregion
    }
}