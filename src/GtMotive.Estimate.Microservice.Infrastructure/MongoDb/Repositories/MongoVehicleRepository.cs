using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using MongoDB.Driver;

namespace GtMotive.Estimate.Microservice.Infrastructure.Repositories
{
    public sealed class MongoVehicleRepository : IVehicleRepository
    {
        private readonly IMongoCollection<Vehicle> _vehicles;

        public MongoVehicleRepository(MongoService mongo)
        {
            _vehicles = mongo.Database.GetCollection<Vehicle>("vehicles");
        }

        public async Task<Vehicle> Add(Vehicle vehicle)
        {
            await _vehicles.InsertOneAsync(vehicle);
            return vehicle;
        }

        public async Task<Vehicle> GetById(Guid id)
        {
            return await _vehicles.Find(v => v.VehicleId == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetAvailable()
        {
            // Assumes Vehicle has a Status or Status property.
            // Example with Status enum/string:
            return await _vehicles.Find(v => v.Status == VehicleStatus.Available).ToListAsync();
        }

        public async Task Update(Vehicle vehicle)
        {
            await _vehicles.ReplaceOneAsync(v => v.VehicleId == vehicle.VehicleId, vehicle);
        }
    }
}
