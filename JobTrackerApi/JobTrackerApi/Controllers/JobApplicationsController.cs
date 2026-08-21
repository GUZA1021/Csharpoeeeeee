using JobTrackerApi.Data;
using JobTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobTrackerApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class JobApplicationsController : ControllerBase
    {
        private readonly JobTrackerDbContext _context;

        public JobApplicationsController(JobTrackerDbContext context)
        {
            _context = context;
        }


        [HttpGet(Name = "GetJobApplications")]
        public async Task<IActionResult> Get() {
            var allApplications = await _context.JobApplications.ToListAsync();
            return Ok(allApplications);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JobApplication newApplication)
        {
            var checkUser = User.FindFirst(ClaimTypes.NameIdentifier);
            if (checkUser == null) {
                return Unauthorized();
            }
            
            var userPostAuth = int.Parse(checkUser.Value);
            newApplication.UserId = userPostAuth; // istedet så måske lav ClaimType.id direkte
            
            _context.JobApplications.Add(newApplication);
            await _context.SaveChangesAsync();
            return Ok(newApplication);
        }

        [HttpGet("{id:Int}")]
        public async Task<IActionResult> Get(int id) {
            var jobApplicationId = await _context.JobApplications.FindAsync(id);
            if (jobApplicationId == null)
            {
                return NotFound();
            }

            return Ok(jobApplicationId);

            
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