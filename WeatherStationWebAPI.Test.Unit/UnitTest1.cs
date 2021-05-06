using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;

namespace WeatherStationWebAPI.Test.Unit
{
    public class Tests
    {
        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private AccountController _uut;

        [SetUp]
        public void Setup()
        {
            var context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=NGKWeatherAPI;Trusted_Connection=True;MultipleActiveResultSets=true").Options);


            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };
            _appSettings = Options.Create(settings);

            //_uut = new AccountController(_context, _appSettings);
        }

        [Test]
        public void Register_RegisterUser_ReceivedCorrectStatusCode()
        {
            var user = new UserDto()
                { FirstName = "Kurt", LastName = "Poulsen", Email = "kp@somemail.com", Password = "Password1234" };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44301/api/");
                var postTask = client.PostAsJsonAsync<UserDto>("account/register", user);
                postTask.Wait();

                var result = postTask.Result;
                Assert.That(result.StatusCode, Is.EqualTo("Created"));
            }
        }

        [Test]
        public async Task Test2()
        {
            var response = await _uut.Register(new UserDto()
                {Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234"});

            Assert.That(response.Result, Is.EqualTo(""));
        }
    }
}