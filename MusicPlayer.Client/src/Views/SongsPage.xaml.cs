using MusicPlayer.Client.src.ViewModels;
using MusicPlayer.Lib.src.Models;
using System.Diagnostics;

namespace MusicPlayer.Client.src.Views;

public partial class SongsPage : ContentView
{
	public SongsPage()
	{
		InitializeComponent();
	}

    private void ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        SongsViewModel viewModel = (BindingContext as SongsViewModel);
        MusicFile song = e.CurrentSelection.FirstOrDefault() as MusicFile;
        viewModel.PlaySoundAsyncCommand.Execute(song);
        viewModel.CurrentlyPlayingIndex = viewModel.FoundSongs.IndexOf(song);
		SongList.SelectedItem = null;
    }
}