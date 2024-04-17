using LibVLCSharp.Shared;
using MusicPlayer.Lib.src.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Services
{
    public class AudioProviderService : IAudioProviderService, IDisposable
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _player;

        public AudioProviderService()
        {
            Core.Initialize();
            _libVlc = new LibVLC();
            _player = new MediaPlayer(_libVlc);
        }

        public async Task PlayAudioFile(string filePath)
        {
            await Task.Run(() =>
            {
                if (!_player.IsPlaying)
                {
                    Media audioFile = new Media(_libVlc, filePath);
                    _player.Play(audioFile);
                }
            });
        }

        public Task StopAudioFile()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _player?.Dispose();
            _libVlc?.Dispose();
        }
    }
}
