using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MunicipalitiesTaxes.Database;
using MunicipalitiesTaxes.Implementations;
using MunicipalitiesTaxes.Interfaces;
using MunicipalitiesTaxes.Model;
using static System.Net.Mime.MediaTypeNames;

namespace MunicipalitiesTaxes
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MunicipalitiesTaxesDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MunicipalitiesTaxesDbContext")));
            builder.Services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            builder.Services.AddSingleton<ITaxRecordOperations, TaxRecordOperations>();
            builder.Services.AddScoped<MunicipalitiesManager>();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();


            builder.Services.AddControllers();

            var app = builder.Build();
            app.UseAuthorization();
            app.MapControllers();

            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                    context.Response.ContentType = Text.Plain;
                    await context.Response.WriteAsync("An exception was thrown.");

                    var exceptionHandlerPathFeature =
                        context.Features.Get<IExceptionHandlerPathFeature>();
                });
            });

            app.Run();
        }
    }
}
