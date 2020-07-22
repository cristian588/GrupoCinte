using GrupoCinte.Common.Entities;
using System.Linq;

namespace GrupoCinte.Api.Data
{
    public class Seed
    {
        private readonly DataContext _context;
        public Seed(DataContext context)
        {
            _context = context;
        }
        public void SeedUsers()
        {
            if (!_context.IdTypes.Any())
            {
                _context.IdTypes.Add(new IdType() { Id = 1, Name = "CC"});
                _context.IdTypes.Add(new IdType() { Id = 2, Name = "RC" });
                _context.IdTypes.Add(new IdType() { Id = 3, Name = "TI" });
                _context.IdTypes.Add(new IdType() { Id = 4, Name = "CE" });
                _context.IdTypes.Add(new IdType() { Id = 5, Name = "PA" });
                _context.SaveChanges();
            }
            if (!_context.Users.Any())
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("Admin", out passwordHash, out passwordSalt);
                _context.Users.Add(new User() { 
                    Id = 1,
                    IdNumber = "11111111",
                    Name = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    IdTypeID = 1
                });
                _context.SaveChanges();
            }


        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
