using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Interfaces
{
    public interface IAudioProviderService
    {
        public bool IsPlaying { get; }

        public Task PlayAudioFile(string filePath);
        public Task PausePlayback();
        public Task ContinuePlayback();
    }
}
