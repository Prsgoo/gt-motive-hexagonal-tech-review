using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rentals.ReturnVehicle
{
    public sealed class ReturnVehiclePresenter :
        IWebApiPresenter,
        IOutputPortStandard<ReturnVehicleOutput>,
        IOutputPortNotFound
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(ReturnVehicleOutput response)
        {
            ActionResult = new OkObjectResult(response);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(new { error = message });
        }
    }
}
