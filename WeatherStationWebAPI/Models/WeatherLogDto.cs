using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class WeatherLogDto
    {
        public DateTime LogTime { get; set; }

        public long LogPlaceId { get; set; }

        public float Temperature { get; set; }

        public int Humidity { get; set; }

        public decimal AirPressure { get; set; }
    }
}
