using Microsoft.AspNetCore.Mvc;
using JobTrackerApi.Models;
using JobTrackerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly JobTrackerDbContext _context;

        public JobApplicationsController(JobTrackerDbContext context)
        {
            _context = context;
        }


        [HttpGet(Name = "GetJobApplications")]
        public IEnumerable<JobApplication> Get() {
            return _context.JobApplications;
        }

        [HttpPost]
        public IActionResult Post([FromBody] JobApplication newApplication)
        {
            _context.JobApplications.Add(newApplication);
            _context.SaveChanges();
            return Ok(newApplication);
        }

        [HttpDelete("{id:Int}")]
        public async Task<IActionResult> DeleteJobapp(int id)
        {
            if (id < 1)
            {
                return BadRequest();
            }

            var job = await _context.JobApplications.FindAsync(id);

            if (job == null) {
                return NotFound();
            }

            _context.JobApplications.Remove(job);
            await _context.SaveChangesAsync();

            return Ok();

        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateApplication(int id, [FromBody] JobApplication jobapplication)
        {

            var original = await _context.JobApplications.FindAsync(id);
            if (original == null)
            {
                return NotFound();

            }
            original.Position = jobapplication.Position;
            original.Status = jobapplication.Status;
            original.Company = jobapplication.Company;

            _context.Update(original);
            await _context.SaveChangesAsync();

            return Ok(original);

        }
       


    }
        
}