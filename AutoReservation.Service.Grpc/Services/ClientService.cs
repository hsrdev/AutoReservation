using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class ClientService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<ClientService> _logger;

        public ClientService(ILogger<ClientService> logger)
        {
            _logger = logger;
        }
    }
}
