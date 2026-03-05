using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle
{
    public class RentVehicleOutput : IUseCaseOutput
    {
        public Guid RentalId { get; init; }
        public Guid VehicleId { get; init; }
        public int PersonId { get; init; }
        public DateTime StartDate { get; init; }
    }
}
