using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            SeedUsers(context);
            SeedWeatherLog(context);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapHub<WeatherHub>("/weatherHub"); //For SignalR
            });
        }

        public void SeedUsers(ApplicationDbContext context)
        {
            Place place1 = new Place() {Latitude = 10.10, Longitude = 10.10, PlaceName = "Himalaya"};
            Place place2 = new Place() { Latitude = 11.10, Longitude = 11.10, PlaceName = "K2" };
            Place place3 = new Place() { Latitude = 12.10, Longitude = 12.10, PlaceName = "Randers" };

            WeatherLog firstlog = new WeatherLog()
            {
                LogTime =  Convert.ToDateTime("10-10-2021"),
                LogPlace = place1,
                Temperature = 24,
                Humidity = 80,
                AirPressure = 50
            };

            context.WeatherLogs.AddAsync(firstlog);

            WeatherLog secondlog = new WeatherLog()
            {
                LogTime = Convert.ToDateTime("10-10-2021"),
                LogPlace = place2,
                Temperature = 24,
                Humidity = 80,
                AirPressure = 50
            };

            WeatherLog thirdlog = new WeatherLog()
            {
                LogTime = Convert.ToDateTime("10-10-2021"),
                LogPlace = place3,
                Temperature = 24,
                Humidity = 80,
                AirPressure = 50
            };
        }

        public void SeedWeatherLog(ApplicationDbContext context)
        {

        }
    }
}
