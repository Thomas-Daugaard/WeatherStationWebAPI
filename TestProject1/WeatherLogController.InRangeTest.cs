using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Test.XUnit.TestFixtures;
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
        protected WeatherLogControllerTestFixture _fixture;

        public WeatherLogControllerTest(WeatherLogControllerTestFixture ctrl) :base()
        {
            _fixture = ctrl;
            this._weatherController = ctrl._weatherController;
            this._context = ctrl._context;
        }


        [Fact, TestPriority(1)]
        public async Task GetWeatherLogs_isEQToSeeded3()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.InRange(logs.Value.Count(), 2, 3);

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
