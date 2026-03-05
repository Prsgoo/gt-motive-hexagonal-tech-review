using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure.Fakes
{
    public sealed class InMemoryRentalRepository : IRentalRepository
    {
        private readonly Dictionary<Guid, Rental> _store = new();

        public Task<Rental> Add(Rental rental)
        {
            _store[rental.RentalId] = rental;
            return Task.FromResult(rental);
        }

        public Task<Rental> GetActiveRentalByPerson(int personId)
        {
            var r = _store.Values.FirstOrDefault(x => x.PersonId == personId && x.EndDate == null);
            return Task.FromResult(r);
        }

        public Task<Rental> GetActiveRentalByVehicle(Guid vehicleId)
        {
            var r = _store.Values.FirstOrDefault(x => x.VehicleId == vehicleId && x.EndDate == null);
            return Task.FromResult(r);
        }

        public Task Update(Rental rental)
        {
            _store[rental.RentalId] = rental;
            return Task.CompletedTask;
        }
    }
}
