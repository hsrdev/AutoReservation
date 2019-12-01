using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Car> Get(int primaryKey)
        {
            await using CarReservationContext context = new CarReservationContext();
            return context.Cars.Single(c => c.Id == primaryKey);
        }

        public async Task Insert(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(reservation).State = EntityState.Added;
                context.SaveChanges();
            }
        }

        public async Task Update(Reservation reservation)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
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

        public async Task Delete(Car car)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(car).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}