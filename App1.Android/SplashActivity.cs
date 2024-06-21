using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using Xamarin.Essentials;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

namespace App1.Droid
{
    [Activity(Label = "SplashActivity", MainLauncher = true, NoHistory = true, Theme = "@style/MainTheme.Splash")]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Retrieve username from preferences
            string userName = Preferences.Get("Username", "User");

            // Set the splash screen content
            SetContentView(Resource.Layout.SplashScreen);

            var userNameTextView = FindViewById<TextView>(Resource.Id.userNameTextView);
            userNameTextView.Text = $"Здравствуйте, {userName}";

            // Delay and then start the main activity
            System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Delay(3000); // Delay for 3 seconds

                var intent = new Intent(this, typeof(MainActivity));
                StartActivity(intent);
            });
        }
    }
}
