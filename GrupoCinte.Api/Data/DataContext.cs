using GrupoCinte.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace GrupoCinte.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<IdType> IdTypes { get; set; }
    }
}
