using Microsoft.EntityFrameworkCore;
using JobManagement.Models;

namespace JobManagement.Data
{
    public class JobManagementDbContext : DbContext
    {
        public JobManagementDbContext(DbContextOptions<JobManagementDbContext> options) : base(options)
        {
        }

        public DbSet<User> User { get; set; }
        public DbSet<Job> Job { get; set; }
    }
}
