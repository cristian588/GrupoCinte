using GrupoCinte.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Data
{
    public class AuthRepository : IAuthRepository
    {
        public readonly DataContext Context;
        public AuthRepository(DataContext context)
        {
            Context = context;
        }
        public async Task<User> Login(string idNumber, string password)
        {
            var user = await Context.Users.FirstOrDefaultAsync(x => x.IdNumber == idNumber);
            if (user == null) return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) return null;
            return user;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computeHash.Length; i++)
                    if (computeHash[i] != passwordHash[i]) return false;
            }
            return true;
        }
        public async Task<bool> UserExists(string idNumber)
        {
            return await Context.Users.AnyAsync(x => x.IdNumber == idNumber);
        }
    }
}
