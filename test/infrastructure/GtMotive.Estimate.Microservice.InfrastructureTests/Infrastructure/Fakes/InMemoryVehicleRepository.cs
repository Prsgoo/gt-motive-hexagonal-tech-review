using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure.Fakes
{
    public sealed class InMemoryVehicleRepository : IVehicleRepository
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
            var result = _store.Values.Where(v => v.Status == VehicleStatus.Available).AsEnumerable();
            return Task.FromResult(result);
        }

        public Task Update(Vehicle vehicle)
        {
            _store[vehicle.VehicleId] = vehicle;
            return Task.CompletedTask;
        }
    }
}
