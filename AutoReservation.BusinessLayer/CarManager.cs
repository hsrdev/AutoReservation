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
            await using CarReservationContext context = new CarReservationContext();

            return await context.Cars.ToListAsync();
        }

        public async Task<Car> Get(int primaryKey)
        {
            await using CarReservationContext context = new CarReservationContext();
            return context.Cars.Single(c => c.Id == primaryKey);
        }

        public async Task<Car> Insert(Car car)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {

                context.Entry(car).State = EntityState.Added;
                context.SaveChanges();
                return car;
            }
        }

        public async Task Update(Car car)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    context.Entry(car).State = EntityState.Modified;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    CreateOptimisticConcurrencyException(context, car);
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