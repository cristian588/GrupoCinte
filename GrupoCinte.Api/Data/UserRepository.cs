using GrupoCinte.Common.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Data
{
    public class UserRepository: IUserRepository
    {
        private readonly DataContext Context;
        public UserRepository(DataContext context)
        {
            Context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            Context.Add(entity);
        }
        public void Delete<T>(T entity) where T : class
        {
            Context.Remove(entity);
        }
        public async Task<User> GetUser(int id)
        {
            var user = await Context.Users.Include(p => p.IdType).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await Context.Users.Include(p => p.IdType).ToListAsync();
            return users;
        }
        public async Task<IEnumerable<IdType>> GetUserIdTypes()
        {
            var idTypes = await Context.IdTypes.ToListAsync();
            return idTypes;
        }
        public async Task<bool> SaveAll()
        {
            return await Context.SaveChangesAsync() > 0;
        }
    }
}
