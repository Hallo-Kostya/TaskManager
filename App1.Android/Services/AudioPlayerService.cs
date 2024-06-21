using Android.Media;
using Xamarin.Forms;
using App1.Droid;
using System.IO;
using App1.Services;
using App1.Droid.Services;

[assembly: Dependency(typeof(AudioPlayerService))]
namespace App1.Droid.Services
{
    public class AudioPlayerService : IAudioPlayer
    {
        private MediaPlayer _mediaPlayer;

        public void PlaySound(string sound)
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Release();
                _mediaPlayer = null;
            }

            int soundResourceId = Android.App.Application.Context.Resources.GetIdentifier(Path.GetFileNameWithoutExtension(sound), "raw", Android.App.Application.Context.PackageName);
            _mediaPlayer = MediaPlayer.Create(Android.App.Application.Context, soundResourceId);
            _mediaPlayer.Start();
        }
    }
}
