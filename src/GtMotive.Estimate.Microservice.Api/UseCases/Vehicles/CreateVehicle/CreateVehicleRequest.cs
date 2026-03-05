using System;
using System.ComponentModel.DataAnnotations;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehicleRequest
    {
        [Required]
        [MinLength(1)]
        public string Plate { get; set; } = string.Empty;

        [Required]
        public DateTime ManufactureDate { get; set; }
    }
}
