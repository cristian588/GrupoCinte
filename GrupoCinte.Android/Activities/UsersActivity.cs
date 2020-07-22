
using Android.App;
using Android.OS;
using GrupoCinte.Android.Helpers;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Services;
using GrupoCinte.Common.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;
using Android.Widget;
using Android.Content;
using System.Linq;
using Android.Runtime;

namespace GrupoCinte.Android.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = false)]
    public class UsersActivity : Activity
    {
        TextView addUserTextView;
        ImageView exitImageView;
        ListView usersListView;
        private ApiService apiService;
        private List<UserForListDto> usersList;
        private UserForListDto userSelected;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.users);
            FindViews();
            HandleEvents();
            SetInitialData();
            LoadData();
        }

        #region Initial methods
        private void FindViews()
        {
            addUserTextView = FindViewById<TextView>(Resource.Id.addUserTextView);
            exitImageView = FindViewById<ImageView>(Resource.Id.exitImageView);
            usersListView = FindViewById<ListView>(Resource.Id.usersListView);
        }

        private void HandleEvents()
        {
            addUserTextView.Click += AddUserTextView_Click;
            exitImageView.Click += ExitImageView_Click;
            usersListView.ItemClick += UsersListView_ItemClick;
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
                var response = await apiService.GetListAsync<UserForListDto>(Settings.ApiUrl, Settings.Prefix, "/Users/GetUsers", "Bearer", Settings.Token);
                if (!response.IsSuccess)
                {
                    DiaglogService.StopLoading();
                    DiaglogService.ShowMessage(this, "Error loading users", response.Message, "Accept");
                    return;
                }
                usersList = JsonConvert.DeserializeObject<List<UserForListDto>>(response.Result.ToString());
                usersListView.Adapter = new UsersCustomListAdapter(usersList);

                DiaglogService.StopLoading();
            }
            catch (System.Exception ex)
            {
                DiaglogService.StopLoading();
                DiaglogService.ShowMessage(this, "Error", CommonHelpers.GetErrorMessage("Error loading users", ex), "Ok");
            }
        }
        #endregion

        private void UsersListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            userSelected = usersList.ElementAt(e.Position);
            Intent intent = new Intent(this, typeof(UpdateUserActivity));
            intent.PutExtra("userId", userSelected.Id.ToString());
            StartActivityForResult(intent, 1);
        }

        private void ExitImageView_Click(object sender, System.EventArgs e)
        {
            Settings.Token = string.Empty;
            Settings.Name = string.Empty;
            Settings.LastName = string.Empty;
            Finish();
            var intent = new Intent(this, typeof(LoginActivity));
            StartActivity(intent);
        }

        private void AddUserTextView_Click(object sender, System.EventArgs e)
        {
            var intent = new Intent(this, typeof(AddUserActivity));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 0 || requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    LoadData();
                }
            }
        }
    }
}