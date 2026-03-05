using System;

namespace GtMotive.Estimate.Microservice.Domain.Models
{
    public class Vehicle
    {
        public Guid VehicleId { get; set; }

        public string Plate { get; set; } = string.Empty;

        public DateTime ManufactureDate { get; set; }

        public VehicleStatus Status { get; set; }
    }
}
