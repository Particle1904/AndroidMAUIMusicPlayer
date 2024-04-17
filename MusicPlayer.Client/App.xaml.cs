using MusicPlayer.Client.src.ViewModels;
using MusicPlayer.Lib.src.Interfaces;

namespace MusicPlayer.Client
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
