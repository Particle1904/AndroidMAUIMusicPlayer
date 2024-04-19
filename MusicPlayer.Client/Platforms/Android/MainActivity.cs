using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
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
        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            if (newConfig.Orientation == Orientation.Landscape)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
            else if(newConfig.Orientation == Orientation.Portrait)
            {
                RequestedOrientation = ScreenOrientation.Portrait;
            }
        }
    }
}
