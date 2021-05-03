using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<WeatherLog>>> GetWeatherLogs()
        {
            return await _context.WeatherLogs.ToListAsync();
        }

        // GET: api/WeatherLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WeatherLog>> GetWeatherLog(int id)
        {
            var weatherLog = await _context.WeatherLogs.FindAsync(id);

            if (weatherLog == null)
            {
                return NotFound();
            }

            return weatherLog;
        }

        // PUT: api/WeatherLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
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
        [HttpPost]
        public async Task<ActionResult<WeatherLog>> PostWeatherLog(WeatherLog weatherLog)
        {
            _context.WeatherLogs.Add(weatherLog);
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
