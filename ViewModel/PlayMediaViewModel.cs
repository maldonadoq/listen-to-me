using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ListenToMe.Model;

namespace ListenToMe.ViewModel
{
    public class CloseWindowMessage : ValueChangedMessage<bool>
    {
        public CloseWindowMessage(bool value) : base(value)
        {
        }
    }

    public class PlayMediaViewModel : ObservableObject
    {
        public Media SelectedMedia { get; set; }
        public RelayCommand Play { get; set; }
        public RelayCommand Pause { get; set; }
        public RelayCommand Stop { get; set; }

        public event EventHandler? PlayEvent;
        public event EventHandler? PauseEvent;
        public event EventHandler? StopEvent;

        public PlayMediaViewModel(Media media)
        {
            SelectedMedia = media;
            Play = new RelayCommand(PlayCommand);
            Pause = new RelayCommand(PauseCommand);
            Stop = new RelayCommand(StopCommand);
        }

        #region Command Handling
        private void PlayCommand()
        {
            if (PlayEvent != null)
            {
                EventHandler handler = PlayEvent;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void PauseCommand()
        {
            if (PauseEvent != null)
            {
                EventHandler handler = PauseEvent;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void StopCommand()
        {
            if (StopEvent != null)
            {
                EventHandler handler = StopEvent;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
    }
}