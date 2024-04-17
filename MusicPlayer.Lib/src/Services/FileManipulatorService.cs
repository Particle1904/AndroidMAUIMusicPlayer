#if ANDROID
using Android.Content;
using Android.Database;
using Android.Provider;
#endif
using MusicPlayer.Lib.src.Interfaces;
using MusicPlayer.Lib.src.Models;
using System.Diagnostics;
using System.Security;
using System.Text.RegularExpressions;

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
        public async Task<List<MusicFile>> GetSoundFilesAsync(Context context)
        {
            await CheckAndRequestPermission<Permissions.StorageRead>();

            List<MusicFile> filesInSDCard = await Task.Run(() => GetSoundFilesInMediaStorage(context));

            return filesInSDCard;
        }

        public string GetFormattedFileName(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string formattedFilename = Regex.Replace(fileName, @"(?<!-\s)\d|[\[\]\-.]", "").Trim();
            return formattedFilename;
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
        /// <returns>A List containing MusicFile objects with data about the sound files.</returns>
        private List<MusicFile> GetSoundFilesInMediaStorage(Context context)
        {
            List<MusicFile> soundFilesInSDCard = new List<MusicFile>();

            string[] projection =
            {
                Android.Provider.MediaStore.IMediaColumns.Data,
                Android.Provider.MediaStore.IMediaColumns.Duration,
                Android.Provider.MediaStore.IMediaColumns.DisplayName,
                Android.Provider.MediaStore.IMediaColumns.Artist,
                Android.Provider.MediaStore.IMediaColumns.Album,
                Android.Provider.MediaStore.IMediaColumns.Genre,
                Android.Provider.MediaStore.IMediaColumns.Title,
            };

            using (ICursor cursor = context.ContentResolver.Query(MediaStore.Audio.Media.ExternalContentUri, projection,
                null, null, null))
            {
                if (cursor != null && cursor.MoveToFirst())
                {
                    do
                    {
                        long duration = cursor.GetLong(cursor.GetColumnIndex(projection[1]));
                        if (duration < TimeSpan.FromSeconds(_minFileThresholdSeconds).TotalMilliseconds)
                        {
                            continue;
                        }

                        string filePath = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                        if (_searchPattern.Split(',').Any(extension => filePath.EndsWith(extension, 
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            string name = cursor.GetString(cursor.GetColumnIndex(projection[2]));
                            string artist = cursor.GetString(cursor.GetColumnIndex(projection[3]));
                            string album = cursor.GetString(cursor.GetColumnIndex(projection[4]));
                            string genre = cursor.GetString(cursor.GetColumnIndex(projection[5]));
                            string title = cursor.GetString(cursor.GetColumnIndex(projection[6]));

                            MusicFile musicFile = new MusicFile(name, title, artist, album, genre, filePath, duration);
                            soundFilesInSDCard.Add(musicFile);
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