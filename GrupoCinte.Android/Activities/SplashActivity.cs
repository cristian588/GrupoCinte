
using Android.App;
using Android.OS;
using GrupoCinte.Common.Helpers;

namespace GrupoCinte.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/Cinte.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }
        protected override void OnResume()
        {
            base.OnResume();
            if (string.IsNullOrEmpty(Settings.Token))
            {
                StartActivity(typeof(LoginActivity));
            }
            else
            {
                StartActivity(typeof(UsersActivity));
            }
        }

        public override void OnBackPressed() { }
    }
}