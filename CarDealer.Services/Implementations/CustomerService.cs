namespace CarDealer.Services.Implementations
{
    using Data;
    using System;
    using Models;
    using System.Linq;
    using Models.Customers;
    using CarDealer.Data.Models;
    using System.Collections.Generic;
    using CarDealer.Services.Models.Sales;

    public class CustomerService : ICustomerService
    {
        private readonly CarDealerDbContext db;

        public CustomerService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public void Create(string name, DateTime birthday, bool isYoungDriver)
        {
            var customer = new Customer
            {
                Name = name,
                BirthDay = birthday,
                IsYoungDriver = isYoungDriver
            };

            this.db.Add(customer);
            this.db.SaveChanges();
        }

        public void Edit(int id, string name, DateTime birthday, bool isYoungDriver)
        {
            var existingCustomer = this.db.Customers.Find(id);

            if (existingCustomer == null)
                return;

            existingCustomer.Name = name;
            existingCustomer.BirthDay = birthday;
            existingCustomer.IsYoungDriver = isYoungDriver;

            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var customer = this.db.Customers.Find(id);

            if (customer == null)
                return;

            this.db.Customers.Remove(customer);
            this.db.SaveChanges();
        }

        public IEnumerable<CustomerModel> Ordered(OrderDirection order)
        {
            var customerQuery = this.db.Customers.AsQueryable();

            switch (order)
            {
                case OrderDirection.Ascending:
                    customerQuery = customerQuery
                        .OrderBy(x => x.BirthDay)
                        .ThenBy(x => x.Name);
                    break;

                case OrderDirection.Descending:
                    customerQuery = customerQuery
                        .OrderByDescending(x => x.BirthDay)
                        .ThenBy(x => x.Name);
                    break;

                default:
                    throw new InvalidOperationException($"Invalid order direction: {order}.");
            }

            return customerQuery.Select(x => new CustomerModel
            {
                Id = x.Id,
                Name = x.Name,
                BirthDay = x.BirthDay,
                IsYoungDriver = x.IsYoungDriver
            })
            .ToList();
        }

        public CustomerModel ById(int id)
            => this.db
                .Customers
                .OrderByDescending(x => x.Id)
                .Where(x => x.Id == id)
                .Select(c => new CustomerModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    BirthDay = c.BirthDay,
                    IsYoungDriver = c.IsYoungDriver
                })
                .FirstOrDefault();

        public CustomerTotalSalesModel TotalSalesById(int id)
        => this.db
            .Customers
            .OrderByDescending(x => x.Id)
            .Where(x => x.Id == id)
            .Select(x => new CustomerTotalSalesModel
            {
                Name = x.Name,
                IsYoungDriver = x.IsYoungDriver,
                BoughtCars = x.Sales.Select(s => new SalesModel
                {
                    Price = s.Car.Parts.Sum(p => p.Part.Price),
                    Discount = s.Discount
                })
            })
            .FirstOrDefault();

        public bool Exists(int id)
            => this.db.Customers.Any(x => x.Id == id);
    }
}
