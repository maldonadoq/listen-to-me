using System;
using TagLib;
using ListenToMe.Constants;
using CommunityToolkit.Mvvm.ComponentModel;
using TagLib.Id3v2;
using System.IO;

namespace ListenToMe.Model
{
    public class Media : ObservableObject, ICloneable
    {
        private string _path;
        private Uri _source;
        private string? _title;
        private string? _artist;
        private string? _duration;
        private string? _type;
        private string? _name;

        public Media(string path)
        {
            this._path = path;
            this._source = new Uri(path);
            LoadMediaInfo();
        }

        private void LoadMediaInfo()
        {
            try
            {
                TagLib.File? file = null;
                FileInfo? fileInfo = null;

                file = TagLib.File.Create(Path);
                fileInfo = new FileInfo(Path);

                this._title = file.Tag.Title;
                this._artist = file.Tag.FirstAlbumArtist;
                this._duration = file.Properties.Duration.ToString(@"hh\:mm\:ss");
                this._type = fileInfo.Extension;
                this._name = fileInfo.Name.Replace(_type, ""); ;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error creating media, exception: {e}");
            }
        }

        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }
        public Uri Source
        {
            get { return _source; }
            set { SetProperty(ref _source, value); }
        }
        public string Title
        {
            get { return String.IsNullOrEmpty(_title) ? _name : _title; }
            set { SetProperty(ref _title, value); }
        }
        public string Artist
        {
            get { return String.IsNullOrEmpty(_artist) ? MediaConstants.UNKNOWN_ARTIST : _artist; }
            set { SetProperty(ref _artist, value); }
        }

        public string Duration
        {
            get { return _duration ?? TimeSpan.Zero.ToString(@"hh\:mm\:ss"); }
            set { SetProperty(ref _duration, value); }
        }

        public string Type
        {
            get { return String.IsNullOrEmpty(_type) ? MediaConstants.UNKNOWN_TYPE : _type; }
            set { SetProperty(ref _artist, value); }
        }

        public Object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}