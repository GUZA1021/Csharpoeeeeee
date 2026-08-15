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

} 
}


