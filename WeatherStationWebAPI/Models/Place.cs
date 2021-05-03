using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherStationWebAPI.Models
{
    public class Place
    {
        [Key]
        public int PlaceId { get; set; }

        public string PlaceName { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [ForeignKey("LogId")]
        public WeatherLog AttachedLog { get; set; }


    }
}
