using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle
{
    public sealed class CreateVehiclePresenter :
    IWebApiPresenter,
    IOutputPortStandard<CreateVehicleOutput>,
    IOutputPortNotFound
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(CreateVehicleOutput response)
        {
            if (response == null)
            {
                ActionResult = new StatusCodeResult(500);
                return;
            }

            ActionResult = new CreatedResult($"/vehicles/{response.VehicleId}", response);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(new { error = message });
        }
    }
}
