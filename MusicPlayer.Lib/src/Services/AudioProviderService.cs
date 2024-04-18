using LibVLCSharp.Shared;
using MusicPlayer.Lib.src.Interfaces;
using MusicPlayer.Lib.src.Models;
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

        public event EventHandler<MusicFile> SongChanged;
        public event EventHandler SongEnded;

        public bool IsRandomized { get; set; } = false;

        public bool IsPlaying 
        { 
            get => _player.IsPlaying;
        }

        public AudioProviderService()
        {
            Core.Initialize();
            _libVlc = new LibVLC();
            _player = new MediaPlayer(_libVlc);

            _player.EndReached += (sender, e) => SongEnded.Invoke(this, EventArgs.Empty);
        }

        public async Task PlayAudioFile(MusicFile musicFile)
        {
            await Task.Run(() =>
            {
                if (_player.IsPlaying)
                {
                    _player.Stop();
                }

                _currentMedia = new Media(_libVlc, musicFile.FilePath);
                SongChanged.Invoke(this, musicFile);
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
