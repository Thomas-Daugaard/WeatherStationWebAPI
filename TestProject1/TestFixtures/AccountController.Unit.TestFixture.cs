using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.XUnit.TestFixtures
{
    public class AccountControllerTestFixture : IDisposable
    {
        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        private AccountController _accountController;

        public AccountControllerTestFixture()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NGKWeatherAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options);
        }

        public void Dispose()
        {
            
        }
    }


}
