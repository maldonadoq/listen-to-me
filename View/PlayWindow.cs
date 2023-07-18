using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using ListenToMe.ViewModel;
using System;
using System.Windows.Controls.Primitives;
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

            WeakReferenceMessenger.Default.Register<CloseWindowMessage>(this, (r, m) =>
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


        #region Slider Time Controller

        /// <summary>
        /// Sets the clock for media time tracking.
        /// </summary>
        private void InitializeMidia()
        {
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
        }

        /// <summary>
        /// Starts the clock, and starts the media automatically.
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
        /// Update slide showing media time.
        /// </summary>
        private void TimerTick(object? sender, EventArgs? e)
        {
            if ((MediaElement.Source != null) && (MediaElement.NaturalDuration.HasTimeSpan) && (!isUsingSlider))
            {
                SliderProgress.Minimum = 0;
                SliderProgress.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                SliderProgress.Value = MediaElement.Position.TotalSeconds;
            }
        }

        /// <summary>
        /// Marks that the user has started to move the position of the slider.
        /// </summary>
		private void SliderDragStarted(object sender, DragStartedEventArgs e)
        {
            isUsingSlider = true;
        }

        /// <summary>
        /// User finished moving slide position, updates media position.
        /// </summary>
		private void SliderDragCompleted(object sender, DragCompletedEventArgs e)
        {
            isUsingSlider = false;
            MediaElement.Position = TimeSpan.FromSeconds(SliderProgress.Value);
        }

        /// <summary>
        /// Updates the label that displays the current media time.
        /// </summary>
		private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ProgressStatus.Text = TimeSpan.FromSeconds(SliderProgress.Value).ToString(@"hh\:mm\:ss");
        }
        #endregion
    }
}