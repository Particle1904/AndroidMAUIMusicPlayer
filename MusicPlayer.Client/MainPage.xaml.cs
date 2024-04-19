using MusicPlayer.Client.src.ViewModels;
using MusicPlayer.Lib.src.Interfaces;
using Microsoft.Maui;
using System.Diagnostics;

namespace MusicPlayer.Client
{
    public partial class MainPage : ContentPage
    {
        private readonly IFileManipulatorService _fileManipulator;
        private readonly IAudioProviderService _audioProvider;

        public MainPage()
        {
            InitializeComponent();

            var _fileManipulator = IPlatformApplication.Current.Services.GetService<IFileManipulatorService>();
            var _audioProvider = IPlatformApplication.Current.Services.GetService<IAudioProviderService>();
            BindingContext = new MainPageViewModel(_fileManipulator, _audioProvider, Android.App.Application.Context);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if ((MainPageViewModel)BindingContext != null)
            {
                await (BindingContext as MainPageViewModel).LoadFiles();
            }
        }
    }
}
