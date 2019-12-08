using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using AutoReservation.BusinessLayer;
using System.Collections.Generic;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Service.Grpc.Services
{
    internal class CarService : Grpc.AutoService.AutoServiceBase
    {
        private readonly ILogger<CarService> _logger;
        private readonly CarManager _carManager = new CarManager();

        public CarService(ILogger<CarService> logger)
        {
            _logger = logger;
        }

        public override async Task<GetAllCarsResponse> GetAllCars(Empty request, ServerCallContext context)
        {
            GetAllCarsResponse response = new GetAllCarsResponse();

            List<Car> data = await _carManager.GetAll();

            response.Data.AddRange(data.ConvertToDtos());
            
            return await Task.FromResult(response);
        }

        public override async Task<CarDto> GetCar(GetCarRequest request, ServerCallContext context)
        {
            CarDto response = new CarDto();
            try
            {
                Car data = await _carManager.Get(request.IdFilter);
                response = data.ConvertToDto();
            }
            catch (System.Exception e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Car key not found"));
            }

            return await Task.FromResult(response);
        }

        public override async Task<CarDto> InsertCar(CarDto request, ServerCallContext context)
        {
            Car car = request.ConvertToEntity();
            
            Car result = await _carManager.Insert(car);

            return result.ConvertToDto();
        }

        public override async Task<Empty> UpdateCar(CarDto request, ServerCallContext context)
        {
            try
            {
                Car car = request.ConvertToEntity();

                await _carManager.Update(car);

            }
            catch (BusinessLayer.Exceptions.OptimisticConcurrencyException<Car> e)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Car update went wrong"));
            }
            Empty empt = new Empty();
            return empt;

        }

        public override async Task<Empty> DeleteCar(CarDto request, ServerCallContext context)
        {
            Car car = request.ConvertToEntity();

            await _carManager.Delete(car);

            Empty empt = new Empty();
            return empt;
        }


    }
}
