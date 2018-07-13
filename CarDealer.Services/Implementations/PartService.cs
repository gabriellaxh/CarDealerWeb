namespace CarDealer.Services.Implementations
{
    using CarDealer.Data;
    using CarDealer.Data.Models;
    using CarDealer.Services.Models.Parts;
    using System.Collections.Generic;
    using System.Linq;

    public class PartService : IPartService
    {
        private readonly CarDealerDbContext db;

        public PartService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<PartBasicModel> All()
            => this.db
                .Parts
                .OrderBy(x => x.Id)
                .Select(x => new PartBasicModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

        public IEnumerable<PartListingModel> AllListings(int page = 1, int pageSize = 10)
            => this.db
                 .Parts
                 .OrderByDescending(x => x.Id)
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize)
                 .Select(x => new PartListingModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     Price = x.Price,
                     Quantity = x.Quantity,
                     SupplierName = x.Supplier.Name
                 })
                 .ToList();

        public PartEditModel ById(int id)
            => this.db
                .Parts
                .Where(x => x.Id == id)
                .Select(x => new PartEditModel
                {
                    Name = x.Name,
                    Price = x.Price,
                    Quantity = x.Quantity
                })
                .FirstOrDefault();

        public void Create(string name, decimal price, int quantity, int supplierId)
        {
            var newPart = new Part
            {
                Name = name,
                Price = price,
                Quantity = quantity > 0 ? quantity : 1,
                SupplierId = supplierId
            };

            this.db.Add(newPart);
            this.db.SaveChanges();
        }

        public void Delete(int id)
        {
            var part = this.db.Parts.Find(id);

            if (part == null)
                return;

            this.db.Parts.Remove(part);
            this.db.SaveChanges();
        }

        public void Edit(int id, decimal price, int quantity)
        {
            var part = this.db.Parts.Find(id);

            if (part == null)
                return;

            part.Price = price;
            part.Quantity = quantity;

            this.db.SaveChanges();
        }

        public int Total()
            => this.db.Parts.Count();
    }
}
