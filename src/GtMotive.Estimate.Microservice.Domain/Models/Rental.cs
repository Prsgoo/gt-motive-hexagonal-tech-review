using System;

namespace GtMotive.Estimate.Microservice.Domain.Models
{
    public class Rental
    {
        /**
         * <summary>
         * Gets or sets the unique identifier for the rental.
         * </summary>
         */
        public Guid RentalId { get; set; }

        /**
         * <summary>
         * Gets or sets the unique identifier for the vehicle being rented.
         * </summary>
         */
        public Guid VehicleId { get; set; }

        /**
         * <summary>
         * Gets or sets the unique identifier for the customer renting the vehicle.
         * </summary>
         */
        public int PersonId { get; set; }

        /**
         * <summary>
         * Gets or sets the start date of the rental period.
         * </summary>
         */
        public DateTime StartDate { get; set; }

        /**
         * <summary>
         * Gets or sets the end date of the rental period.
         * </summary>
         */
        public DateTime? EndDate { get; set; }

        /**
         * <summary>
         * Determines whether the rental is currently active based on the current date and time.
         * </summary>
         * <returns>True if the rental is active; otherwise, false.</returns>
         */
        public bool IsActive()
        {
            var now = DateTime.Now;
            return StartDate <= now && EndDate >= now;
        }

        /**
         * <summary>
         * Closes the rental by setting the end date to the current date and time.
         * </summary>
         */
        public void Close()
        {
            EndDate = DateTime.Now;
        }
    }
}
