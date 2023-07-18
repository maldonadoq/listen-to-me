using System.Windows;
using CommunityToolkit.Mvvm.Messaging;
using ListenToMe.ViewModel;

namespace ListenToMe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            WeakReferenceMessenger.Default.Register<OpenWindowMessage>(this, (r, m) =>
            {
                var fw = new PlayWindow(m.Value);

                fw.ShowDialog();
            });

            DataContext = new MediaListViewModel();
        }
    }
}
