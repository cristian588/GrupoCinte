using System;
using System.IO;

namespace GrupoCinte.Common.Helpers
{
    public class CommonHelpers
    {
        public static string GetErrorMessage(String message, Exception ex)
        {
            String errorMessage = string.Concat(message, " : ", ex.Message);
            var tempExeption = ex;
            while (!(tempExeption.InnerException == null))
            {
                errorMessage = string.Concat(errorMessage, " || ", tempExeption.InnerException.Message);
                tempExeption = tempExeption.InnerException;
            }
            return errorMessage;
        }

        public static byte[] ConvertByteToArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
    }
}
