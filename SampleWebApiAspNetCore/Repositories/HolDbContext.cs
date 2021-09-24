using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HolDbContext : DbContext
    {
        public HolDbContext(DbContextOptions<HolDbContext> options)
           : base(options)
        {

        }

        public DbSet<EmployeeEntity> Employees { get; set; }

    }
}
