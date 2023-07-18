using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using ListenToMe.Model;
using Microsoft.Win32;
using ListenToMe.Util;

namespace ListenToMe.ViewModel
{
    public class OpenWindowMessage : ValueChangedMessage<PlayMediaViewModel>
    {
        public OpenWindowMessage(PlayMediaViewModel playMediaViewModel) : base(playMediaViewModel)
        {
        }
    }

    public class MediaListViewModel : ObservableObject
    {
        // Media list.
        public ObservableCollection<Media> MediaList { get; set; }

        // Media sekected.
        private Media? _selectedMedia;

        // Object that handles the new command.
        public RelayCommand NewMedia { get; set; }

        // Object that handles the delete command.
        public RelayCommand DeleteMedia { get; set; }

        // Object that handles the play command.
        public RelayCommand PlayMedia { get; set; }

        public Media? SelectedMedia
        {
            get { return _selectedMedia; }
            set
            {
                SetProperty(ref _selectedMedia, value);
                DeleteMedia.NotifyCanExecuteChanged();
                PlayMedia.NotifyCanExecuteChanged();
            }
        }

        public MediaListViewModel()
        {
            this.NewMedia = new RelayCommand(NewCommand);
            this.PlayMedia = new RelayCommand(PlayCommand, CanPlayOrDeleteCommand);
            this.DeleteMedia = new RelayCommand(DeleteCommand, CanPlayOrDeleteCommand);
            this.MediaList = new ObservableCollection<Media>();
            LoadMediaList();
        }

        /// <summary>
        /// Load the media list using files from the Music and Video directories on the computer.
        /// </summary>
        private void LoadMediaList()
        {
            DirectoryInfo musicsPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            DirectoryInfo videosPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            List<FileInfo> listPaths = new List<FileInfo>();

            listPaths.AddRange(musicsPath.GetFilesByExtensions(".mp3", ".mpg", ".mpeg", ".mp4"));
            listPaths.AddRange(videosPath.GetFilesByExtensions(".mp3", ".mpg", ".mpeg", ".mp4"));

            foreach (FileInfo path in listPaths)
            {
                this.MediaList.Add(new Media(path.FullName));
            }

            if (this.MediaList.Count > 0)
                this.SelectedMedia = this.MediaList[0];
            else
                this.SelectedMedia = null;
        }

        #region Command Handling
        private void NewCommand()
        {
            Media media;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg;*.mp4)|*.mp3;*.mpg;*.mpeg;*.mp4|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                media = new Media(openFileDialog.FileName);
                this.MediaList.Add(media);
                this._selectedMedia = media;
            }
        }

        private void DeleteCommand()
        {
            if (this.SelectedMedia != null)
                this.MediaList.Remove(this.SelectedMedia);

            if (this.MediaList.Count > 0)
                this.SelectedMedia = this.MediaList[0];
            else
                this.SelectedMedia = null;
        }

        private void PlayCommand()
        {
            if (this.SelectedMedia != null)
            {
                var playMediaViewModel = new PlayMediaViewModel(this.SelectedMedia);

                WeakReferenceMessenger.Default.Send(new OpenWindowMessage(playMediaViewModel));
            }
        }

        private bool CanPlayOrDeleteCommand()
        {
            return this.SelectedMedia != null;
        }
        #endregion
    }
}
