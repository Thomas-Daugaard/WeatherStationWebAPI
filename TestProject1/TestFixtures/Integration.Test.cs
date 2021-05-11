using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WeatherStationWebAPI.Models;
using Xunit;

namespace WeatherStationWebAPI.Test.XUnit.TestFixtures
{
    public class IntegrationTest
    {
        // Make sure to run project with Web API before running tests 

        [Fact]
        public void IntegrationTest_RegisterUser_UserCreated()
        {
            var user = new UserDto()
                { FirstName = "Kurt", LastName = "Poulsen", Email = "kp@somemail.com", Password = "Password1234" };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44301/api/");
                var postTask = client.PostAsJsonAsync<UserDto>("account/register", user);
                postTask.Wait();

                var result = postTask.Result;

                Assert.Equal("Created", result.StatusCode.ToString());
            }
        }
    }
}
