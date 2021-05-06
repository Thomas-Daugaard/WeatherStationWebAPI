using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherStationWebAPI.Data;
using WeatherStationWebAPI.Models;

namespace WeatherStationWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WeatherLogsController(ApplicationDbContext context)
        {
            _context = context;
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

            //Send SignalR Message to all signed up users

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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<WeatherLog>> PostWeatherLog(WeatherLog weatherLog)
        {
            _context.WeatherLogs.Add(weatherLog);
            //Send SignalR Message to all signed up users
            await _context.SaveChangesAsync();

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
