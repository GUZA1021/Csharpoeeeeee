namespace JobTrackerApi.Tests;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Threading.RateLimiting;
using JobTrackerApi.Data;
using JobTrackerApi.Models;
using JobTrackerApi.Controllers;
using Microsoft.AspNetCore.Mvc;

public class UnitTest1
{
    [Fact]
    public async Task Test1()
    { //Testing if it actually orders applications my id
        //arrange
        var options = new DbContextOptionsBuilder<JobTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase1")
            .Options;

        var context = new JobTrackerDbContext(options);
        var job1 = new JobApplication(3, "Microsoft", "Head Leader");
        var job2 = new JobApplication(1, "Google", "Leader");
        var job3 = new JobApplication(2,"Youtube", "second Leader");

        context.JobApplications.Add(job1);
        context.JobApplications.Add(job2);
        context.JobApplications.Add(job3);
        context.SaveChanges();

        //ACT
        JobApplicationsController controller = new JobApplicationsController(context);
        var result = await controller.Get();
        var okResult = result as OkObjectResult;
        var applications = okResult.Value as List<JobApplication>;

        //assert
        Assert.Equal(1, applications[0].Id);
        Assert.Equal(2, applications[1].Id);
        Assert.Equal(3, applications[2].Id);
    }

    [Fact]
    public async Task test2()
    { //testing that deleting a job actually works
        //arrange
        var options = new DbContextOptionsBuilder<JobTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase2")
            .Options;

        var context = new JobTrackerDbContext(options);
        var job1 = new JobApplication(3, "Microsoft", "Head Leader");
        var job2 = new JobApplication(1, "Google", "Leader");
        var job3 = new JobApplication(2, "Youtube", "second Leader");

        context.JobApplications.Add(job1);
        context.JobApplications.Add(job2);
        context.JobApplications.Add(job3);
        context.SaveChanges();

        //ACT
        JobApplicationsController controller = new JobApplicationsController(context);
        var result = await controller.DeleteJobapp(job1.Id);

        //assert
        Assert.Equal(2, context.JobApplications.Count());
    }

    [Fact]
    public async Task test3()
    { //Testing if my not found works correct
        //arrange
        var options = new DbContextOptionsBuilder<JobTrackerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase3")
            .Options;

        var context = new JobTrackerDbContext(options);
        var job1 = new JobApplication(3, "Microsoft", "Head Leader");
        var job2 = new JobApplication(1, "Google", "Leader");
        var job3 = new JobApplication(2, "Youtube", "second Leader");

        context.JobApplications.Add(job1);
        context.JobApplications.Add(job2);
        context.JobApplications.Add(job3);
        context.SaveChanges();

        //ACT
        JobApplicationsController controller = new JobApplicationsController(context);
        var result = await controller.DeleteJobapp(999);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }
}
