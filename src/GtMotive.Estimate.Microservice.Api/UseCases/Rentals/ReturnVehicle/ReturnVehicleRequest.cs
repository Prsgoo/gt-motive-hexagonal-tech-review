using System;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rentals.ReturnVehicle
{
    public sealed class ReturnVehicleRequest
    {
        public Guid VehicleId { get; set; }
    }
}
