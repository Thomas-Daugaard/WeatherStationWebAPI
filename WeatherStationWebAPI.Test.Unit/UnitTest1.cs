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
using NUnit.Framework;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;

namespace WeatherStationWebAPI.Test.Unit
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
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
    }
}