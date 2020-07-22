using Android.App;
using Android.OS;
using Android.Views.InputMethods;
using Android.Widget;
using GrupoCinte.Android.Helpers;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Helpers;
using GrupoCinte.Common.Results;
using GrupoCinte.Common.Services;
using Newtonsoft.Json;
using System;

namespace GrupoCinte.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class LoginActivity : Activity
    {
        EditText numberEditText, passwordEditText;
        Button loginButton;
        ApiService apiService;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.login);
            FindViews();
            HandleEvents();
            SetInitialData();
        }

        #region Initial methods

        private void FindViews()
        {

            numberEditText = FindViewById<EditText>(Resource.Id.numberEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);
        }

        private void HandleEvents()
        {
            loginButton.Click += LoginButton_Click;
        }

        private void SetInitialData()
        {
            apiService = new ApiService();
        }

        #endregion

        private async void LoginButton_Click(object sender, EventArgs e)
        {
            HideKeyboard();
            string idNumber, password;
            idNumber = numberEditText.Text;
            password = passwordEditText.Text;

            if (string.IsNullOrEmpty(idNumber))
            {
                DiaglogService.ShowMessage(this, "Error", "Please enter id number", "Ok");
                return;
            }
            else if (!Validators.ValidateStringFourToEightCharacters(password))
            {
                DiaglogService.ShowMessage(this, "Error", "Password should be between 4 and 8 characters", "Ok");
                return;
            }
            try
            {
                DiaglogService.ShowLoading(this);
                var request = new UserForLoginDto()
                {
                    IdNumber = idNumber,
                    Password = password
                };
                var response = await apiService.PostAsync<UserForLoginDto>(Settings.ApiUrl, Settings.Prefix, "/Auth/Login", request);
                DiaglogService.StopLoading();

                if (!response.IsSuccess)
                {
                    DiaglogService.ShowMessage(this, "Error", response.Message, "Ok");
                    return;
                }

                var result = JsonConvert.DeserializeObject<UserForLoginResultDto>(response.Result.ToString());
                Settings.Token = result.Token;
                Settings.Name = result.Name;
                Settings.LastName = result.LastName;

                Finish();
                StartActivity(typeof(UsersActivity));
            }
            catch (Exception ex)
            {
                DiaglogService.StopLoading();
                DiaglogService.ShowMessage(this, "Error", CommonHelpers.GetErrorMessage("", ex), "Ok");
            }
        }

        #region other methods

        private void HideKeyboard()
        {
            if (this.CurrentFocus != null)
            {
                InputMethodManager inputMethodManager = (InputMethodManager)this.GetSystemService(global::Android.App.Activity.InputMethodService);
                inputMethodManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
        }

        #endregion
    }
}