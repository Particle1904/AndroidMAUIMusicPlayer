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

namespace MusicPlayer.Client.src.ViewModels
{
    public partial class MainPageViewModel : ObservableObject, INotifyPropertyChanged
    {
        private readonly IFileManipulatorService _fileManipulator;
        private readonly IAudioProviderService _audioProvider;
        private readonly Context _context;
        
        [ObservableProperty]
        private HashSet<string> _filesInSDCard;

        public MainPageViewModel(IFileManipulatorService fileManipulator, IAudioProviderService audioProvider, Context context)
        {
            _fileManipulator = fileManipulator;
            _audioProvider = audioProvider;
            _context = context;

            _filesInSDCard = new HashSet<string>();
        }

        [RelayCommand]
        public async Task LoadFiles()
        {
            if (_fileManipulator != null)
            {
                List<MusicFile> musicFiles = await _fileManipulator.GetSoundFilesAsync(_context);

                List<MusicFile> sortedList = musicFiles.OrderBy(file => file.Title).ToList();
            }
        }
    }
}
