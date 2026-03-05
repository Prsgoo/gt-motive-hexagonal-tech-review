using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.Domain.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles
{
    public sealed class ListAvailableVehiclesUseCase : IUseCase<ListAvailableVehiclesInput>
    {
        private readonly IVehicleRepository _vehicles;
        private readonly IOutputPortStandard<ListAvailableVehiclesOutput> _standard;

        public ListAvailableVehiclesUseCase(
            IVehicleRepository vehicles,
            IOutputPortStandard<ListAvailableVehiclesOutput> standard)
        {
            _vehicles = vehicles;
            _standard = standard;
        }

        public async Task Execute(ListAvailableVehiclesInput input)
        {
            var available = await _vehicles.GetAvailable();

            _standard.StandardHandle(new ListAvailableVehiclesOutput
            {
                Vehicles = available.Select(v => new ListAvailableVehiclesOutput.VehicleItem
                {
                    Id = v.VehicleId,
                    Plate = v.Plate,
                    ManufactureDate = v.ManufactureDate,
                    IsAvailable = v.Status == Domain.Models.VehicleStatus.Available
                })
            });
        }
    }
}
