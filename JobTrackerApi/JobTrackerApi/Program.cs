
using JobTrackerApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace JobTrackerApi

{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("===== KØRER NYESTE VERSION =====");
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddDbContext<JobTrackerDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = "JobTrackerApi",
                        ValidateAudience = true,
                        ValidAudience = "MyAudience",
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("JWT FEJL: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.WriteLine("JWT ok: " + context);
                            return Task.CompletedTask;
                        },
                    };
                }); 
            
            var app = builder.Build();

            if (!app.Environment.IsProduction())
            {
                app.UseExceptionHandler(exceptionHandlerApp =>
                {
                    exceptionHandlerApp.Run(async context =>
                    {
                        var response = context.Response;
                        //Set response Contentype
                        response.ContentType = "application/json";

                        //get the exception objekt
                        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                        response.StatusCode = 400;

                        await response.WriteAsJsonAsync(exceptionFeature.Error.Message);
                    });
                });
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();

        }
    }
}
