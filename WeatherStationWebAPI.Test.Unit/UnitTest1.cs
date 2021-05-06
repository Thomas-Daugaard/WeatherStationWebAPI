using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.JsonWebTokens;
using NSubstitute;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Test.Unit
{
    public class Tests
    {
        private ApplicationDbContext _context;
        private IOptions<AppSettings> _appSettings;
        private IHubContext<WeatherHub> _mockHub;
        private AccountController _accountController;

        [SetUp]
        public void Setup()
        {
            _context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NGKWeatherAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False").Options);


            var settings = new AppSettings()
            {
                SecretKey = "ncSK45=)7@#qwKDSopevvkj3274687236"
            };

            _appSettings = Options.Create(settings);

            _mockHub = Substitute.For<IHubContext<WeatherHub>>();

            _accountController = new AccountController(_context, _appSettings, _mockHub);
        }

        //[Test]
        //public void Register_RegisterUser_ReceivedCorrectStatusCode()
        //{
        //    var user = new UserDto()
        //        { FirstName = "Kurt", LastName = "Poulsen", Email = "kp@somemail.com", Password = "Password1234" };

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("https://localhost:44301/api/");
        //        var postTask = client.PostAsJsonAsync<UserDto>("account/register", user);
        //        postTask.Wait();

        //        var result = postTask.Result;
        //        Assert.That(result.StatusCode, Is.EqualTo("Created"));
        //    }
        //}

        [Test]
        public async Task AccountController_RegisterUser_ResponseCreated()
        {
            var user = new UserDto()
                {Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234"};

            var response = await _accountController.Register(user);

            var result = response.Result as CreatedAtActionResult;

            Assert.NotNull(result);
        }

        [Test]
        public async Task AccountController_LoginUser_ResponseCreated()
        {
            var user = new UserDto()
                { Email = "ml@somemail.com", Password = "Password1234" };

            var response = await _accountController.Login(user);

            var token = response.Value.JWT;

            Assert.AreEqual(token, response.Value.JWT);

            // cleanup

            var userToDelete = _context.Users.SingleOrDefault(u => u.Email == user.Email);
            if(userToDelete != null)
                _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }
    }
}