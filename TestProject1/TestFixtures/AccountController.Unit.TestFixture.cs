using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.XUnit.TestFixtures
{
    public class AccountControllerTestFixture : IDisposable
    {
        public ApplicationDbContext _context;
        public IOptions<AppSettings> _appSettings;
        public IHubContext<WeatherHub> _mockHub;
        public AccountController _accountController;

        public AccountControllerTestFixture()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NGKWeatherAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options);
            
            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _appSettings = Options.Create(settings);

            _mockHub = Substitute.For<IHubContext<WeatherHub>>();

            _accountController = new AccountController(_context, _appSettings, _mockHub);
        }

        public void Dispose()
        {
            
        }
    }


}
