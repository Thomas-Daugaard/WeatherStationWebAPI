using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class WeatherLog
    {
        [Key]
        public int LogId { get; set; }
        public  DateTime LogTime { get; set; }

        public Place LogPlace { get; set; }

        public float Temperature { get; set; }

        public int Humidity { get; set; }

        public decimal AirPressure { get; set; }
    }
}
