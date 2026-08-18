using Microsoft.EntityFrameworkCore;
using JobTrackerApi.Models;


namespace JobTrackerApi.Data
{
    public class JobTrackerDbContext : DbContext
    {
        public JobTrackerDbContext(DbContextOptions <JobTrackerDbContext>  options): base(options)
        {
        }

        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JobApplication>()
                .HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        }
    } 
}


