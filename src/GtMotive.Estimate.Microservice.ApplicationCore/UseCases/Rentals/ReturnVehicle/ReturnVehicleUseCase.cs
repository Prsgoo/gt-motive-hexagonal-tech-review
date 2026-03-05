using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle
{
    public sealed class ReturnVehicleUseCase : IUseCase<ReturnVehicleInput>
    {
        private readonly IVehicleRepository _vehicles;
        private readonly IRentalRepository _rentals;
        private readonly IOutputPortStandard<ReturnVehicleOutput> _standard;
        private readonly IOutputPortNotFound _notFound;

        public ReturnVehicleUseCase(
            IVehicleRepository vehicles,
            IRentalRepository rentals,
            IOutputPortStandard<ReturnVehicleOutput> standard,
            IOutputPortNotFound notFound)
        {
            _vehicles = vehicles;
            _rentals = rentals;
            _standard = standard;
            _notFound = notFound;
        }

        public async Task Execute(ReturnVehicleInput input)
        {
            var vehicle = await _vehicles.GetById(input.VehicleId);
            if (vehicle == null)
            {
                _notFound.NotFoundHandle($"Vehicle '{input.VehicleId}' not found.");
                return;
            }

            var activeRental = await _rentals.GetActiveRentalByVehicle(input.VehicleId);
            if (activeRental == null)
            {
                _notFound.NotFoundHandle($"No active rental found for vehicle '{input.VehicleId}'.");
                return;
            }

            activeRental.EndDate = DateTime.UtcNow;
            await _rentals.Update(activeRental);

            vehicle.Status = VehicleStatus.Available; // available again
            await _vehicles.Update(vehicle);

            _standard.StandardHandle(new ReturnVehicleOutput
            {
                RentalId = activeRental.RentalId,
                VehicleId = activeRental.VehicleId,
                EndDate = activeRental.EndDate.Value
            });
        }
    }
}
