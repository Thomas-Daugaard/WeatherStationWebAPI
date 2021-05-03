using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class WeatherLog
    {
        public int LogId { get; set; }
        public  DateTime LogTime { get; set; }

        public Place LogPlace { get; set; }

        public float Temperature { get; set; }
    }
}
