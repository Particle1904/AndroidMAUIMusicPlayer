#if ANDROID
using Android.Content;
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
        public Task<HashSet<string>> GetSoundFilesAsync(Context contenxt);
#endif
    }
}
