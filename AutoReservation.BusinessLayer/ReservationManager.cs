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
                    throw new InvalidDateRangeException(
                        $"Reservation < 24h -> {(reservation.From - reservation.To).Hours}h");
                }

                /*if (!await AvailabilityCheck(reservation))
                {
                    throw new CarUnavailableException($"car: {reservation.CarId} unavailable");
                }*/
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
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
                        $"Reservation < 24h -> {(reservation.From - reservation.To).Hours}h");
                }

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

        public async Task<bool> AvailabilityCheck(Reservation reservation)
        {
            var allReservations = await GetAll();
            var targetCarReservations = allReservations.FindAll(c => c.CarId == reservation.CarId);
            foreach (var targetReservation in targetCarReservations)
            {
                if (IsDateInTargetReservationRange(reservation.From, targetReservation) || IsDateInTargetReservationRange(reservation.To, targetReservation))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsDateInTargetReservationRange(DateTime reservationDate, Reservation targetReservation)
        {
            return reservationDate >= targetReservation.From && reservationDate < targetReservation.To;

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