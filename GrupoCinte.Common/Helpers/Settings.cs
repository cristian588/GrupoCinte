using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace GrupoCinte.Common.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        private const string token = "token";
        private const string name = "name";
        private const string lastName = "lastName";
        private const string apiUrl = "koachApiUrl";
        private static readonly string stringDefault = string.Empty;
        #endregion

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(token, stringDefault);
            set => AppSettings.AddOrUpdateValue(token, value);
        }

        public static string Name
        {
            get => AppSettings.GetValueOrDefault(name, stringDefault);
            set => AppSettings.AddOrUpdateValue(name, value);
        }

        public static string LastName
        {
            get => AppSettings.GetValueOrDefault(lastName, stringDefault);
            set => AppSettings.AddOrUpdateValue(lastName, value);
        }

        public static string ApiUrl
        {
            get => "https://34812c3119e1.ngrok.io";
            set => AppSettings.AddOrUpdateValue(apiUrl, value);
        }

        public static string Prefix
        {
            get => "/api";
        }

        public static string TokenType
        {
            get => "Bearer";
            set => AppSettings.AddOrUpdateValue(token, value);
        }
    }
}
