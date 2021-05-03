using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class WeatherInfo
    {
        [Key]
        public int MeasurementID { get; set; }
        public DateTime MeasurementTime { get; set; }
        [ForeignKey("PlaceId")]
        public Place MeasurementPlace { get; set; }
        public float MeasurementTemperature { get; set; }
        public int MeasurementHumidity { get; set; }
        public decimal MeasurementAirPressure { get; set; }
    }
}
