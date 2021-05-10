using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherStationWebAPI.Controllers;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Test.XUnit.TestFixtures;
using Xunit;

namespace WeatherStationWebAPI.Test.XUnit
{
    public class AccountControllerTest : IClassFixture<AccountControllerTestFixture>
    {
        private AccountController _accountController;

        public AccountControllerTest(AccountController accountController) : base()
        {
            _accountController = accountController;
        }

        [Fact]
        public async Task AccountController_RegisterUser_ResponseCreated()
        {
            var user = new UserDto()
                { Email = "ml@somemail.com", FirstName = "Morten", LastName = "Larsen", Password = "Password1234" };

            var response = await _accountController.Register(user);

            var result = response.Result as CreatedAtActionResult;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task AccountController_LoginUser_ResponseCreated()
        {
            var user = new UserDto()
                { Email = "ml@somemail.com", Password = "Password1234" };

            var response = await _accountController.Login(user);

            var token = response.Value.JWT;

            Assert.Equal(token, response.Value.JWT);

            // cleanup

            var userToDelete = _context.Users.SingleOrDefault(u => u.Email == user.Email);
            if (userToDelete != null)
                _context.Users.Remove(userToDelete);
            _context.SaveChanges();
        }
    }
}
