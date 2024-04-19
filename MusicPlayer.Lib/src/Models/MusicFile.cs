using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Models
{
    public class MusicFile
    {
        public string Name { get; private set; }
        public string Title { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string Genre { get; private set; }
        public string FilePath { get; private set; }
        public double DurationInMilliseconds { get; private set; }
        public ImageSource AlbumArt { get; private set; }

        public MusicFile(string name, string title, string artist, string album, string genre,
            string filePath, double durationInMilliseconds, ImageSource albumArt)
        {
            Name = name;
            Title = title;
            Artist = artist;
            Album = album;
            Genre = genre;
            FilePath = filePath;
            DurationInMilliseconds = durationInMilliseconds;
            AlbumArt = albumArt;
        }
    }
}