using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class CarManager
        : ManagerBase
    {
        public async Task<List<Car>> GetAll()
        {
            using CarReservationContext context = new CarReservationContext();
            return await context.Cars.Include(c => c.Reservations).ToListAsync();
        }

        public async Task<Car> Get(int primaryKey)
        {
            try
            {
                using CarReservationContext context = new CarReservationContext();
                return await context.Cars.Include(c => c.Reservations).SingleAsync(c => c.Id == primaryKey);
            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message, ex);
            }
        }

        public async Task<Car> Insert(Car car)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(car).State = EntityState.Added;
                await context.SaveChangesAsync();
                return car;
            }
        }

        public async Task Update(Car car)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    context.Entry(car).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw CreateOptimisticConcurrencyException(context, car);
                }
            }
        }

        public async Task Delete(Car car)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(car).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}