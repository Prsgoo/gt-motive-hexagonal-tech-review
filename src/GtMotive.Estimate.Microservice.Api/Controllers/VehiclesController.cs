using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ListAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("vehicles")]
    public sealed class VehiclesController : ControllerBase
    {
        private readonly IUseCase<CreateVehicleInput> _useCase;
        private readonly CreateVehiclePresenter _presenter;

        public VehiclesController(
            IUseCase<CreateVehicleInput> useCase,
            CreateVehiclePresenter presenter)
        {
            _useCase = useCase;
            _presenter = presenter;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            await _useCase.Execute(new CreateVehicleInput
            {
                Plate = request.Plate,
                ManufactureDate = request.ManufactureDate
            });

            return _presenter.ActionResult;
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable([FromServices] IUseCase<ListAvailableVehiclesInput> useCase,
            [FromServices] ListAvailableVehiclesPresenter presenter)
        {
            await useCase.Execute(new ListAvailableVehiclesInput());
            return presenter.ActionResult;
        }
    }
}
