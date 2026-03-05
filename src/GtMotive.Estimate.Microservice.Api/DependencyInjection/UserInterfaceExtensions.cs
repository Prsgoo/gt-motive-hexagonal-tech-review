// API presenters
using GtMotive.Estimate.Microservice.Api.UseCases;
using GtMotive.Estimate.Microservice.Api.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Rentals.ReturnVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.Api.UseCases.Vehicles.ListAvailableVehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;

// ApplicationCore
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.ReturnVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.CreateVehicle;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles.ListAvailableVehicles;

// Domain ports
using GtMotive.Estimate.Microservice.Domain.Interfaces;

// Infrastructure adapters
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDb.Settings;
using GtMotive.Estimate.Microservice.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GtMotive.Estimate.Microservice.Api.DependencyInjection
{
    public static class UserInterfaceExtensions
    {
        public static IServiceCollection AddUserInterface(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));
            services.AddSingleton<MongoService>();

            // --------------------------
            // Infrastructure (Adapters)
            // --------------------------
            services.AddSingleton<IVehicleRepository, MongoVehicleRepository>();
            services.AddSingleton<IRentalRepository, MongoRentalRepository>();

            // --------------------------
            // Presenters + Output Ports
            // --------------------------

            // CreateVehicle
            services.AddScoped<CreateVehiclePresenter>();
            services.AddScoped<IWebApiPresenter>(sp => sp.GetRequiredService<CreateVehiclePresenter>());
            services.AddScoped<IOutputPortStandard<CreateVehicleOutput>>(sp => sp.GetRequiredService<CreateVehiclePresenter>());
            services.AddScoped<IOutputPortNotFound>(sp => sp.GetRequiredService<CreateVehiclePresenter>());

            // ListAvailableVehicles
            services.AddScoped<ListAvailableVehiclesPresenter>();
            services.AddScoped<IWebApiPresenter>(sp => sp.GetRequiredService<ListAvailableVehiclesPresenter>());
            services.AddScoped<IOutputPortStandard<ListAvailableVehiclesOutput>>(sp => sp.GetRequiredService<ListAvailableVehiclesPresenter>());
            services.AddScoped<IOutputPortNotFound>(sp => sp.GetRequiredService<ListAvailableVehiclesPresenter>());

            // RentVehicle
            services.AddScoped<RentVehiclePresenter>();
            services.AddScoped<IWebApiPresenter>(sp => sp.GetRequiredService<RentVehiclePresenter>());
            services.AddScoped<IOutputPortStandard<RentVehicleOutput>>(sp => sp.GetRequiredService<RentVehiclePresenter>());
            services.AddScoped<IOutputPortNotFound>(sp => sp.GetRequiredService<RentVehiclePresenter>());

            // ReturnVehicle
            services.AddScoped<ReturnVehiclePresenter>();
            services.AddScoped<IWebApiPresenter>(sp => sp.GetRequiredService<ReturnVehiclePresenter>());
            services.AddScoped<IOutputPortStandard<ReturnVehicleOutput>>(sp => sp.GetRequiredService<ReturnVehiclePresenter>());
            services.AddScoped<IOutputPortNotFound>(sp => sp.GetRequiredService<ReturnVehiclePresenter>());

            // --------------------------
            // Use Cases (Interactors)
            // --------------------------
            services.AddScoped<IUseCase<CreateVehicleInput>, CreateVehicleUseCase>();
            services.AddScoped<IUseCase<ListAvailableVehiclesInput>, ListAvailableVehiclesUseCase>();
            services.AddScoped<IUseCase<RentVehicleInput>, RentVehicleUseCase>();
            services.AddScoped<IUseCase<ReturnVehicleInput>, ReturnVehicleUseCase>();

            return services;
        }
    }
}
