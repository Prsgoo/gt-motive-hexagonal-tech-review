using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    public sealed class MongoRentalRepository : IRentalRepository
    {
        private readonly IMongoCollection<Rental> _rentals;

        public MongoRentalRepository(MongoService mongo)
        {
            _rentals = mongo.Database.GetCollection<Rental>("rentals");
        }

        public async Task<Rental> Add(Rental rental)
        {
            await _rentals.InsertOneAsync(rental);
            return rental;
        }

        public async Task<Rental> GetActiveRentalByPerson(int personId)
        {
            // Assumes Rental has EndDate nullable and PersonId int
            return await _rentals
                .Find(r => r.PersonId == personId && r.EndDate == null)
                .FirstOrDefaultAsync();
        }

        public async Task<Rental> GetActiveRentalByVehicle(Guid vehicleId)
        {
            return await _rentals
                .Find(r => r.VehicleId == vehicleId && r.EndDate == null)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Rental rental)
        {
            await _rentals.ReplaceOneAsync(r => r.RentalId == rental.RentalId, rental);
        }
    }
}
