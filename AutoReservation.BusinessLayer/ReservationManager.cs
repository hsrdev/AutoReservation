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
            return await context.Reservations.Include(r => r.Car).Include(r => r.Customer).ToListAsync();
        }

        public async Task<Reservation> Get(int primaryKey)
        {
            await using CarReservationContext context = new CarReservationContext();
            return await context.Reservations.Include(r => r.Car).Include(r => r.Customer)
                .SingleAsync(c => c.ReservationNr == primaryKey);
        }

        public async Task<Reservation> Insert(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                if (!DateRangeCheck(reservation))
                {
                    throw new InvalidDateRangeException(
                        $"Reservation < 24h -> {(reservation.From - reservation.To).TotalHours}h");
                }

                if (!await AvailabilityCheck(reservation))
                {
                    throw new CarUnavailableException($"car: {reservation.CarId} unavailable");
                }

                context.Entry(reservation).State = EntityState.Added;
                await context.SaveChangesAsync();
                return reservation;
            }
        }

        public async Task Update(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                if (!DateRangeCheck(reservation))
                {
                    throw new InvalidDateRangeException(
                        $"Reservation < 24h -> {(reservation.From - reservation.To).TotalHours}h");
                }

                if (!await AvailabilityCheck(reservation))
                {
                    throw new CarUnavailableException($"car: {reservation.CarId} unavailable");
                }

                context.Entry(reservation).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(reservation).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> AvailabilityCheck(Reservation reservation)
        {
            var allReservations = await GetAll();
            var targetCarReservations = allReservations.FindAll(r =>
                r.CarId == reservation.CarId && r.ReservationNr != reservation.ReservationNr);
            //if (isUpdateOfReservation(targetCarReservations, reservation.ReservationNr)) return true;
            foreach (var targetReservation in targetCarReservations)
            {
                if (IsFromDateInTargetReservationRange(reservation.From, targetReservation)
                    || IsToDateInTargetReservationRange(reservation.To, targetReservation))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsFromDateInTargetReservationRange(DateTime reservationDate, Reservation targetReservation)
        {
            return reservationDate >= targetReservation.From && reservationDate < targetReservation.To;
        }

        private bool IsToDateInTargetReservationRange(DateTime reservationDate, Reservation targetReservation)
        {
            return reservationDate > targetReservation.From && reservationDate < targetReservation.To;
        }


        public bool DateRangeCheck(Reservation reservation)
        {
            var timeDifference = reservation.To.Subtract(reservation.From).TotalHours;
            if (timeDifference < 24 || reservation.From < DateTime.Now)
            {
                return false;
            }

            return true;
        }
    }
}