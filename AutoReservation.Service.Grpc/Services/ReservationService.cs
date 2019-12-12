using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using System.Collections.Generic;
using AutoReservation.Dal.Entities;


namespace AutoReservation.Service.Grpc.Services
{
    internal class ReservationService : Grpc.ReservationService.ReservationServiceBase
    {
        private readonly ILogger<ReservationService> _logger;
        private readonly ReservationManager _reservationManager = new ReservationManager();
        public ReservationService(ILogger<ReservationService> logger)
        {
            _logger = logger;
        }
        public override async Task<GetAllReservationsResponse> GetAllReservations(Empty request, ServerCallContext context)
        {
            GetAllReservationsResponse response = new GetAllReservationsResponse();

            List<Reservation> data = await _reservationManager.GetAll();

            response.Data.AddRange(data.ConvertToDtos());

            return await Task.FromResult(response);
        }

        public override async Task<ReservationDto> GetReservation(GetReservationRequest request, ServerCallContext context)
        {
            ReservationDto response = new ReservationDto();
            try
            {
                Reservation data = await _reservationManager.Get(request.IdFilter);
                
                response = data.ConvertToDto();
            }
            catch (System.Exception e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, e.Message));
            }

            return await Task.FromResult(response);
        }

        public override async Task<ReservationDto> InsertReservation(ReservationDto request, ServerCallContext context)
        {
            Reservation reservation = request.ConvertToEntity();
            try
            {
                Reservation result = await _reservationManager.Insert(reservation);

                return result.ConvertToDto();
            }
            catch (InvalidDateRangeException e)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            catch (CarUnavailableException ex)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, ex.Message));
            }

        }

        public override async Task<Empty> UpdateReservation(ReservationDto request, ServerCallContext context)
        {
            try
            {
                Reservation reservation = request.ConvertToEntity();

                await _reservationManager.Update(reservation);

            }
            catch (OptimisticConcurrencyException<Reservation> e)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, e.Message));
            }
            catch (CarUnavailableException e)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, e.Message));
            }
            catch (InvalidDateRangeException e)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, e.Message));
            }
            Empty empt = new Empty();
            return empt;

        }

        public override async Task<Empty> DeleteReservation(ReservationDto request, ServerCallContext context)
        {
            Reservation reservation = request.ConvertToEntity();

            await _reservationManager.Delete(reservation);

            Empty empt = new Empty();
            return empt;
        }

        public override async Task<CheckResponse> AvailabilityCheck(ReservationDto request, ServerCallContext context)
        {
            Reservation reservation = request.ConvertToEntity();
            CheckResponse response = new CheckResponse();

            response.IsValid = await _reservationManager.AvailabilityCheck(reservation);

            return response;
        }
    }



}
