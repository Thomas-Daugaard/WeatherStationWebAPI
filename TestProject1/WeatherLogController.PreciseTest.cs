using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Test.XUnit.TestFixtures;
using WeatherStationWebAPI.WebSocket;
using Xunit;

namespace WeatherStationWebAPI.Test.XUnit
{
    public class WeatherLogControllerUnitTest
    {

        protected ApplicationDbContext _context { get; set; }
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        protected WeatherLogsController _weatherController { get; set; }
        protected Place _place { get; set; }


        public WeatherLogControllerUnitTest() 
        {

            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _place = Substitute.For<Place>();

            _appSettings = Options.Create(settings);

        }


        [Fact]
        public async Task GetWeatherLogs_isEQToSeeded3()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            //Act
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            //Assert
            Assert.Equal(3,logs.Value.Count());

            Dispose();
        }

        [Fact]
        public async Task GetWeatherLogById_SeededLogs_TempEqualTo24()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            ActionResult<WeatherLog> log = await _weatherController.GetWeatherLog(1);

            Assert.Equal(24, log.Value.Temperature);

            Dispose();
        }

        [Fact]
        public void GetLastThreeWeatherLogs_SeededLogs()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            Task<ActionResult<IEnumerable<WeatherLog>>> logs = _weatherController.GetLastThreeWeatherLogs();

            var temp = logs.Result.Value.LongCount();

            Assert.Equal(3,temp);

            Dispose();

        }

        [Fact]
        public async Task GetAllWeatherLogsForDate_SeededLogs()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetAllWeatherLogsForDate(Convert.ToDateTime("2021, 10, 9"));

            Assert.Single(logs.Value);

            Dispose();
        }

        [Fact]
        public async Task GetWeatherLogsForTimeframe_SeededLogs()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            DateTime from = Convert.ToDateTime("2021, 10, 8");
            DateTime to = Convert.ToDateTime("2021, 10, 10");

            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogsForTimeframe(from, to);

            Assert.Equal(2,logs.Value.Count());

            Dispose();

        }

        [Fact]
        public async Task PostWeatherLog_SeededLogs()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            WeatherLogDto tempweatherlog = new WeatherLogDto()
            {
                LogTime = new DateTime(2021, 10, 9, 8, 00, 00),
                LogPlaceId = 1,
                Temperature = 24,
                Humidity = 90,
                AirPressure = 85
            };

            var res = await _weatherController.PostWeatherLog(tempweatherlog);

            var got = await _weatherController.GetWeatherLog(4);

            Assert.Equal(90, got.Value.Humidity);

            Dispose();

        }

        [Fact]
        public async Task DeleteWeatherLog_SeededLogs()
        {
            //Arrange
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            _mockHub = Substitute.For<IHubContext<WeatherHub>>();
            _weatherController = new WeatherLogsController(_context, _mockHub);
            Seed();

            await _weatherController.DeleteWeatherLog(1);

            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.Equal(2, logs.Value.Count());

            Dispose();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            _context.WeatherLogs.AddRange(
                new WeatherLog()
                {
                    LogId = 1,
                    LogTime = new DateTime(2021, 10, 8, 8, 00, 00),
                    LogPlace = new Place() { Latitude = 10.10, Longitude = 10.10, PlaceName = "Himalaya" },
                    Temperature = 24,
                    Humidity = 80,
                    AirPressure = 50
                },
                new WeatherLog()
                {
                    LogId = 2,
                    LogTime = new DateTime(2021, 10, 9, 9, 00, 00),
                    LogPlace = new Place() { Latitude = 12.12, Longitude = 12.12, PlaceName = "K2" },
                    Temperature = 1,
                    Humidity = 50,
                    AirPressure = 90
                },
                new WeatherLog()
                {
                    LogId = 3,
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
    }
}
