
using Android.App;
using Android.Content;

namespace GrupoCinte.Android.Helpers
{
    public static class DiaglogService
    {
        static global::Android.Support.V7.App.AlertDialog.Builder alert;
        static global::Android.Support.V7.App.AlertDialog alertDialog;

        public static void ShowMessage(Context context, string title, string message, string button)
        {
            new AlertDialog.Builder(context)
                .SetPositiveButton(button, (sent, args) => { })
                .SetMessage(message)
                .SetTitle(title)
                .SetCancelable(false)
                .Show();
        }

        public static void ShowLoading(Context context)
        {
            alert = new global::Android.Support.V7.App.AlertDialog.Builder(context);
            alert.SetView(Resource.Layout.loading);
            alert.SetCancelable(false);
            alertDialog = alert.Show();
        }

        public static void StopLoading()
        {
            if (alert != null)
            {
                alertDialog.Dismiss();
                alertDialog = null;
                alert = null;
            }
        }
    }
}