using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GrupoCinte.Common.Helpers
{
    public class Validators
    {
        #region Regex String
        private const string nineteenCharacters = @"^.{1,19}";
        private const string fiveToEightCharacters = @"^.{4,8}";
        private const string sixToTwoHundredCharacters = @"^.{6,200}";
        private const string internationalPhoneNumber = @"^((\+\d{1,3}(-| )?\(?\d\)?(-| )?\d{1,5})|(\(?\d{2,6}\)?))(-| )?(\d{3,4})(-| )?(\d{4})(( x| ext)\d{1,5}){0,1}$";
        private const string emailCharacters = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
        private const string isNumber = @"^[0-9]*$";
        private const string zipCodeCharacters = @"^[0-9]{5}(?:-[0-9]{4})?$";
        private const string latitudeCharacters = @"^(\+|-)?(?:90(?:(?:\.0{1,6})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,30})?))$";
        private const string longitudeCharacters = @"^(\+|-)?(?:180(?:(?:\.0{1,6})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,30})?))$";
        #endregion

        #region String

        public static bool ValidateStringNineteenCharacters(string text)
        {
            return Regex.IsMatch(text, nineteenCharacters);
        }
        public static bool ValidateStringFourToEightCharacters(string text)
        {
            return Regex.IsMatch(text, fiveToEightCharacters);
        }
        public static bool ValidateStringSixToTwoHundredCharacters(string text)
        {
            return Regex.IsMatch(text, sixToTwoHundredCharacters);
        }
        public static bool ValidateStringIsNumber(string number)
        {
            return Regex.IsMatch(number, isNumber);
        }
        public static bool ValidateStringEmail(string email)
        {
            return new Regex(emailCharacters).IsMatch(email);
        }
        public static bool ValidateInternationalPhoneNumber(string phone)
        {
            return new Regex(internationalPhoneNumber).IsMatch(phone);
        }
        #endregion

        #region Number
        public static bool ValidateNumbersCharacters(string number, int quantity)
        {
            return Regex.IsMatch(number, $@"^[0-9]{{{quantity}}}$");
        }
        public static bool ValidateNumberNineteenCharacters(long number)
        {
            return Regex.IsMatch(number.ToString(), @"^[0-9]{1,19}$");
        }
        public static bool ValidateNumberPhoneUS(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$");
        }
        public static bool ValidateLatitude(string lat)
        {
            return Regex.IsMatch(lat, latitudeCharacters);
        }
        public static bool ValidateLongitude(string lon)
        {
            return Regex.IsMatch(lon, longitudeCharacters);
        }
        public static bool ValidateZipCode(string lon)
        {
            return Regex.IsMatch(lon, zipCodeCharacters);
        }
        #endregion
    }
}
