using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    public class RentalFlowFunctionalTests
    {
        [Fact]
        public async Task RentThenReturnUpdatesAvailability()
        {
            // Arrange: in-memory repos
            var vehicleRepo = new InMemoryVehicleRepository();
            var rentalRepo = new InMemoryRentalRepository();

            // Output ports capture
            var createPresenter = new CaptureStandard<CreateVehicleOutput>();
            var listPresenter = new CaptureStandard<ListAvailableVehiclesOutput>();
            var rentPresenter = new CaptureStandard<RentVehicleOutput>();
            var returnPresenter = new CaptureStandard<ReturnVehicleOutput>();
            var notFound = new CaptureNotFound();

            // Use cases
            var create = new CreateVehicleUseCase(vehicleRepo, createPresenter);
            var list = new ListAvailableVehiclesUseCase(vehicleRepo, listPresenter);
            var rent = new RentVehicleUseCase(vehicleRepo, rentalRepo, rentPresenter, notFound);
            var ret = new ReturnVehicleUseCase(vehicleRepo, rentalRepo, returnPresenter, notFound);

            // 1) Create a vehicle
            await create.Execute(new CreateVehicleInput
            {
                Plate = "FUNC-001",
                ManufactureDate = DateTime.UtcNow.AddYears(-1)
            });

            var vehicleId = createPresenter.Last!.VehicleId;

            // 2) Confirm available contains it
            await list.Execute(new ListAvailableVehiclesInput());
            Assert.Contains(listPresenter.Last!.Vehicles, v => v.Id == vehicleId);

            // 3) Rent it
            await rent.Execute(new RentVehicleInput { VehicleId = vehicleId, PersonId = 99 });
            Assert.NotNull(rentPresenter.Last);

            // 4) Now it should NOT be available
            await list.Execute(new ListAvailableVehiclesInput());
            Assert.DoesNotContain(listPresenter.Last!.Vehicles, v => v.Id == vehicleId);

            // 5) Return it
            await ret.Execute(new ReturnVehicleInput { VehicleId = vehicleId });
            Assert.NotNull(returnPresenter.Last);

            // 6) Now it should be available again
            await list.Execute(new ListAvailableVehiclesInput());
            Assert.Contains(listPresenter.Last!.Vehicles, v => v.Id == vehicleId);
        }

        // --------- Test doubles (kept inside test file for simplicity) ---------
        private sealed class CaptureStandard<T> : IOutputPortStandard<T>
            where T : IUseCaseOutput
        {
            public T Last { get; private set; }

            public void StandardHandle(T response) => Last = response;
        }

        private sealed class CaptureNotFound : IOutputPortNotFound
        {
            public string LastMessage { get; private set; }

            public void NotFoundHandle(string message) => LastMessage = message;
        }

        private sealed class InMemoryVehicleRepository : IVehicleRepository
        {
            private readonly Dictionary<Guid, Vehicle> _store = new();

            public Task<Vehicle> Add(Vehicle vehicle)
            {
                _store[vehicle.VehicleId] = vehicle;
                return Task.FromResult(vehicle);
            }

            public Task<Vehicle> GetById(Guid id)
            {
                _store.TryGetValue(id, out var v);
                return Task.FromResult(v);
            }

            public Task<IEnumerable<Vehicle>> GetAvailable()
            {
                var result = _store.Values.Where(v => v.Status == VehicleStatus.Available).ToList();
                return Task.FromResult<IEnumerable<Vehicle>>(result);
            }

            public Task Update(Vehicle vehicle)
            {
                _store[vehicle.VehicleId] = vehicle;
                return Task.CompletedTask;
            }
        }

        private sealed class InMemoryRentalRepository : IRentalRepository
        {
            private readonly Dictionary<Guid, Rental> _store = new();

            public Task<Rental> Add(Rental rental)
            {
                _store[rental.RentalId] = rental;
                return Task.FromResult(rental);
            }

            public Task<Rental> GetActiveRentalByPerson(int personId)
            {
                var rental = _store.Values.FirstOrDefault(r => r.PersonId == personId && r.EndDate == null);
                return Task.FromResult(rental);
            }

            public Task<Rental> GetActiveRentalByVehicle(Guid vehicleId)
            {
                var rental = _store.Values.FirstOrDefault(r => r.VehicleId == vehicleId && r.EndDate == null);
                return Task.FromResult(rental);
            }

            public Task Update(Rental rental)
            {
                _store[rental.RentalId] = rental;
                return Task.CompletedTask;
            }
        }
    }
}
