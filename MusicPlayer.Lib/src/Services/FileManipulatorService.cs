#if ANDROID
using Android.Content;
using Android.Database;
using Android.Provider;
#endif
using MusicPlayer.Lib.src.Interfaces;
using System.Diagnostics;

namespace MusicPlayer.Lib.src.Services
{
    public class FileManipulatorService : IFileManipulatorService
    {
#if ANDROID
        private readonly string _searchPattern = ".aac,.ac3,.aiff,.amr,.ape,.dts,.flac,.m4a,.mka,.mp2,.mp3,.mpc,.ogg,.opus,.ra,.tta,.wav,.wma,.wv";
        private readonly double _minFileThresholdSeconds = 15;

        /// <summary>
        /// Asynchronously retrieves sound files from the device's storage.
        /// </summary>
        /// <param name="context">The context used to access content resolver.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains a HashSet containing paths to the sound files found.</returns>
        public async Task<HashSet<string>> GetSoundFilesAsync(Context context)
        {
            await CheckAndRequestPermission<Permissions.StorageRead>();

            HashSet<string> filesInSDCard = await Task.Run(() => GetSoundFilesInMediaStorage(context));

            return filesInSDCard;
        }

        /// <summary>
        /// Checks and requests the specified permission asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of permission to check and request.</typeparam>
        /// <returns>A task representing the asynchronous operation. The task result contains the permission status.</returns>
        private async Task<PermissionStatus> CheckAndRequestPermission<T>() where T : Permissions.BasePermission, new()
        {
            PermissionStatus permission = await Permissions.CheckStatusAsync<T>();

            if (permission == PermissionStatus.Granted)
            {
                return permission;
            }

            permission = await Permissions.RequestAsync<T>();
            return permission;
        }

        /// <summary>
        /// Retrieves sound files from the SD card.
        /// </summary>
        /// <param name="context">The context used to access content resolver.</param>
        /// <returns>A HashSet containing paths to the sound files found.</returns>
        private HashSet<string> GetSoundFilesInMediaStorage(Context context)
        {
            HashSet<string> soundFilesInSDCard = new HashSet<string>();

            string[] projection =
            {
                Android.Provider.MediaStore.IMediaColumns.Data,
                Android.Provider.MediaStore.IMediaColumns.Duration
            };

            using (ICursor cursor = context.ContentResolver.Query(MediaStore.Audio.Media.ExternalContentUri, projection,
                null, null, null))
            {
                if (cursor != null && cursor.MoveToFirst())
                {
                    do
                    {
                        if (cursor.GetLong(cursor.GetColumnIndex(projection[1])) < TimeSpan.FromSeconds(_minFileThresholdSeconds).TotalMilliseconds)
                        {
                            continue;
                        }

                        string filePath = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                        if (_searchPattern.Split(',').Any(extension => filePath.EndsWith(extension, 
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            soundFilesInSDCard.Add(filePath);
                        }
                    }
                    while (cursor.MoveToNext());
                }
            }
            return soundFilesInSDCard;
        }
#endif
    }
}