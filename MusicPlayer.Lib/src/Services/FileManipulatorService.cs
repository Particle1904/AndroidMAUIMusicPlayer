﻿#if ANDROID
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Provider;
#endif
using MusicPlayer.Lib.src.Interfaces;
using MusicPlayer.Lib.src.Models;
using System.Diagnostics;
using System.Security;
using Microsoft.Maui.Graphics.Platform;
using System.Text.RegularExpressions;
using System.Net.Mime;

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
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
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
                Android.Provider.MediaStore.Audio.IAlbumColumns.AlbumId,
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

                        string album = cursor.GetString(cursor.GetColumnIndex(projection[4]));
                        if (album.Equals("WhatsApp Audio", StringComparison.InvariantCultureIgnoreCase))
                        {
                            continue;
                        }

                        string filePath = cursor.GetString(cursor.GetColumnIndex(projection[0]));
                        if (_searchPattern.Split(',').Any(extension => filePath.EndsWith(extension, 
                                StringComparison.OrdinalIgnoreCase)))
                        {
                            string name = cursor.GetString(cursor.GetColumnIndex(projection[2])); ;
                            string artist = cursor.GetString(cursor.GetColumnIndex(projection[3]));
                            string genre = cursor.GetString(cursor.GetColumnIndex(projection[5]));
                            // Format title to upper case first letter of title.
                            string rawTitle = cursor.GetString(cursor.GetColumnIndex(projection[6]));
                            string formattedTitle = $"{char.ToUpper(rawTitle[0])}{rawTitle[1..]}";
                            string title = GetFormattedFileName(formattedTitle).Replace("   ", " ").Replace("  ", " ");

                            if (title.Contains("Aero Chord"))
                            {
                                Trace.WriteLine("Aero");
                            }

                            ImageSource albumArtImageSource = null;
                            try
                            {
                                long albumArtId = cursor.GetLong(cursor.GetColumnIndex(projection[7]));
                                    Android.Net.Uri albumArtUri = ContentUris.WithAppendedId(
                                        Android.Net.Uri.Parse("content://media/external/audio/albumart"),
                                        albumArtId);
                                albumArtImageSource = ImageSource.FromUri(new Uri(albumArtUri.ToString()));

                                using (var inputStream = context.ContentResolver.OpenInputStream(albumArtUri))
                                {
                                    if (inputStream == null)
                                    {
                                        albumArtImageSource = null;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                // Exception is thrown cus the current image doesn't have AlbumArt.
                                albumArtImageSource = null;
                            }

                            MusicFile musicFile = new MusicFile(name, title, artist, album, genre, filePath, duration,
                                albumArtImageSource);
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