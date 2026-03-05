using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle
{
    public sealed class ReturnVehicleInput : IUseCaseInput
    {
        public Guid VehicleId { get; init; }
    }
}
