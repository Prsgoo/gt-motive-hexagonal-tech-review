using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle
{
    public sealed class ReturnVehicleOutput : IUseCaseOutput
    {
        public Guid RentalId { get; init; }
        public Guid VehicleId { get; init; }
        public DateTime EndDate { get; init; }
    }
}
