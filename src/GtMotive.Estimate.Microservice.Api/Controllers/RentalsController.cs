using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Rentals.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    [ApiController]
    [Route("rentals")]
    public sealed class RentalsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Rent(
            [FromBody] RentVehicleRequest request,
            [FromServices] IUseCase<RentVehicleInput> useCase,
            [FromServices] RentVehiclePresenter presenter)
        {
            await useCase.Execute(new RentVehicleInput
            {
                VehicleId = request.VehicleId,
                PersonId = request.PersonId
            });

            return presenter.ActionResult;
        }

        [HttpPost("return")]
        public async Task<IActionResult> Return(
            [FromBody] ReturnVehicleRequest request,
            [FromServices] IUseCase<ReturnVehicleInput> useCase,
            [FromServices] ReturnVehiclePresenter presenter)
        {
            await useCase.Execute(new ReturnVehicleInput
            {
                VehicleId = request.VehicleId
            });

            return presenter.ActionResult;
        }
    }
}
