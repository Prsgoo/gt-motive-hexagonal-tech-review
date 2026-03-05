using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ListAvailableVehicles
{
    public sealed class ListAvailableVehiclesPresenter :
        IWebApiPresenter,
        IOutputPortStandard<ListAvailableVehiclesOutput>,
        IOutputPortNotFound
    {
        public IActionResult ActionResult { get; private set; } = new StatusCodeResult(500);

        public void StandardHandle(ListAvailableVehiclesOutput response)
        {
            ActionResult = new OkObjectResult(response);
        }

        public void NotFoundHandle(string message)
        {
            ActionResult = new NotFoundObjectResult(new { error = message });
        }
    }
}
