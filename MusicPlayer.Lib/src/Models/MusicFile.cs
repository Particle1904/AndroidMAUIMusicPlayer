using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Models
{
    public class MusicFile
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string Genre { get; set; }
        public string FilePath { get; set; }
        public double DurationInMilliseconds { get; set; }
                        
        public MusicFile(string name, string title, string artist, string album, string genre, string filePath, double durationInMilliseconds)
        {
            Name = name;
            Title = title;
            Artist = artist;
            Album = album;
            Genre = genre;
            FilePath = filePath;
            DurationInMilliseconds = durationInMilliseconds;
        }
    }
}
