using Microsoft.Extensions.Logging;

namespace AutoReservation.Service.Grpc.Services
{
    internal class CarService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<CarService> _logger;

        public CarService(ILogger<CarService> logger)
        {
            _logger = logger;
        }
    }
}
