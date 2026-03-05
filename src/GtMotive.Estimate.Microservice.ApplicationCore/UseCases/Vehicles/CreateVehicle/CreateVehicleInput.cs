using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    public class CreateVehicleInput : IUseCaseInput
    {
        public DateTime ManufactureDate { get; init; }

        public string Plate { get; init; } = string.Empty;
    }
}
