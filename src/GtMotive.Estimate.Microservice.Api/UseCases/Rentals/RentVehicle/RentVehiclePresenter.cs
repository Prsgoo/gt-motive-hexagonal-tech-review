using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Rentals.RentVehicle
{
    public sealed class RentVehiclePresenter :
    IWebApiPresenter,
    IOutputPortStandard<RentVehicleOutput>,
    IOutputPortNotFound
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(RentVehicleOutput response)
        {
            if (response == null)
            {
                ActionResult = new StatusCodeResult(500);
                return;
            }

            ActionResult = new CreatedResult($"/rentals/{response}", response);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(new { error = message });
        }
    }
}
