using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Abstractions;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals.RentVehicle;
using GtMotive.Estimate.Microservice.Domain;
using GtMotive.Estimate.Microservice.Domain.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Models;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore
{
    /**
     * <summary>
     * Contains unit tests for the <see cref="RentVehicleUseCase"/> class, verifying rental logic and error handling.
     * </summary>
     */
    public class RentVehicleUseCaseTests
    {
        /**
         * <summary>
         * Verifies that a <see cref="DomainException"/> is thrown when a person already has an active rental.
         * </summary>
         * <returns>A task representing the asynchronous test operation.</returns>
         */
        [Fact]
        public async Task ThrowsWhenPersonAlreadyHasActiveRental()
        {
            var vehicleId = Guid.NewGuid();

            var vehicles = new Mock<IVehicleRepository>();
            vehicles.Setup(v => v.GetById(vehicleId))
                .ReturnsAsync(new Vehicle { VehicleId = vehicleId, Status = VehicleStatus.Available });

            var rentals = new Mock<IRentalRepository>();
            rentals.Setup(r => r.GetActiveRentalByPerson(1))
                .ReturnsAsync(new Rental { RentalId = Guid.NewGuid(), PersonId = 1, VehicleId = Guid.NewGuid(), EndDate = null });

            var standard = new Mock<IOutputPortStandard<RentVehicleOutput>>();
            var notFound = new Mock<IOutputPortNotFound>();

            var useCase = new RentVehicleUseCase(vehicles.Object, rentals.Object, standard.Object, notFound.Object);

            await Assert.ThrowsAsync<DomainException>(() =>
                useCase.Execute(new RentVehicleInput { VehicleId = vehicleId, PersonId = 1 }));
        }

        /**
         * <summary>
         * Verifies that a <see cref="DomainException"/> is thrown when attempting to rent a vehicle that is already rented.
         * </summary>
         * <returns>A task representing the asynchronous test operation.</returns>
         */
        [Fact]
        public async Task ThrowsWhenVehicleRented()
        {
            var vehicleId = Guid.NewGuid();

            var vehicles = new Mock<IVehicleRepository>();
            vehicles.Setup(v => v.GetById(vehicleId))
                .ReturnsAsync(new Vehicle { VehicleId = vehicleId, Status = VehicleStatus.Rented });

            var rentals = new Mock<IRentalRepository>();
            rentals.Setup(r => r.GetActiveRentalByPerson(1))
                .ReturnsAsync((Rental)null);

            var standard = new Mock<IOutputPortStandard<RentVehicleOutput>>();
            var notFound = new Mock<IOutputPortNotFound>();

            var useCase = new RentVehicleUseCase(vehicles.Object, rentals.Object, standard.Object, notFound.Object);

            await Assert.ThrowsAsync<DomainException>(() =>
                useCase.Execute(new RentVehicleInput { VehicleId = vehicleId, PersonId = 1 }));
        }

        /**
         * <summary>
         * Verifies that a vehicle is rented successfully when the person has no active rental and the vehicle is available.
         * </summary>
         * <returns>A task representing the asynchronous test operation.</returns>
         */
        [Fact]
        public async Task RentsVehicleSuccessfully()
        {
            var vehicleId = Guid.NewGuid();

            var vehicles = new Mock<IVehicleRepository>();
            vehicles.Setup(v => v.GetById(vehicleId))
                .ReturnsAsync(new Vehicle { VehicleId = vehicleId, Status = VehicleStatus.Available });

            var rentals = new Mock<IRentalRepository>();
            rentals.Setup(r => r.GetActiveRentalByPerson(1))
                .ReturnsAsync((Rental)null);

            var standard = new Mock<IOutputPortStandard<RentVehicleOutput>>();
            var notFound = new Mock<IOutputPortNotFound>();

            var useCase = new RentVehicleUseCase(vehicles.Object, rentals.Object, standard.Object, notFound.Object);

            await useCase.Execute(new RentVehicleInput
            {
                VehicleId = vehicleId,
                PersonId = 1
            });

            rentals.Verify(r => r.Add(It.IsAny<Rental>()), Times.Once);
            vehicles.Verify(v => v.Update(It.IsAny<Vehicle>()), Times.Once);
            standard.Verify(o => o.StandardHandle(It.IsAny<RentVehicleOutput>()), Times.Once);
        }
    }
}
