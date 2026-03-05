using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle
{
    public class RentVehicleInput : IUseCaseInput
    {
        public Guid VehicleId { get; init; }
        public int PersonId { get; init; }
    }
}
