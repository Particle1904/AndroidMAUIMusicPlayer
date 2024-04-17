using LibVLCSharp.Shared;
using MusicPlayer.Lib.src.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Lib.src.Services
{
    public class AudioProviderService : IAudioProviderService, IDisposable
    {
        private readonly LibVLC _libVlc;
        private readonly MediaPlayer _player;
        
        private Media _currentMedia;
        private float _position = 0.0f;

        public bool IsPlaying 
        { 
            get => _player.IsPlaying;
        }

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
                if (_player.IsPlaying)
                {
                    _player.Stop();
                }

                _currentMedia = new Media(_libVlc, filePath);
                _position = 0.0f;
                _player.Play(_currentMedia);
            });
        }

        public async Task PausePlayback()
        {
            await Task.Run(() =>
            {
                if (_player.IsPlaying)
                {
                    _position = _player.Position;
                    _player.Pause();
                }
            });
        }

        public async Task ContinuePlayback()
        {
            await Task.Run(() =>
            {
                if (!_player.IsPlaying)
                {
                    _player.Position = _position;
                    _player.Play();
                }
            });
        }

        public void Dispose()
        {
            _player?.Dispose();
            _libVlc?.Dispose();
        }
    }
}
