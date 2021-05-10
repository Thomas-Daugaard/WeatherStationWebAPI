using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.XUnit.TestFixtures
{
    public class AccountControllerTestFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }
        public AccountController AccountController { get; private set; }

        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        

        public AccountControllerTestFixture()
        {
            Context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NGKWeatherAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options);
            
            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _appSettings = Options.Create(settings);

            _mockHub = Substitute.For<IHubContext<WeatherHub>>();

            AccountController = new AccountController(Context, _appSettings, _mockHub);
        }

        public void Dispose()
        {
            var user = new UserDto()
                { Email = "ml@somemail.com", Password = "Password1234" };

            // Act
            var userToDelete = Context.Users.SingleOrDefault(u => u.Email == user.Email);

            if (userToDelete != null)
                Context.Users.Remove(userToDelete);
            Context.SaveChanges();
        }
    }
}
