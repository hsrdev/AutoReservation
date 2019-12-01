using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class ReservationManager
        : ManagerBase
    {
        public async Task<List<Reservation>> GetAll()
        {
            await using CarReservationContext context = new CarReservationContext();

            return await context.Reservations.ToListAsync();
        }

        public async Task<Reservation> Get(int primaryKey)
        {
            await using CarReservationContext context = new CarReservationContext();
            return context.Reservations.Single(c => c.ReservationNr == primaryKey);
        }

        public async Task Insert(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    DateRangeCheck(reservation);
                    await AvailabilityCheck(reservation);
                    context.Entry(reservation).State = EntityState.Added;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }
            }
        }

        public async Task Update(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    await AvailabilityCheck(reservation);
                    context.Entry(reservation).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    CreateOptimisticConcurrencyException(context, reservation);
                }

            }
        }

        public async Task Delete(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(reservation).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        private static async Task AvailabilityCheck(Reservation reservation)
        {
            var carManager = new CarManager();
            var reservationCar = await carManager.Get(reservation.CarId);
            foreach (var madeReservation in reservationCar.Reservations)
            {
                if (reservation.From < madeReservation.To)
                {
                    throw new CarUnavailableException($"{reservationCar.Id} not available until {madeReservation.To}");
                }
            }
        }

        private static void DateRangeCheck(Reservation reservation)
        {
            var timeDifference = reservation.To.Hour - reservation.From.Hour;
            if ( timeDifference < 24 || reservation.To < reservation.From)
            {
                throw new InvalidDateRangeException("Invalid Reservation Range");
            }

        }
    }
}