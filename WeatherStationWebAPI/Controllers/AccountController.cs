using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.Data;
using static BCrypt.Net.BCrypt;


namespace WeatherStationWebAPI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int BcryptWorkfactor = 10;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserDto regUser)
        {
            regUser.Email = regUser.Email.ToLower();
            var emailExist = await _context.Users.Where(u => u.Email == regUser.Email).FirstOrDefaultAsync();
            if (emailExist != null)
                return BadRequest(new { errorMessage = "Email already in use" });
            User user = new User()
            {
                Email = regUser.Email,
                FirstName = regUser.FirstName,
                LastName = regUser.LastName
            };
            user.PwHash = HashPassword(regUser.Password, BcryptWorkfactor);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = user.UserId }, regUser);
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<ActionResult<UserDto>> Login(UserDto login)
        {
            login.Email = login.Email.ToLower();
            var user = await _context.Users.Where(u =>
                u.Email == login.Email).FirstOrDefaultAsync();
            if (user != null)
            {
                var validPwd = Verify(login.Password, user.PwHash);
                if (validPwd)
                {
                    return login;
                }
            }
            ModelState.AddModelError(string.Empty, "Forkert brugernavn eller password");
            return BadRequest(ModelState);
        }

        // GET: api/Account/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ActionResult<UserDto>> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = new UserDto();
            userDto.Email = user.Email;
            userDto.FirstName = user.FirstName;
            userDto.LastName = user.LastName;
            return userDto;
        }
    }
}
