using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement1.Models;

namespace TaskManagement1.Data
{
    public class TaskManagementDbContext: IdentityDbContext<AppUser>
    {
        public  TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Messages> Messagess { get; set; }
            
           
    }
}
