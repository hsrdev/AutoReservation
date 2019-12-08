using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoReservation.Dal;
using AutoReservation.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoReservation.BusinessLayer
{
    public class CustomerManager
        : ManagerBase
    {
        public async Task<List<Customer>> GetAll()
        {
            await using CarReservationContext context = new CarReservationContext();

            return await context.Customers.ToListAsync();
        }

        public async Task<Customer> Get(int primaryKey)
        {
            await using CarReservationContext context = new CarReservationContext();
            return context.Customers.Single(c => c.Id == primaryKey);
        }

        public async Task<Customer> Insert(Customer customer)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(customer).State = EntityState.Added;
                context.SaveChanges();
                return customer;
            }
        }

        public async Task Update(Customer customer)
        {
            await using (CarReservationContext context = new CarReservationContext())
            {
                try
                {
                    context.Entry(customer).State = EntityState.Modified;
                    context.SaveChanges();
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
            await using (CarReservationContext context = new CarReservationContext())
            {
                context.Entry(customer).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }
    }
}