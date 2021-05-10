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
using NSubstitute;
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
        protected ApplicationDbContext _context;
        protected WeatherLogsController _weatherController;
        protected AccountController _accountController;

        public WeatherLogControllerTest(WeatherLogControllerTestFixture ctrl) :base()
        {
            this._weatherController = ctrl._weatherController;
            this._context = ctrl._context;
            this._accountController = ctrl._accountController;
        }

        

        [Fact]
        public async Task GetWeatherLogs_isEQToSeeded3()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.Equal(3, logs.Value.Count());

        }

        [Theory]
        [InlineData(1)]
        public async Task GetWeatherLogById_SeededLogs(int id)
        {
            ActionResult<WeatherLog> log = await _weatherController.GetWeatherLog(id);

            Assert.Equal(24, log.Value.Temperature);
        }

        [Theory]
        [InlineData()]
        public async Task GetLastThreeWeatherLogs_SeededLogs()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetLastThreeWeatherLogs();

            Assert.Equal(3, logs.Value.Count());
        }

        [Theory]
        [InlineData("2021, 10, 8")]
        public async Task GetAllWeatherLogsForDate_SeededLogs(DateTime date)
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetAllWeatherLogsForDate(date);

            Assert.Equal(1, logs.Value.Count());
        }

        [Theory]
        [InlineData("2021, 10, 8", "2021, 10, 10")]
        public async Task GetWeatherLogsForTimeframe_SeededLogs(DateTime from, DateTime to)
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogsForTimeframe(from,to);

            Assert.Equal(2, logs.Value.Count());
        }

        [Theory]
        [InlineData(1)]
        public async Task PutWeatherLog_SeededLogs(int id)
        {

            WeatherLog tempweatherlog = new WeatherLog()
            {
                LogTime = new DateTime(2021, 10, 9, 8, 00, 00),
                LogPlace = new Place() { Latitude = 11.11, Longitude = 10.10, PlaceName = "Himalaya" },
                Temperature = 24,
                Humidity = 90,
                AirPressure = 50
            };

            var user = new UserDto()
                { Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234" };

            await _accountController.Register(user);

            //Login
            await _accountController.Login(user);

            await _weatherController.PutWeatherLog(id, tempweatherlog);

            var changedentity = await _weatherController.GetWeatherLog(id);

            Assert.Equal(90, changedentity.Value.Humidity);
        }

        [Fact]
        public async Task PostWeatherLog_SeededLogs()
        {
            WeatherLogDto tempweatherlog = new WeatherLogDto()
            {
                LogTime = new DateTime(2021, 10, 9, 8, 00, 00),
                Temperature = 24,
                Humidity = 90,
                AirPressure = 85
            };

            //Login

            var res = await _weatherController.PostWeatherLog(tempweatherlog);

            var got = await _weatherController.GetWeatherLog(4);

            Assert.Equal(85,got.Value.AirPressure);
        }

        [Theory]
        [InlineData(3)]
        public async Task DeleteWeatherLog_SeededLogs(int id)
        {

        }

        [Theory]
        [InlineData(2)]
        public async Task WeatherLogExists_SeededLogs(int id)
        {

        }
    }
}
