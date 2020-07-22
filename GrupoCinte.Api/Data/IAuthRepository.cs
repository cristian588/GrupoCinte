using GrupoCinte.Common.Entities;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Data
{
    public interface IAuthRepository
    {
        Task<User> Login(string idNumber, string password);
        Task<bool> UserExists(string idNumber);
    }
}
