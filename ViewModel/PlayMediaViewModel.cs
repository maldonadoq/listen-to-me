using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ListenToMe.Model;
using Microsoft.Win32;

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
            Play = new RelayCommand(PlayCMD);
            Pause = new RelayCommand(PauseCMD);
            Stop = new RelayCommand(StopCMD);
        }

        #region Command Handling
        private void PlayCMD()
        {
            if (PlayEvent != null)
            {
                EventHandler handler = PlayEvent;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void PauseCMD()
        {
            if (PauseEvent != null)
            {
                EventHandler handler = PauseEvent;
                handler?.Invoke(this, EventArgs.Empty);
            }
        }

        private void StopCMD()
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