using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Helpers
{
    public class Helpers
    {
        #region Exceptions
        public static string GetErrorMessage(String message, Exception ex)
        {
            String errorMessage = string.Concat(message, " : ", ex.Message);
            if (ex.GetType() == typeof(DbUpdateException))
            {
                DbUpdateException _dbex = (DbUpdateException)ex;
                var _ex = ex;
                while (!(_ex.InnerException == null))
                {
                    errorMessage = string.Concat(errorMessage, " || ", _ex.InnerException.Message);
                    _ex = _ex.InnerException;
                }
                foreach (var _error in _dbex.Data)
                {
                    errorMessage = String.Concat(errorMessage, " || ", _error.ToString());
                }
            }
            else
            {
                var _ex = ex;
                while (!(_ex.InnerException == null))
                {
                    errorMessage = string.Concat(errorMessage, " || ", _ex.InnerException.Message);
                    _ex = _ex.InnerException;
                }
            }
            return errorMessage;
        }

        #endregion
    }
}
