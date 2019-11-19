using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;

        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
        }
    }
}
