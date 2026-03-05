using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Models;

namespace GtMotive.Estimate.Microservice.Domain.Interfaces
{
    public interface IRentalRepository
    {
        Task<Rental> Add(Rental rental);

        Task<Rental> GetActiveRentalByPerson(int personId);

        Task<Rental> GetActiveRentalByVehicle(Guid vehicleId);

        Task Update(Rental rental);
    }
}
