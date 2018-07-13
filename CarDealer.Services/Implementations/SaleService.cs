namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Services.Models.Sales;

    public class SaleService : ISaleService
    {
        private readonly CarDealerDbContext db;

        public SaleService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SaleListModel> All()
             => this.db
                 .Sales
                 .OrderByDescending(x => x.Id)
                 .Select(x => new SaleListModel
                 {
                     Id = x.Id,
                     CustomerName = x.Customer.Name,
                     Price = x.Car.Parts.Sum(p => p.Part.Price),
                     IsYoungDriver = x.Customer.IsYoungDriver,
                     Discount = x.Discount
                 })
                 .ToList();

        public SaleDetailsModel Details(int id)
        => this.db
            .Sales
            .Where(x => x.Id == id)
            .Select(s => new SaleDetailsModel
            {
                Id = s.Id,
                CustomerName = s.Customer.Name,
                Price = s.Car.Parts.Sum(p => p.Part.Price),
                IsYoungDriver = s.Customer.IsYoungDriver,
                Discount = s.Discount,
                Car = new CarModel
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TravelledDistance = s.Car.TravelledDistance
                }
            })
            .FirstOrDefault();
    }
}
