using AutoReservation.BusinessLayer;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Collections.Generic;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Service.Grpc.Services
{
    internal class CustomerService : Grpc.KundeService.KundeServiceBase
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly CustomerManager _customerManager = new CustomerManager();
        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }


        public override async Task<GetAllCustomersResponse> GetAllCustomers(Empty request, ServerCallContext context)
        {
            GetAllCustomersResponse response = new GetAllCustomersResponse();

            List<Customer> data = await _customerManager.GetAll();

            response.Data.AddRange(data.ConvertToDtos());

            return await Task.FromResult(response);
        }

        public override async Task<CustomerDto> GetCustomer(GetCustomerRequest request, ServerCallContext context)
        {
            CustomerDto response = new CustomerDto();
            try
            {
                Customer data = await _customerManager.Get(request.IdFilter);
                response = data.ConvertToDto();
            }
            catch (System.Exception e)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Customer key not found"));
            }

            return await Task.FromResult(response);
        }

        public override async Task<CustomerDto> InsertCustomer(CustomerDto request, ServerCallContext context)
        {
            Customer Customer = request.ConvertToEntity();

            Customer result = await _customerManager.Insert(Customer);

            return result.ConvertToDto();
        }

        public override async Task<Empty> UpdateCustomer(CustomerDto request, ServerCallContext context)
        {
            try
            {
                Customer Customer = request.ConvertToEntity();

                await _customerManager.Update(Customer);

            }
            catch (BusinessLayer.Exceptions.OptimisticConcurrencyException<Customer> e)
            {
                throw new RpcException(new Status(StatusCode.FailedPrecondition, "Customer update went wrong"));
            }
            Empty empt = new Empty();
            return empt;

        }

        public override async Task<Empty> DeleteCustomer(CustomerDto request, ServerCallContext context)
        {
            Customer Customer = request.ConvertToEntity();

            await _customerManager.Delete(Customer);

            Empty empt = new Empty();
            return empt;
        }

    }
}
