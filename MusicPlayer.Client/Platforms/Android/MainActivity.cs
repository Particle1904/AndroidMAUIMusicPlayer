using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace MusicPlayer.Client
{
    [Activity(Theme = "@style/Maui.SplashTheme", 
              MainLauncher = true, 
              ConfigurationChanges = ConfigChanges.ScreenSize | 
              ConfigChanges.Orientation |
              ConfigChanges.UiMode |
              ConfigChanges.ScreenLayout | 
              ConfigChanges.SmallestScreenSize | 
              ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);

            RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
        }
    }
}
