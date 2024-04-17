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
		(BindingContext as SongsViewModel).PlaySoundAsyncCommand.Execute((e.SelectedItem as MusicFile).FilePath);
    }
}