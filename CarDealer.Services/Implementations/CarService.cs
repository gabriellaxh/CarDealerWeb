namespace CarDealer.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using CarDealer.Data;
    using CarDealer.Services.Models.Parts;
    using CarDealer.Services.Models.Cars;
    using CarDealer.Data.Models;

    public class CarService : ICarService
    {
        private readonly CarDealerDbContext db;

        public CarService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<CarModel> ByMake(string make)
        {
            return this.db
                 .Cars
                 .Where(x => x.Make.ToLower() == make.ToLower())
                 .OrderBy(x => x.Model)
                 .ThenBy(x => x.TravelledDistance)
                 .Select(x => new CarModel
                 {
                     Id = x.Id,
                     Make = x.Make,
                     Model = x.Model,
                     TravelledDistance = x.TravelledDistance
                 })
                 .ToList();
        }

        public void Create(string make, string model, long travelledDistrance, IEnumerable<int> parts)
        {
            var existingPartIds = this.db
                                      .Parts
                                      .Where(x => parts.Contains(x.Id))
                                      .Select(x => x.Id)
                                      .ToList();
            var newCar = new Car
            {
                Make = make,
                Model = model,
                TravelledDistance = travelledDistrance,
                
            };

            foreach (var partId in existingPartIds)
            {
                newCar.Parts.Add(new PartCar { PartId = partId });
            }

            this.db.Add(newCar);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = this.db.Cars.Find(id);

            if (car == null)
                return;

            this.db.Cars.Remove(car);
            this.db.SaveChanges();
        }
        
        public IEnumerable<CarWithPartsModel> WithParts()
         => this.db
            .Cars
            .OrderByDescending(x => x.Id)
            .Select(x => new CarWithPartsModel
            {
                Id = x.Id,
                Make = x.Make,
                Model = x.Model,
                TravelledDistance = x.TravelledDistance,
                Parts = x.Parts
                         .Select(p => new PartModel
                         {
                             Name = p.Part.Name,
                             Price = p.Part.Price
                         })
            })
            .ToList();
    }
}

