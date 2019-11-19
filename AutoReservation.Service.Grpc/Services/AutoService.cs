using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class AutoService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<AutoService> _logger;

        public AutoService(ILogger<AutoService> logger)
        {
            _logger = logger;
        }
    }
}
