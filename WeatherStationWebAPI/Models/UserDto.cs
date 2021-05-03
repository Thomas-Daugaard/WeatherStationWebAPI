using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class UserDto
    {
        [MaxLength(64)]
        public string FirstName { get; set; }
        [MaxLength(32)]
        public string LastName { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        [MaxLength(72)]
        public string Password { get; set; }
    }
}
