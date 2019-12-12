using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class CustomerManager : ManagerBase
    {
        public async Task<List<Customer>> GetAll()
        {
            using CarReservationContext context = new CarReservationContext();
            return await context.Customers.Include(c => c.Reservations).ToListAsync();
        }

        public async Task<Customer> Get(int primaryKey)
        {
            using CarReservationContext context = new CarReservationContext();
            try
            {
                return await context.Customers.Include(c => c.Reservations).SingleAsync(c => c.Id == primaryKey);

            }
            catch (Exception ex)
            {
                throw new KeyNotFoundException(ex.Message, ex);
            }        
        }

        public async Task<Customer> Insert(Customer customer)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(customer).State = EntityState.Added;
                await context.SaveChangesAsync();
                return customer;
            }
        }

        public async Task Update(Customer customer)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    context.Entry(customer).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw CreateOptimisticConcurrencyException(context, customer);
                }
            }
        }

        public async Task Delete(Customer customer)
        {
            using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(customer).State = EntityState.Deleted;
                await context.SaveChangesAsync();
            }
        }
    }
}