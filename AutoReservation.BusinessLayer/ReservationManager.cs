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

        public async Task<Reservation> Insert(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                if (!DateRangeCheck(reservation))
                {
                    throw new InvalidDateRangeException("Reservation < 24h");
                }
                if (!await AvailabilityCheck(reservation))
                {
                    throw new CarUnavailableException("car unavailable");
                }
                await AvailabilityCheck(reservation);
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
                return reservation;
            }
        }

        public async Task Update(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(reservation).State = EntityState.Modified;
                context.SaveChanges();
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

        private static async Task<bool> AvailabilityCheck(Reservation reservation)
        {
            var carManager = new CarManager();
            var reservationCar = await carManager.Get(reservation.CarId);
            foreach (var madeReservation in reservationCar.Reservations)
            {
                if (reservation.From < madeReservation.To)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool DateRangeCheck(Reservation reservation)
        {
            var timeDifference = (reservation.To - reservation.From).Hours;
            if (timeDifference < 24)
            {
                return false;
            }

            return true;
        }
    }
}