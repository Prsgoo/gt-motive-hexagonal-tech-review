using System;
using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles
{
    public class ListAvailableVehiclesOutput : IUseCaseOutput
    {
        public IEnumerable<VehicleItem> Vehicles { get; init; } = [];
        public sealed class VehicleItem
        {
            public Guid Id { get; init; }
            public string Plate { get; init; } = string.Empty;
            public DateTime ManufactureDate { get; init; }
            public bool IsAvailable { get; init; }
        }
    }
}
