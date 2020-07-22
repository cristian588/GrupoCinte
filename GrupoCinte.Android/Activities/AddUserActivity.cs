using Android.App;
using Android.OS;
using Android.Widget;
using GrupoCinte.Android.Helpers;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Helpers;
using GrupoCinte.Common.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GrupoCinte.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AddUserActivity : Activity
    {
        EditText nameEditText, lastNameEditText, idNumberEditText, emailEditText, passwordEditText;
        Spinner idTypesSpinner;
        Button saveButton;
        ApiService apiService;

        private List<IdTypeForDetailedDto> idTypesList;
        private IdTypeForDetailedDto idTypeSelected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.add_user);
            FindViews();
            HandleEvents();
            SetInitialData();
            LoadData();
        }

        #region Initial methods

        private void FindViews()
        {

            nameEditText = FindViewById<EditText>(Resource.Id.nameEditText);
            lastNameEditText = FindViewById<EditText>(Resource.Id.lastNameEditText);
            idTypesSpinner = FindViewById<Spinner>(Resource.Id.idTypesSpinner);
            idNumberEditText = FindViewById<EditText>(Resource.Id.idNumberEditText);
            emailEditText = FindViewById<EditText>(Resource.Id.emailEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
        }

        private void HandleEvents()
        {
            saveButton.Click += SaveButton_Click;
        }

        private void SetInitialData()
        {
            apiService = new ApiService();
        }

        private async void LoadData()
        {
            try
            {
                DiaglogService.ShowLoading(this);
                var response = await apiService.GetListAsync<IdTypeForDetailedDto>(Settings.ApiUrl, Settings.Prefix, "/Users/GetUserIdTypes", "Bearer", Settings.Token);
                if (!response.IsSuccess)
                {
                    DiaglogService.StopLoading();
                    DiaglogService.ShowMessage(this, "Error loading id types", response.Message, "Accept");
                    return;
                }
                idTypesList = new List<IdTypeForDetailedDto>();
                idTypesList = JsonConvert.DeserializeObject<List<IdTypeForDetailedDto>>(response.Result.ToString());

                if (idTypesList.Count > 0)
                {
                    var categoriesAdapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleDropDownItem1Line, idTypesList.Select(x => x.Name).ToList());
                    idTypeSelected = idTypesList.ElementAt(0);
                    idTypesSpinner.Adapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleDropDownItem1Line, idTypesList.Select(x => x.Name).ToList());
                    idTypesSpinner.ItemSelected += IdTypesSpinner_ItemSelected;
                }
                else
                {
                    DiaglogService.ShowMessage(this, "Error", "Please add a category", "Accept");
                    Finish();
                }

                DiaglogService.StopLoading();
            }
            catch (System.Exception ex)
            {
                DiaglogService.StopLoading();
                DiaglogService.ShowMessage(this, "Error", CommonHelpers.GetErrorMessage("Error loading users", ex), "Ok");
            }
        }

        #endregion

        private void IdTypesSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            idTypeSelected = idTypesList[e.Position];
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(nameEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a name", "Ok");
                    return;
                } 
                else if (string.IsNullOrEmpty(lastNameEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a last name", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(idNumberEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a id number", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(passwordEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a password", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(emailEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a email", "Ok");
                    return;
                }
                else if (!Validators.ValidateStringIsNumber(idNumberEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Id number should be a number", "Ok");
                    return;
                }
                else if (!Validators.ValidateStringFourToEightCharacters(passwordEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Password should be between 4 and 8 characteres", "Ok");
                    return;
                }
                else if (!Validators.ValidateStringEmail(emailEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a valid email", "Ok");
                    return;
                }
                var user = new UserForAddDto()
                {
                    Name = nameEditText.Text,
                    LastName = lastNameEditText.Text,
                    IdTypeID = idTypeSelected.Id,
                    IdNumber = idNumberEditText.Text,
                    Password = passwordEditText.Text,
                    Email = emailEditText.Text
                };
                DiaglogService.ShowLoading(this);
                var response = await apiService.PostAsync<UserForAddDto>(Settings.ApiUrl, Settings.Prefix, "/Users/AddUser", user, Settings.TokenType, Settings.Token);
                DiaglogService.StopLoading();
                if (response.IsSuccess)
                {
                    Toast.MakeText(this, "User added correctly", ToastLength.Long).Show();
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Fail to add user - " + response.Message, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                DiaglogService.StopLoading();
                Toast.MakeText(this, CommonHelpers.GetErrorMessage("Fail to add user - ", ex), ToastLength.Long).Show();
            }
        }
    }
}