using GrupoCinte.Common.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrupoCinte.Api.Data
{
    public interface IUserRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<IdType>> GetUserIdTypes();
        Task<User> GetUser(int id);
    }
}
