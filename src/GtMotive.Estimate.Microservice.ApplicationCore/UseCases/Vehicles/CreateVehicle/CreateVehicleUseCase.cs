using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehicleUseCase : IUseCase<CreateVehicleInput>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IOutputPortStandard<CreateVehicleOutput> _standard;

        public CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        IOutputPortStandard<CreateVehicleOutput> standard)
        {
            _vehicleRepository = vehicleRepository;
            _standard = standard;
        }

        public async Task Execute(CreateVehicleInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            // Rule: manufacture date not older than 5 years
            if (input.ManufactureDate < DateTime.UtcNow.Date.AddYears(-5))
            {
                throw new DomainException("Vehicle manufacture date cannot be older than 5 years.");
            }

            var vehicle = new Domain.Models.Vehicle
            {
                VehicleId = Guid.NewGuid(),
                Plate = input.Plate,
                ManufactureDate = input.ManufactureDate,
                Status = Domain.Models.VehicleStatus.Available
            };

            await _vehicleRepository.Add(vehicle);

            _standard.StandardHandle(new CreateVehicleOutput
            {
                VehicleId = vehicle.VehicleId,
                Plate = vehicle.Plate,
                IsAvailable = vehicle.Status == Domain.Models.VehicleStatus.Available
            });
        }
    }
}
