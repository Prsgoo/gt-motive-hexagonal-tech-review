using GtMotive.Estimate.Microservice.Domain.Models;
using MongoDB.Bson.Serialization;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDb
{
    public static class MongoMappings
    {
        public static void Register()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Vehicle)))
            {
                BsonClassMap.RegisterClassMap<Vehicle>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(v => v.VehicleId);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Rental)))
            {
                BsonClassMap.RegisterClassMap<Rental>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdProperty(r => r.RentalId);
                });
            }
        }
    }
}
