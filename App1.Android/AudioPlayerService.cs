using Android.Media;
using Xamarin.Forms;
using App1.Droid;
using App1.Services;
using System;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace App1.Droid
{
    public class AudioPlayerService : IAudioPlayer
    {
        private MediaPlayer _mediaPlayer;

        public void PlaySound(string soundFileName)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Release();
                _mediaPlayer = null;
            }

            int resourceId = Android.App.Application.Context.Resources.GetIdentifier(
                System.IO.Path.GetFileNameWithoutExtension(soundFileName),
                "raw",
                Android.App.Application.Context.PackageName);

            if (resourceId != 0)
            {
                _mediaPlayer = MediaPlayer.Create(Android.App.Application.Context, resourceId);
                _mediaPlayer.Start();
            }
            else
            {
                Console.WriteLine($"Sound file {soundFileName} not found.");
            }
        }
    }
}
