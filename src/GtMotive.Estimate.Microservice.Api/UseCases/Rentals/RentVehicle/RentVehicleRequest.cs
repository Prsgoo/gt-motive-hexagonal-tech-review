using System;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rentals.RentVehicle
{
    public sealed class RentVehicleRequest
    {
        public Guid VehicleId { get; set; }
        public int PersonId { get; set; }
    }
}
