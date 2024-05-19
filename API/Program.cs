using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<TrackerContext>(opt =>
            {
                opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
           /* if (app.Environment.IsDevelopment())
            {*/
                app.UseSwagger();
                app.UseSwaggerUI();
           /* }*/

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<TrackerContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                await context.Database.MigrateAsync();
                await ExpenseTrackerContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error ocured during migration");
            }
            app.Run();
        }
    }
}