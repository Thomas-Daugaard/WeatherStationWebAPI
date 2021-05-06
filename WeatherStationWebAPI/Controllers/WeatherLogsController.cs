using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;
using WeatherStationWebAPI.WebSocket;

namespace WeatherStationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private IHubContext<WeatherHub> _weatherHub;

        public WeatherLogsController(ApplicationDbContext context, IHubContext<WeatherHub> hub)
        {
            _context = context;
            _weatherHub = hub;
        }

        // GET: api/WeatherLogs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WeatherLog>>> GetWeatherLogs()
        {
            return await _context.WeatherLogs.ToListAsync();
        }

        // GET: api/WeatherLogs/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<WeatherLog>> GetWeatherLog(int id)
        {
            var weatherLog = await _context.WeatherLogs.FindAsync(id);

            if (weatherLog == null)
            {
                return NotFound();
            }

            return weatherLog;
        }

        // GET: api/WeahterLogs/LastThree
        [HttpGet("Three")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WeatherLog>>> GetLastThreeWeatherLogs()
        {
            var lastThreeEntries = await _context.WeatherLogs.OrderByDescending(i => i.LogId).Take(3).Include(p=>p.LogPlace).ToListAsync();

            if (lastThreeEntries == null)
            {
                return NotFound();
            }

            return lastThreeEntries;
        }

        [HttpGet("LogDate")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WeatherLog>>> GetAllWeatherLogsForDate(DateTime date)
        {
            var allMeasurementsForDate = await _context.WeatherLogs.Where(d => d.LogTime.Date == date.Date).Include(p=>p.LogPlace).ToListAsync();

            if (allMeasurementsForDate == null)
            {
                return NotFound();
            }

            return allMeasurementsForDate;
        }

        [HttpGet("LogRange")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<WeatherLog>>> GetWeatherLogsForTimeframe(DateTime startTime, DateTime endTime)
        {
            var AllMeasurementsForTimeframe = await _context.WeatherLogs.Where(s => s.LogTime >= startTime).Where(e => e.LogTime <= endTime).Include(p=>p.LogPlace).ToListAsync();

            if (AllMeasurementsForTimeframe == null)
            {
                return NotFound();
            }

            return AllMeasurementsForTimeframe;
        }

        // PUT: api/WeatherLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutWeatherLog(int id, WeatherLog weatherLog)
        {
            if (id != weatherLog.LogId)
            {
                return BadRequest();
            }

            _context.Entry(weatherLog).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherLogExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/WeatherLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("PostWeather")]
        [Authorize]
        public async Task<ActionResult<WeatherLog>> PostWeatherLog(WeatherLogDto weatherLogDto)
        {
            var place = await _context.Places.FindAsync(weatherLogDto.LogPlaceId);
            if (place == null)
            {
                return NotFound();
            }
            WeatherLog weatherLog = new WeatherLog()
            {
                LogTime = weatherLogDto.LogTime,
                LogPlace = place,
                Temperature = weatherLogDto.Temperature,
                Humidity = weatherLogDto.Humidity,
                AirPressure = weatherLogDto.AirPressure
            };

            _context.WeatherLogs.Add(weatherLog);

            await _context.SaveChangesAsync();

            //==================  SignalR ===================
            var placeid = weatherLog.LogPlace.PlaceId;
            //var users = _context.Users.SelectMany(d => d.SignedUpPlaces).Where(l => l.PlaceId == placeid).ToList();

            var jsonmsg = JsonConvert.SerializeObject(weatherLog);
            await _weatherHub.Clients.Group(placeid.ToString()).SendAsync("Update", jsonmsg); //Send SignalR Message to all signed up users
            return CreatedAtAction("GetWeatherLog", new { id = weatherLog.LogId }, weatherLog);

        }

        // DELETE: api/WeatherLogs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeatherLog(int id)
        {
            var weatherLog = await _context.WeatherLogs.FindAsync(id);
            if (weatherLog == null)
            {
                return NotFound();
            }

            _context.WeatherLogs.Remove(weatherLog);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WeatherLogExists(int id)
        {
            return _context.WeatherLogs.Any(e => e.LogId == id);
        }
    }
}
