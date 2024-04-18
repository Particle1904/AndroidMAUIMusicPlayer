using MusicPlayer.Lib.src.Models;
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
        public event EventHandler<MusicFile> SongChanged;
        public event EventHandler SongEnded;
        public bool IsRandomized { get; set; }

        public Task PlayAudioFile(MusicFile musicFile);
        public Task PausePlayback();
        public Task ContinuePlayback();
    }
}
