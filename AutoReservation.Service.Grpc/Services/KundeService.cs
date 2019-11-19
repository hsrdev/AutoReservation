using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class KundeService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<KundeService> _logger;

        public KundeService(ILogger<KundeService> logger)
        {
            _logger = logger;
        }
    }
}
