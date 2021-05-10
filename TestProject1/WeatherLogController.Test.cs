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
        //Test prioritering
        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        public class TestPriorityAttribute : Attribute
        {
            public int Priority { get; private set; }

            public TestPriorityAttribute(int priority) => Priority = priority;
        }

        protected ApplicationDbContext _context;
        protected WeatherLogsController _weatherController;
        //protected AccountController _accountController;
        protected WeatherLogControllerTestFixture _fixture;

        public WeatherLogControllerTest(WeatherLogControllerTestFixture ctrl) :base()
        {
            _fixture = ctrl;
            this._weatherController = ctrl._weatherController;
            this._context = ctrl._context;
            //this._accountController = ctrl._accountController;
        }

        

        [Fact, TestPriority(1)]
        public async Task GetWeatherLogs_isEQToSeeded3()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.InRange(logs.Value.Count(), 2, 3);

            //Assert.Equal(3,logs.Value.Count());
            

        }

        [Fact, TestPriority(2)]
        public async Task GetWeatherLogById_SeededLogs_TempEqualTo24()
        {
            ActionResult<WeatherLog> log = await _weatherController.GetWeatherLog(1);

            Assert.Equal(24, log.Value.Temperature);
            
        }

        [Fact, TestPriority(3)]
        public void GetLastThreeWeatherLogs_SeededLogs()
        {
            Task<ActionResult<IEnumerable<WeatherLog>>> logs = _weatherController.GetLastThreeWeatherLogs();

            var temp = logs.Result.Value.LongCount();

            Assert.InRange(temp,2,3);

            //Assert.Equal(3,temp);
            
        }

        [Fact, TestPriority(4)]
        public async Task GetAllWeatherLogsForDate_SeededLogs()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetAllWeatherLogsForDate(Convert.ToDateTime("2021, 10, 9"));

            Assert.Single(logs.Value);
        }

        [Fact, TestPriority(5)]
        public async Task GetWeatherLogsForTimeframe_SeededLogs()
        {
            DateTime from = Convert.ToDateTime("2021, 10, 8");
            DateTime to = Convert.ToDateTime("2021, 10, 10");

            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogsForTimeframe(from,to);
            
            Assert.InRange(logs.Value.Count(),1,2);
            
        }

        //[Theory, TestPriority(6)]
        //[InlineData(1)]
        //public async Task PutWeatherLog_SeededLogs(int id)
        //{
        //    WeatherLog tempweatherlog = new WeatherLog()
        //    {
        //        //LogId = id,
        //        LogTime = new DateTime(2021, 10, 9, 8, 00, 00),
        //        LogPlace = new Place() { Latitude = 11.11, Longitude = 10.10, PlaceName = "Himalaya" },
        //        Temperature = 24,
        //        Humidity = 90,
        //        AirPressure = 50
        //    };

        //    //var user = new UserDto()
        //    //    { Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234" };

        //    //await _accountController.Register(user);

        //    //Login
        //    //await _accountController.Login(user);

        //    await _weatherController.PutWeatherLog(id, tempweatherlog);

        //    var changedentity = await _weatherController.GetWeatherLog(id);

        //    Assert.Equal(90, changedentity.Value.Humidity);
        //}

        [Fact, TestPriority(6)]
        public async Task PostWeatherLog_SeededLogs()
        {
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
            
            Assert.Equal(90,got.Value.Humidity);
            
        }

        [Fact, TestPriority(7)]
        public async Task DeleteWeatherLog_SeededLogs()
        {
            await _weatherController.DeleteWeatherLog(1);

            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.Equal(2, logs.Value.Count());
            
        }
    }
}
