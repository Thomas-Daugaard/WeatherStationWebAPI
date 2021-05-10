using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Test.XUnit.TestFixtures;
using WeatherStationWebAPI.WebSocket;
using Xunit;

namespace WeatherStationWebAPI.Test.XUnit
{
    public class WeatherLogControllerTest : IClassFixture<WeatherLogControllerTestFixture>
    {
        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        private WeatherLogsController _weatherController;
        private Place _place;

        public WeatherLogControllerTest() : base(
            new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(CreateInMemoryDatabase())
            .Options)
        {

        }

        static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");//Fake db

            //Real DB:
            //var connection = new SqlConnection("Data Source=localhost;Initial Catalog=NGKWebApiWeatherLog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            connection.Open();
            return connection;
        }

        [Fact]
        public async Task GetWeatherLogs_isEQToSeeded9()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.Equal(2, logs.Value.Count());

        }

        [InlineData(1)]
        public async Task GetWeatherLogById_SeededLogs(int id)
        {
            ActionResult<WeatherLog> log = await _weatherController.GetWeatherLog(id);

            Assert.Equal(24, log.Value.Temperature);
        }

        [InlineData()]
        public async Task GetLastThreeWeatherLogs_SeededLogs()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetLastThreeWeatherLogs();

            Assert.Equal(3, logs.Value.Count());
        }

        [InlineData()]
        public async Task GetAllWeatherLogsForDate_SeededLogs(DateTime date)
        {

        }

        [InlineData()]
        public async Task GetWeatherLogsForTimeframe_SeededLogs(DateTime from, DateTime to)
        {

        }

        [InlineData()]
        public async Task PutWeatherLog_SeededLogs(int id, WeatherLog log)
        {

        }

        [InlineData()]
        public async Task PostWeatherLog_SeededLogs(WeatherLogDto weatherLog)
        {

        }
        
        [InlineData(8)]
        public async Task DeleteWeatherLog_SeededLogs(int id)
        {

        }
        
        [InlineData(7)]
        public async Task WeatherLogExists_SeededLogs(int id)
        {

        }
    }
}
