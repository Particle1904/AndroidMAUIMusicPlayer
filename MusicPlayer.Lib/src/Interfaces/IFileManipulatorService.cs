#if ANDROID
using Android.Content;
using MusicPlayer.Lib.src.Models;

#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Interfaces
{
    public interface IFileManipulatorService
    {
#if ANDROID
        public string GetFormattedFileName(string filePath);
        public Task<List<MusicFile>> GetSoundFilesAsync(Context contenxt);
#endif
    }
}
