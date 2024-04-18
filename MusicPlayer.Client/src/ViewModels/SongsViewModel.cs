using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Lib.src.Interfaces;
using MusicPlayer.Lib.src.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Client.src.ViewModels
{
    public partial class SongsViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IFileManipulatorService _fileManipulator;
        private readonly IAudioProviderService _audioProvider;
        private readonly Random _random;

        [ObservableProperty]
        private string _totalSongs;

        [ObservableProperty]
        private ObservableCollection<MusicFile> _foundSongs;

        [ObservableProperty]
        private int _currentlyPlayingIndex;

        public SongsViewModel(IFileManipulatorService fileManipulator, IAudioProviderService audioProvider, Random random)
        {
            _fileManipulator = fileManipulator;
            _audioProvider = audioProvider;
            _audioProvider.SongEnded += (sender, e) => PlayNext();

            TotalSongs = "No songs found!";

            _random = random;
        }

        partial void OnFoundSongsChanged(ObservableCollection<MusicFile> value)
        {
            TotalSongs = $"{value.Count} songs";
        }

        [RelayCommand]
        private void Shuffle()
        {
            if (FoundSongs == null)
            {
                return;
            }

            if (FoundSongs.Count > 0)
            {
                _audioProvider.IsRandomized = true;
                CurrentlyPlayingIndex = _random.Next(FoundSongs.Count);
                _audioProvider.PlayAudioFile(FoundSongs.ElementAt(CurrentlyPlayingIndex));
            }
        }

        [RelayCommand]
        private void PlayFirst()
        {
            if (FoundSongs == null)
            {
                return;
            }

            if (FoundSongs.Count > 0) 
            {
                _audioProvider.IsRandomized = false;
                CurrentlyPlayingIndex = 0;
                _audioProvider.PlayAudioFile(FoundSongs.FirstOrDefault());
            }
        }

        [RelayCommand]
        private void PlaySoundAsync(MusicFile musicFile)
        {
            _audioProvider.PlayAudioFile(musicFile);
        }

        private void PlayNext()
        {
            if (_audioProvider.IsRandomized)
            {
                CurrentlyPlayingIndex = _random.Next(FoundSongs.Count);
                _audioProvider.PlayAudioFile(FoundSongs.ElementAt(CurrentlyPlayingIndex));
            }
            else
            {
                CurrentlyPlayingIndex++;
                if (CurrentlyPlayingIndex > FoundSongs.Count)
                {
                    CurrentlyPlayingIndex = 0;
                }
                _audioProvider.PlayAudioFile(FoundSongs.ElementAt(CurrentlyPlayingIndex));
            }
        }
    }
}
