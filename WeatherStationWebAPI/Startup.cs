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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;
using static BCrypt.Net.BCrypt;

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
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            services.AddCors();
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

            app.UseCors(o =>
            o.AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(x => _ = true));

            app.UseRouting();

            app.UseAuthentication();
            
            SeedUsers(context);
            SeedPlaces(context);
            SeedWeatherLog(context);
            
            //SignUpUserToPlace(context);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
                endpoints.MapHub<WeatherHub>("/WeatherHub"); //For SignalR
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
                List<UserDto> users = new List<UserDto>()
                {
                    new ()
                    {
                        FirstName = "Hans",
                        LastName = "Hansen",
                        Email = "test1@testesen.dk",
                        Password = "Sommer25!"
                    },
                    new()
                    {
                        FirstName = "Peter",
                        LastName = "Petersen",
                        Email = "test2@testesen.dk",
                        Password = "Sommer25!"
                    },
                    new()
                    {
                        FirstName = "Kurt",
                        LastName = "Kurtesen",
                        Email = "test3@testesen.dk",
                        Password = "Sommer25!"
                    }
                };

                foreach (var user in users)
                {
                    var userToReg = new User()
                        {Email = user.Email, FirstName = user.FirstName, LastName = user.LastName};

                    userToReg.PwHash = HashPassword(user.Password, 10);
                    context.Users.Add(userToReg);
                    context.SaveChangesAsync();
                }
            }
        }

        public void SeedPlaces(ApplicationDbContext context)
        {
            var exists = context.Places.SingleOrDefault(d => d.PlaceId == 1);

            if (exists != null)
            {

            }
            else
            {
                Place place1 = new Place() {  Latitude = 10.10, Longitude = 10.10, PlaceName = "Himalaya"};
                Place place2 = new Place() {  Latitude = 11.10, Longitude = 11.10, PlaceName = "K2"};
                Place place3 = new Place() {  Latitude = 12.10, Longitude = 12.10, PlaceName = "Randers"};
                context.Places.Add(place1);
                context.Places.Add(place2);
                context.Places.Add(place3);
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
                var places = context.Places.ToList();
                foreach (var place in places)
                {
                    if (place.PlaceId == 1)
                    {
                        DateTime date1 = new DateTime(2021, 10, 8, 8, 00, 00);
                        WeatherLog log1 = new WeatherLog()
                        {
                            LogTime = date1,
                            LogPlace = place,
                            Temperature = 24,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log1);
                        context.SaveChanges();
                        DateTime date2 = new DateTime(2021, 10, 9, 8, 00, 00);
                        WeatherLog log2 = new WeatherLog()
                        {
                            LogTime = date2,
                            LogPlace = place,
                            Temperature = 20,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log2);
                        context.SaveChanges();
                        DateTime date3 = new DateTime(2021, 10, 9, 12, 30, 00);
                        WeatherLog log3 = new WeatherLog()
                        {
                            LogTime = date3,
                            LogPlace = place,
                            Temperature = 21,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log3);
                        context.SaveChanges();
                        DateTime date4 = new DateTime(2021, 10, 9, 16, 40, 00);
                        WeatherLog log4 = new WeatherLog()
                        {
                            LogTime = date4,
                            LogPlace = place,
                            Temperature = 22,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log4);
                        context.SaveChanges();

                        DateTime date5 = new DateTime(2021, 10, 10, 18, 20, 00);
                        WeatherLog log5 = new WeatherLog()
                        {
                            LogTime = date5,
                            LogPlace = place,
                            Temperature = 23,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log5);
                        context.SaveChanges();
                    }

                    if (place.PlaceId == 2)
                    {
                        WeatherLog log1 = new WeatherLog()
                        {
                            LogTime = Convert.ToDateTime("10-10-2021"),
                            LogPlace = place,
                            Temperature = 18,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log1);
                        context.SaveChanges();

                        WeatherLog log2 = new WeatherLog()
                        {
                            LogTime = Convert.ToDateTime("11-10-2021"),
                            LogPlace = place,
                            Temperature = 16,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log2);
                        context.SaveChanges();
                    }

                    if (place.PlaceId == 3)
                    {
                        WeatherLog log1 = new WeatherLog()
                        {
                            LogTime = Convert.ToDateTime("12-10-2021"),
                            LogPlace = place,
                            Temperature = 28,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log1);
                        context.SaveChanges();
                        WeatherLog log2 = new WeatherLog()
                        {
                            LogTime = Convert.ToDateTime("13-10-2021"),
                            LogPlace = place,
                            Temperature = 24,
                            Humidity = 80,
                            AirPressure = 50
                        };

                        context.WeatherLogs.Add(log2);
                        context.SaveChanges();
                    }
                    
                }
                

                //WeatherLog secondlog = new WeatherLog()
                //{
                //    LogTime = Convert.ToDateTime("10-10-2021"),
                //    LogPlace = place2,
                //    Temperature = 24,
                //    Humidity = 80,
                //    AirPressure = 50
                //};

                //context.WeatherLogs.Add(secondlog);
                //context.SaveChanges();

                //WeatherLog thirdlog = new WeatherLog()
                //{
                //    LogTime = Convert.ToDateTime("10-10-2021"),
                //    LogPlace = place3,
                //    Temperature = 24,
                //    Humidity = 80,
                //    AirPressure = 50
                //};

                //context.WeatherLogs.Add(thirdlog);
                //context.SaveChanges();
            }
            
        }

        public void SignUpUserToPlace(ApplicationDbContext context)
        {
            WeatherHub temp = new WeatherHub();

            var tempuser = context.Users.Include(d=>d.SignedUpPlaces).SingleOrDefault(d=>d.UserId==(long)1);

            if (tempuser != null)
            {

                if (!tempuser.SignedUpPlaces.Exists(d=>d.PlaceId==1))
                {
                    var place = context.Places.SingleOrDefault(o => o.PlaceId == 1);

                    tempuser.SignedUpPlaces.Add(place);

                    context.SaveChanges();

                    //temp.JoinGroup(1);
                }
            }

        }
    }
}
