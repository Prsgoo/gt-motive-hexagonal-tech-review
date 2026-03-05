using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> Add(Vehicle vehicle);

        Task<Vehicle> GetById(Guid id);

        Task<IEnumerable<Vehicle>> GetAvailable();

        Task Update(Vehicle vehicle);
    }
}
