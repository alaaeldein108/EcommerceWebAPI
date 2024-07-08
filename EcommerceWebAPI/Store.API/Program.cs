using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.API.Extentions;
using Store.API.Helper;
using Store.API.Middlewares;
using Store.Data.Context;
using Store.Repository.Interfaces;
using Store.Repository.Repositories;
using Store.Service.HandleResponse;
using Store.Service.Services.ProductService;
using Store.Service.Services.ProductService.Dtos;

namespace Store.API
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
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.AddAplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //builder.Services.AddSwaggerDocumention();
            var app = builder.Build();

            await ApplySeeding.ApplySeedingAsync(app);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                /* app.UseSwaggerUI(options =>
                 {
                     options.SwaggerEndpoint("/swagger/v1/swagger.json","MyAPI v1");
                 });*/
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>(); 
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
