using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MusicPlayer.Lib.src.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using MusicPlayer.Lib.src.Models;
using MusicPlayer.Client.src.Views;
using System.Collections.ObjectModel;

namespace MusicPlayer.Client.src.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IFileManipulatorService _fileManipulator;
        private readonly IAudioProviderService _audioProvider;
        private readonly Context _context;
        private readonly Random _random;

        private View _songsPage;

        [ObservableProperty]
        private View _currentPage;

        [ObservableProperty]
        private MusicFile _currentPlayingMusic;
        [ObservableProperty]
        private ImageSource _currentPlayingAlbumArt;

        public MainPageViewModel(IFileManipulatorService fileManipulator, IAudioProviderService audioProvider, Context context)
        {
            _fileManipulator = fileManipulator;
            _audioProvider = audioProvider;
            _audioProvider.SongChanged += (sender, e) => {
                CurrentPlayingMusic = e;
            };

            _context = context;
            _random = new Random();

            InitializeViews();
            
            CurrentPage = _songsPage;
            CurrentPlayingMusic = new MusicFile(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty,
                string.Empty, 0.0f, null);
        }

        [RelayCommand]
        public async Task LoadFiles()
        {
            if (_fileManipulator != null)
            {
                List<MusicFile> musicFiles = await _fileManipulator.GetSoundFilesAsync(_context);
                List<MusicFile> sortedList = musicFiles.OrderBy(file => file.Title).ToList();

                (_songsPage.BindingContext as SongsViewModel).FoundSongs = new ObservableCollection<MusicFile>(sortedList);
            }
        }

        [RelayCommand]
        public async Task PlayButton()
        {
            if(_audioProvider.IsPlaying) 
            {
                await _audioProvider.PausePlayback();
            }
            else
            {
                await _audioProvider.ContinuePlayback();
            }
        }

        private void InitializeViews()
        {
            _songsPage = new SongsPage();
            _songsPage.BindingContext = new SongsViewModel(_fileManipulator, _audioProvider, _random);
        }
    }
}