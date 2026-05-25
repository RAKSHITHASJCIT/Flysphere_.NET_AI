using FlysphereBackendDotnet.Models;
using FlysphereBackendDotnet.Repositories;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public class TicketService : ITicketService
    {
        private readonly IBookingRepository _bookingRepository;

        public TicketService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<byte[]> GenerateTicketPdfAsync(string bookingId)
        {
            var booking = await _bookingRepository.GetByBookingIdAsync(bookingId);
            if (booking == null)
                throw new System.Exception("Booking not found");

            using var ms = new MemoryStream();
            var writer = new PdfWriter(ms);
            var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var bold = iText.Kernel.Font.PdfFontFactory
                .CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);

            var brandColor = new DeviceRgb(16, 55, 92);   // Deep airline blue
            var lightGray = new DeviceRgb(245, 247, 250);

            // ================= HEADER =================
            var headerTable = new Table(1).UseAllAvailableWidth();
            headerTable.AddCell(new Cell()
                .Add(new Paragraph("✈  FlySphere")
                    .SetFont(bold)
                    .SetFontSize(24)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(brandColor)
                .SetPadding(15)
                .SetBorder(Border.NO_BORDER));

            document.Add(headerTable);
            document.Add(new Paragraph(" "));

            // ================= BOOKING BOX =================
            var bookingBox = new Table(2).UseAllAvailableWidth();
            bookingBox.SetBackgroundColor(lightGray);

            bookingBox.AddCell(new Cell().Add(new Paragraph("BOOKING REF").SetFont(bold)).SetBorder(Border.NO_BORDER));
            bookingBox.AddCell(new Cell().Add(new Paragraph(booking.BookingId).SetFont(bold)).SetBorder(Border.NO_BORDER));

            bookingBox.AddCell(new Cell().Add(new Paragraph("Status")).SetBorder(Border.NO_BORDER));
            bookingBox.AddCell(new Cell().Add(new Paragraph(booking.Status)).SetBorder(Border.NO_BORDER));

            bookingBox.AddCell(new Cell().Add(new Paragraph("Total Paid")).SetBorder(Border.NO_BORDER));
            bookingBox.AddCell(new Cell().Add(new Paragraph("₹ " + booking.TotalAmount)).SetBorder(Border.NO_BORDER));

            document.Add(bookingBox);
            document.Add(new Paragraph(" "));

            // ================= DEPARTURE =================
            if (booking.Segments != null && booking.Segments.Any())
            {
                foreach (var segment in booking.Segments)
                {
                    var flight = segment.Flight;
                    if (flight == null) continue;

                    document.Add(new Paragraph(segment.SegmentNo == 1 ? "DEPARTURE FLIGHT" : "RETURN FLIGHT")
                        .SetFont(bold)
                        .SetFontSize(14)
                        .SetFontColor(brandColor)
                        .SetMarginTop(15));

                    var flightTable = new Table(2).UseAllAvailableWidth();

                    flightTable.AddCell(new Cell().Add(new Paragraph("Airline").SetFont(bold)));
                    flightTable.AddCell(new Cell().Add(new Paragraph(flight.AirlineName)));

                    flightTable.AddCell(new Cell().Add(new Paragraph("Flight No").SetFont(bold)));
                    flightTable.AddCell(new Cell().Add(new Paragraph(flight.FlightNo)));

                    flightTable.AddCell(new Cell().Add(new Paragraph("Route").SetFont(bold)));
                    flightTable.AddCell(new Cell().Add(new Paragraph(
                        flight.DepartureAirport + "  →  " + flight.ArrivalAirport)));

                    flightTable.AddCell(new Cell().Add(new Paragraph("Departure").SetFont(bold)));
                    flightTable.AddCell(new Cell().Add(new Paragraph(
                        flight.DepartureDate?.ToString("yyyy-MM-dd") + "  " + flight.DepartureTime)));

                    flightTable.AddCell(new Cell().Add(new Paragraph("Arrival").SetFont(bold)));
                    flightTable.AddCell(new Cell().Add(new Paragraph(
                        flight.ArrivalDate?.ToString("yyyy-MM-dd") + "  " + flight.ArrivalTime)));

                    document.Add(flightTable);
                }
            }

            document.Add(new Paragraph(" "));

            // ================= PASSENGERS =================
            document.Add(new Paragraph("PASSENGER DETAILS")
                .SetFont(bold)
                .SetFontSize(14)
                .SetFontColor(brandColor)
                .SetMarginTop(15));

            if (booking.Passengers != null && booking.Passengers.Any())
            {
                var table = new Table(6).UseAllAvailableWidth();

                string[] headers = {
                    "Name", "Type", "Outbound Seat",
                    "Outbound Meal", "Return Seat", "Return Meal"
                };

                foreach (var h in headers)
                {
                    table.AddHeaderCell(new Cell()
                        .Add(new Paragraph(h).SetFont(bold).SetFontColor(ColorConstants.WHITE))
                        .SetBackgroundColor(brandColor));
                }

                foreach (var p in booking.Passengers)
                {
                    table.AddCell(p.FirstName + " " + p.LastName);
                    table.AddCell(p.Type);
                    table.AddCell(p.OutboundSeat ?? "N/A");
                    table.AddCell(p.OutboundMeal ?? "N/A");
                    table.AddCell(p.ReturnSeat ?? "N/A");
                    table.AddCell(p.ReturnMeal ?? "N/A");
                }

                document.Add(table);
            }

            document.Add(new Paragraph(" "));

            // ================= CONTACT =================
            document.Add(new Paragraph("Contact Information")
                .SetFont(bold)
                .SetFontColor(brandColor)
                .SetMarginTop(10));

            document.Add(new Paragraph("Email: " + booking.ContactEmail));
            document.Add(new Paragraph("Phone: " + booking.ContactPhone));

            document.Add(new Paragraph(" "));
            document.Add(new Paragraph("Thank you for choosing FlySphere  |  Have a pleasant journey")
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontColor(brandColor)
                .SetMarginTop(25));

            document.Close();
            return ms.ToArray();
        }
    }
}
