using MusicPlayer.Client.src.ViewModels;
using MusicPlayer.Lib.src.Models;
using System.Diagnostics;

namespace MusicPlayer.Client.src.Controls;

public partial class SongsPage : ContentView
{
	public SongsPage()
	{
		InitializeComponent();
	}

    private void ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        SongsViewModel viewModel = (BindingContext as SongsViewModel);
        viewModel.PlaySoundAsyncCommand.Execute(e.SelectedItem);
        viewModel.CurrentlyPlayingIndex = e.SelectedItemIndex;
		SongList.SelectedItem = null;
    }
}