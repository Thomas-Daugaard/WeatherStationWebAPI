using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Test.XUnit.TestFixtures;
using Xunit;

namespace WeatherStationWebAPI.Test.XUnit
{
    public class AccountControllerTest : IClassFixture<AccountControllerTestFixture>
    {
        private ApplicationDbContext _context;
        private AccountController _accountController;
        private AccountControllerTestFixture _fixture;

        public AccountControllerTest(AccountControllerTestFixture fixture) : base()
        {
            _fixture = fixture;
            this._context = fixture.Context;
            this._accountController = fixture.AccountController;
        }

        [Fact]
        public async Task AccountController_RegisterUser_ReceivedResponseCreated()
        {
            // Arrange
            var user = new UserDto()
                { Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234" };

            // Act - post to /api/account/register
            var response = await _accountController.Register(user);

            // Assert - user registered / created
            Assert.NotNull(response.Result as CreatedAtActionResult);

            // Cleanup
            _fixture.Dispose();
        }

        [Fact]
        public async Task AccountController_Login_UserLoggedInJWTTokenReceived()
        {
            // Arrange
            var user = new UserDto()
                { Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234" };

            // Act - post to /api/account/register
            await _accountController.Register(user);
            
            // Act - post to /api/account/login
            var response = await _accountController.Login(user);

            // Assert - JWT token received
            Assert.Equal(typeof(TokenDto).FullName, response.Value.GetType().FullName);

            // Cleanup
            _fixture.Dispose();
        }
    }
}
