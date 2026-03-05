using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle
{
    public sealed class RentVehicleUseCase : IUseCase<RentVehicleInput>
    {
        private readonly IVehicleRepository _vehicles;
        private readonly IRentalRepository _rentals;
        private readonly IOutputPortStandard<RentVehicleOutput> _standard;
        private readonly IOutputPortNotFound _notFound;

        public RentVehicleUseCase(
            IVehicleRepository vehicles,
            IRentalRepository rentals,
            IOutputPortStandard<RentVehicleOutput> standard,
            IOutputPortNotFound notFound)
        {
            _vehicles = vehicles;
            _rentals = rentals;
            _standard = standard;
            _notFound = notFound;
        }

        public async Task Execute(RentVehicleInput input)
        {
            var vehicle = await _vehicles.GetById(input.VehicleId);
            if (vehicle == null)
            {
                _notFound.NotFoundHandle($"Vehicle '{input.VehicleId}' not found.");
                return;
            }
            if (vehicle.Status != VehicleStatus.Available)
            {
                throw new DomainException("Vehicle is not available.");
            }

            var activeByPerson = await _rentals.GetActiveRentalByPerson(input.PersonId);
            if (activeByPerson != null)
            {

                throw new DomainException("Person already has an active rental.");
            }

            var rental = new Rental
            {
                RentalId = Guid.NewGuid(),
                VehicleId = vehicle.VehicleId,
                PersonId = input.PersonId,
                StartDate = DateTime.UtcNow,
                EndDate = null
            };

            await _rentals.Add(rental);

            vehicle.Status = VehicleStatus.Rented;
            await _vehicles.Update(vehicle);

            _standard.StandardHandle(new RentVehicleOutput
            {
                RentalId = rental.RentalId,
                VehicleId = rental.VehicleId,
                PersonId = rental.PersonId,
                StartDate = rental.StartDate
            });
        }
    }
}
