using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ThomasConnectionString")));

            services.AddControllers();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

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

            app.UseRouting();

            app.UseAuthentication();

            SeedUsers(context);
            SeedWeatherLog(context);
            SignUpUserToPlace(context);

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
            var exists = context.Users.SingleOrDefault(d=>d.UserId == 1);
            if (exists != null)
            {

            }
            else
            {
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        FirstName = "Hans",
                        LastName = "Hansen",
                        Email = "test1@testesen.dk"
                    },
                    new User()
                    {
                        FirstName = "Peter",
                        LastName = "Petersen",
                        Email = "test2@testesen.dk"
                    },
                    new User()
                    {
                        FirstName = "Kurt",
                        LastName = "Kurtesen",
                        Email = "test3@testesen.dk"
                    }
                };

                foreach (var item in users)
                {
                    context.Users.Add(item);
                }

                context.SaveChanges();
            }
           
        }

        public void SeedWeatherLog(ApplicationDbContext context)
        {
            var exists = context.WeatherLogs.SingleOrDefault(d => d.LogId == 1);

            if (exists != null)
            {

            }
            else
            {
                Place place1 = new Place() { Latitude = 10.10, Longitude = 10.10, PlaceName = "Himalaya" };
                Place place2 = new Place() { Latitude = 11.10, Longitude = 11.10, PlaceName = "K2" };
                Place place3 = new Place() { Latitude = 12.10, Longitude = 12.10, PlaceName = "Randers" };

                WeatherLog firstlog = new WeatherLog()
                {
                    LogTime = Convert.ToDateTime("10-10-2021"),
                    LogPlace = place1,
                    Temperature = 24,
                    Humidity = 80,
                    AirPressure = 50
                };

                context.WeatherLogs.Add(firstlog);
                context.SaveChanges();

                WeatherLog secondlog = new WeatherLog()
                {
                    LogTime = Convert.ToDateTime("10-10-2021"),
                    LogPlace = place2,
                    Temperature = 24,
                    Humidity = 80,
                    AirPressure = 50
                };

                context.WeatherLogs.Add(secondlog);
                context.SaveChanges();

                WeatherLog thirdlog = new WeatherLog()
                {
                    LogTime = Convert.ToDateTime("10-10-2021"),
                    LogPlace = place3,
                    Temperature = 24,
                    Humidity = 80,
                    AirPressure = 50
                };

                context.WeatherLogs.Add(thirdlog);
                context.SaveChanges();
            }
            
        }

        public async void SignUpUserToPlace(ApplicationDbContext context)
        {
            WeatherHub temp = new WeatherHub();

            var exists = context.Users.SingleOrDefault(d => d.UserId == 1);
            if (exists != null)
            {

            }
            else
            {
                var user = context.Users.SingleOrDefault(d => d.UserId == 1);
                var place = context.Places.SingleOrDefault(o => o.PlaceId == 1);

                if (user != null)
                {
                    user.SignedUpPlaces.Add(place);

                    await context.SaveChangesAsync();

                    temp.JoinGroup(1);
                }

            }
        }
    }
}
