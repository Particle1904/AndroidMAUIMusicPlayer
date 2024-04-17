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

        [ObservableProperty]
        private string _totalSongs;

        [ObservableProperty]
        private ObservableCollection<MusicFile> _foundSongs;

        public SongsViewModel(IFileManipulatorService fileManipulator, IAudioProviderService audioProvider)
        {
            _fileManipulator = fileManipulator;
            _audioProvider = audioProvider;

            TotalSongs = "No songs found!";
        }

        partial void OnFoundSongsChanged(ObservableCollection<MusicFile> value)
        {
            TotalSongs = $"{value.Count} songs";
        }

        [RelayCommand]
        private void PlaySoundAsync(string filePath)
        {
            _audioProvider.PlayAudioFile(filePath);
        }
    }
}
