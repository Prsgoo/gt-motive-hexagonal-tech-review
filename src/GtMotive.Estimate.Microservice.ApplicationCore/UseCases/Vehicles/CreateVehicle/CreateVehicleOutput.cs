using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    public class CreateVehicleOutput : IUseCaseOutput
    {
        public Guid VehicleId { get; set; }

        public string Plate { get; init; } = string.Empty;

        public bool IsAvailable { get; init; }
    }
}
