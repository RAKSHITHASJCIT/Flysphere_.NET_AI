using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Controllers
{
    [ApiController]
    [Route("api/flights")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [HttpPost]
        // Authorization disabled temporarily
        public async Task<ActionResult<Flight>> CreateFlight([FromBody] Flight flight)
        {
            var created = await _flightService.CreateFlightAsync(flight);
            return Ok(created);
        }

        [HttpGet]
        public async Task<ActionResult> GetFlights(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] string date)
        {
            if (!string.IsNullOrEmpty(from) &&
                !string.IsNullOrEmpty(to) &&
                !string.IsNullOrEmpty(date))
            {
                var parsedDate = DateTime.Parse(date);
                var result = await _flightService.SearchFlightsAsync(from, to, parsedDate);
                return Ok(result);
            }

            var flights = await _flightService.GetAllFlightsAsync();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlightById(long id)
        {
            var flight = await _flightService.GetFlightByIdAsync(id);
            return Ok(flight);
        }

        [HttpPut("{id}")]
        // Authorization disabled temporarily
        public async Task<ActionResult<Flight>> UpdateFlight(long id, [FromBody] Flight flight)
        {
            var updated = await _flightService.UpdateFlightAsync(id, flight);
            return Ok(updated);
        }

        [HttpPatch("{id}/cancel")]
        // Authorization disabled temporarily
        public async Task<ActionResult<Flight>> CancelFlight(long id)
        {
            var cancelled = await _flightService.CancelFlightAsync(id);
            return Ok(cancelled);
        }

        [HttpDelete("{id}")]
        // Authorization disabled temporarily
        public async Task<IActionResult> DeleteFlight(long id)
        {
            await _flightService.DeleteFlightAsync(id);
            return NoContent();
        }
    }
}
