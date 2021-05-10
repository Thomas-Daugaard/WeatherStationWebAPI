﻿using System;
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

        public WeatherLogControllerTest(WeatherLogControllerTestFixture ctrl) :base()
        {
            this._weatherController = ctrl._weatherController;
            this._context = ctrl._context;
        }

        

        [Fact]
        public async Task GetWeatherLogs_isEQToSeeded3()
        {
            ActionResult<IEnumerable<WeatherLog>> logs = await _weatherController.GetWeatherLogs();

            Assert.Equal(3, logs.Value.Count());

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
