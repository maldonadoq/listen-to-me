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
        // Lista de mediaas.
        public ObservableCollection<Media> MediaList { get; set; }

        // Media selecionada.
        private Media? _selectedMedia;

        // Objeto que lida com o comando novo.
        public RelayCommand NewMedia { get; set; }

        // Objeto que lida com o comando deletar.
        public RelayCommand DeleteMedia { get; set; }

        // Objeto que lida com o comando play.
        public RelayCommand PlayMedia { get; set; }

        public Media? SelectedMedia
        {
            get { return _selectedMedia; }
            set { SetProperty(ref _selectedMedia, value);
                    DeleteMedia.NotifyCanExecuteChanged();
                    PlayMedia.NotifyCanExecuteChanged(); }
        }

        public MediaListViewModel()
        {
            this.NewMedia = new RelayCommand(NewCMD);
            this.PlayMedia = new RelayCommand(PlayCMD, CanPlayOrDeleteCMD);
            this.DeleteMedia = new RelayCommand(DeleteCMD, CanPlayOrDeleteCMD);
            this.MediaList = new ObservableCollection<Media>();
            LoadMediaList();
        }

        /// <summary>
        /// Carrega a lista de midias utilizando os arquivos dos diretorios Music e Video do computador.
        /// </summary>
        private void LoadMediaList()
        {
            DirectoryInfo musicsPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            DirectoryInfo videosPath = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos));
            List<FileInfo> listPaths = new List<FileInfo>();

            listPaths.AddRange(musicsPath.GetFilesByExtensions(".mp3",".mpg",".mpeg",".mp4"));
            listPaths.AddRange(videosPath.GetFilesByExtensions(".mp3",".mpg",".mpeg",".mp4"));

            foreach(FileInfo path in listPaths)
            {
                this.MediaList.Add(new Media(path.FullName));
            }

            if (this.MediaList.Count > 0)
                this.SelectedMedia = this.MediaList[0];
            else
                this.SelectedMedia = null;
        }

        #region Command Handling
        private void NewCMD()
        {
            Media media;
            OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg;*.mp4)|*.mp3;*.mpg;*.mpeg;*.mp4|All files (*.*)|*.*";
			if(openFileDialog.ShowDialog() == true)
			{
                media = new Media(openFileDialog.FileName);
                this.MediaList.Add(media);
                this._selectedMedia = media;
			}
        }

        private void DeleteCMD()
        {
            if (this.SelectedMedia != null)
                this.MediaList.Remove(this.SelectedMedia);

            if (this.MediaList.Count > 0)
                this.SelectedMedia = this.MediaList[0];
            else
                this.SelectedMedia = null;
        }

        private void PlayCMD()
        {
            if (this.SelectedMedia != null)
            {
                var playMediaViewModel = new PlayMediaViewModel(this.SelectedMedia);

                WeakReferenceMessenger.Default.Send(new OpenWindowMessage(playMediaViewModel));
            }
        }

        private bool CanPlayOrDeleteCMD()
        {
            return this.SelectedMedia != null;
        }
        #endregion
    }
}
