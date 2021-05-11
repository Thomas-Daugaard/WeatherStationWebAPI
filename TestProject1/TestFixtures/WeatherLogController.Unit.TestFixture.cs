using System;
using System.Data.Common;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.XUnit.TestFixtures
{
    public class WeatherLogControllerTestFixture : IDisposable
    {
        public ApplicationDbContext _context { get; set; }
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        public WeatherLogsController _weatherController { get; set; }
        private Place _place;

        public WeatherLogControllerTestFixture()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);

            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _place = Substitute.For<Place>();

            _appSettings = Options.Create(settings);

            _mockHub = Substitute.For<IHubContext<WeatherHub>>();

            _weatherController = new WeatherLogsController(_context, _mockHub);

            Seed();
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            _context.WeatherLogs.AddRange(
                new WeatherLog()
                {
                    LogTime = new DateTime(2021, 10, 8, 8, 00, 00),
                    LogPlace = new Place() { Latitude = 10.10, Longitude = 10.10, PlaceName = "Himalaya" },
                    Temperature = 24,
                    Humidity = 80,
                    AirPressure = 50
                },
                new WeatherLog()
                {
                    LogTime = new DateTime(2021, 10, 9, 9, 00, 00),
                    LogPlace = new Place() { Latitude = 12.12, Longitude = 12.12, PlaceName = "K2" },
                    Temperature = 1,
                    Humidity = 50,
                    AirPressure = 90
                },
                new WeatherLog()
                {
                    LogTime = new DateTime(2021, 11, 9, 9, 00, 00),
                    LogPlace = new Place() { Latitude = 14.14, Longitude = 14.14, PlaceName = "K10" },
                    Temperature = 5,
                    Humidity = 55,
                    AirPressure = 97
                });
            _context.SaveChanges();
        }

        static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");//Fake db

            //Real DB:
            //var connection = new SqlConnection("Data Source=localhost;Initial Catalog=NGKWebApiWeatherLog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();
            return connection;
        }

        public void Dispose()
        {
        }
    }


}
