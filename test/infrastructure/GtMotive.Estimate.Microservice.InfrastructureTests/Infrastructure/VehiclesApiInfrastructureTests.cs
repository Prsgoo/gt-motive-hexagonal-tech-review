using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure
{
    public sealed class VehiclesApiInfrastructureTests : InfrastructureTestBase
    {
        private readonly HttpClient _client;

        public VehiclesApiInfrastructureTests(GenericInfrastructureTestServerFixture fixture)
            : base(fixture)
        {
#pragma warning disable CA1062 // Validate arguments of public methods
            _client = fixture.Server.CreateClient();
#pragma warning restore CA1062 // Validate arguments of public methods
        }

        [Fact]
        public async Task GetVehiclesWhenRouteNotDefinedReturns404Or405()
        {
            var response = await _client.GetAsync(new Uri("/vehicles", UriKind.Relative));
            Assert.True(response.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public async Task CreateVehicleWhenMissingRequiredFieldsReturns400()
        {
            // Missing "plate" and "manufactureDate"
            var invalidJson = /*lang=json,strict*/ "{\"plate\":\"\",\"manufactureDate\":\"2023-01-01T00:00:00Z\"}";
            using var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(new Uri("/vehicles", UriKind.Relative), content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateVehicleWhenInvalidJsonReturns400()
        {
            // Invalid JSON (model binding fails)
            var invalidJson = "{";
            using var content = new StringContent(invalidJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(new Uri("/vehicles", UriKind.Relative), content);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
