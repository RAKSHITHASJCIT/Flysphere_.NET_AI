using System.Threading.Tasks;

namespace FlysphereBackendDotnet.Services
{
    public interface ITicketService
    {
        Task<byte[]> GenerateTicketPdfAsync(string bookingId);
    }
}
