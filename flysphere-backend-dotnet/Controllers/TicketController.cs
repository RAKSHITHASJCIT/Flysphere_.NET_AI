using FlysphereBackendDotnet.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{bookingId}/pdf")]
        public async Task<IActionResult> DownloadTicket(string bookingId)
        {
            var pdfBytes = await _ticketService.GenerateTicketPdfAsync(bookingId);

            return File(
                pdfBytes,
                "application/pdf",
                bookingId + "_Eticket.pdf"
            );
        }
    }
}
