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
    public class UpdateUserActivity : Activity
    {
        EditText nameEditText, lastNameEditText, idNumberEditText, emailEditText;
        Spinner idTypesSpinner;
        Button saveButton, deleteButton;
        ApiService apiService;
        
        private List<IdTypeForDetailedDto> idTypesList;
        private IdTypeForDetailedDto idTypeSelected;
        UserForDetailedDto userInformation;
        String userId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.update_user);
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
            saveButton = FindViewById<Button>(Resource.Id.saveButton);
            deleteButton = FindViewById<Button>(Resource.Id.deleteButton);
        }

        private void HandleEvents()
        {
            saveButton.Click += SaveButton_Click;
            deleteButton.Click += DeleteButton_Click;
        }

        private void SetInitialData()
        {
            apiService = new ApiService();
            userId = Intent.GetStringExtra("userId");
        }

        private async void LoadData()
        {
            try
            {
                DiaglogService.ShowLoading(this);
                var userResponse = await apiService.GetAsync<UserForDetailedDto>(Settings.ApiUrl, Settings.Prefix, "/Users/GetUser/" + userId, "Bearer", Settings.Token);
                if (!userResponse.IsSuccess)
                {
                    DiaglogService.StopLoading();
                    DiaglogService.ShowMessage(this, "Error loading user", userResponse.Message, "Accept");
                    return;
                }
                userInformation = JsonConvert.DeserializeObject<UserForDetailedDto>(userResponse.Result.ToString());
                if (userInformation != null)
                {
                    nameEditText.Text = userInformation.Name;
                    lastNameEditText.Text = userInformation.LastName;
                    idNumberEditText.Text = userInformation.IdNumber;
                    emailEditText.Text = userInformation.Email;
                }


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
                    var usersAdapter = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleDropDownItem1Line, idTypesList.Select(x => x.Name).ToList());
                    idTypeSelected = idTypesList.FirstOrDefault(x => x.Id == userInformation.IdTypeID);
                    idTypesSpinner.Adapter = usersAdapter;
                    idTypesSpinner.ItemSelected += IdTypesSpinner_ItemSelected;

                    int spinnerPosition = usersAdapter.GetPosition(idTypeSelected.Name);
                    idTypesSpinner.SetSelection(spinnerPosition);

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
                else if (!Validators.ValidateStringEmail(emailEditText.Text))
                {
                    DiaglogService.ShowMessage(this, "Warining", "Please enter a valid email", "Ok");
                    return;
                }
                else if (userInformation.Name == nameEditText.Text && userInformation.LastName == lastNameEditText.Text 
                    && userInformation.IdNumber == idNumberEditText.Text && userInformation.Email == emailEditText.Text
                    && userInformation.IdTypeID == idTypeSelected.Id)
                {
                    DiaglogService.ShowMessage(this, "Warining", "Information has not been changed", "Ok");
                    return;
                }
                var user = new UserForUpdateDto()
                {
                    Id = Convert.ToInt32(userId),
                    Name = nameEditText.Text,
                    LastName = lastNameEditText.Text,
                    IdTypeID = idTypeSelected.Id,
                    IdNumber = idNumberEditText.Text,
                    Email = emailEditText.Text
                };
                DiaglogService.ShowLoading(this);
                var response = await apiService.PostAsync<UserForUpdateDto>(Settings.ApiUrl, Settings.Prefix, "/Users/UpdateUser", user, Settings.TokenType, Settings.Token);
                DiaglogService.StopLoading();
                if (response.IsSuccess)
                {
                    Toast.MakeText(this, "User updated correctly", ToastLength.Long).Show();
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Fail to update user - " + response.Message, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                DiaglogService.StopLoading();
                Toast.MakeText(this, CommonHelpers.GetErrorMessage("Fail to update user - ", ex), ToastLength.Long).Show();
            }
        }

        private async void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                var user = new UserForUpdateDto()
                {
                    Id = Convert.ToInt32(userId)
                };
                DiaglogService.ShowLoading(this);
                var response = await apiService.PostAsync<UserForUpdateDto>(Settings.ApiUrl, Settings.Prefix, "/Users/DeleteUser", user, Settings.TokenType, Settings.Token);
                DiaglogService.StopLoading();
                if (response.IsSuccess)
                {
                    Toast.MakeText(this, "User deleted correctly", ToastLength.Long).Show();
                    SetResult(Result.Ok);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Fail to delete user - " + response.Message, ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {
                DiaglogService.StopLoading();
                Toast.MakeText(this, CommonHelpers.GetErrorMessage("Fail to delete user - ", ex), ToastLength.Long).Show();
            }
        }
    }
}