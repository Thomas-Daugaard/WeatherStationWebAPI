using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.Unit
{
    public class WeatherLogControllerTest
    {
        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        private WeatherLogsController _weatherController;
        private Place _place;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite(CreateInMemoryDatabase()).Options);
            //.UseSqlServer(CreateInMemoryDatabase()).Options); //Real DB 
            //Fake db: UseSqlite(CreateInMemoryDatabase()).Options);

            //In memory DB for testing purpose
            static DbConnection CreateInMemoryDatabase()
                    {
                        var connection = new SqliteConnection("Filename=:memory:");//Fake db

                //Real DB:
                        //var connection = new SqlConnection("Data Source=localhost;Initial Catalog=NGKWebApiWeatherLog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                        connection.Open();
                        return connection;
                    }

            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };


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

            _place = Substitute.For<Place>();

            _appSettings = Options.Create(settings);

            _mockHub = Substitute.For<IHubContext<WeatherHub>>();

            _weatherController = new WeatherLogsController(_context, _mockHub);
        }

        [Test]
        public async Task GetWeatherLogs_isEQToSeeded9()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.AreEqual(2, logs.Value.Count());

        }

        [TestCase(1)]
        public async Task GetWeatherLogById_SeededLogs(int id)
        {
            ActionResult<WeatherLog> log = await _weatherController.GetWeatherLog(id);

            Assert.AreEqual(24, log.Value.Temperature);
        }

        [Test]
        public async Task GetLastThreeWeatherLogs_SeededLogs()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetLastThreeWeatherLogs();

            Assert.AreEqual(3, logs.Value.Count());
        }

        [Test]
        public async Task GetAllWeatherLogsForDate_SeededLogs(DateTime date)
        {
            
        }

        [Test]
        public async Task GetWeatherLogsForTimeframe_SeededLogs(DateTime from, DateTime to)
        {

        }

        [Test]
        public async Task PutWeatherLog_SeededLogs(int id, WeatherLog log)
        {

        }

        [Test]
        public async Task PostWeatherLog_SeededLogs(WeatherLogDto weatherLog)
        {

        }

        [Test]
        [TestCase(8)]
        public async Task DeleteWeatherLog_SeededLogs(int id)
        {

        }

        [Test]
        [TestCase(7)]
        public async Task WeatherLogExists_SeededLogs(int id)
        {

        }

    }
}
