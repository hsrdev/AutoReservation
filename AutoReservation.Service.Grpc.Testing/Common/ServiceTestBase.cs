using AutoReservation.TestEnvironment;
using Grpc.Net.Client;
using Xunit;

namespace AutoReservation.Service.Grpc.Testing.Common
{
    public abstract class ServiceTestBase
        : TestBase
        , IClassFixture<ServiceTestFixture>

    {
        private readonly ServiceTestFixture _serviceTestFixture;

        protected ServiceTestBase(ServiceTestFixture serviceTestFixture)
        {
            _serviceTestFixture = serviceTestFixture;
        }

        protected GrpcChannel Channel => _serviceTestFixture.Channel;
    }
}
