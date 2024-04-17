using Microsoft.Extensions.Logging;
using MusicPlayer.Lib.src.Services;
using MusicPlayer.Lib.src.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MusicPlayer.Client.src.ViewModels;

namespace MusicPlayer.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<IFileManipulatorService, FileManipulatorService>();
            builder.Services.AddSingleton<IAudioProviderService, AudioProviderService>();
            return builder.Build();
        }
    }
}
