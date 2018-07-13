namespace CarDealer.Services.Implementations
{
    using System.Linq;
    using CarDealer.Data;
    using System.Collections.Generic;
    using CarDealer.Services.Models.Suppliers;

    public class SupplierService : ISupplierService
    {
        private readonly CarDealerDbContext db;

        public SupplierService(CarDealerDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SupplierListingModel> All(bool isImpoter)
            => this.db
                 .Suppliers
                 .OrderByDescending(x => x.Id)
                 .Where(x => x.IsImporter == isImpoter)
                 .Select(x => new SupplierListingModel
                 {
                     Id = x.Id,
                     Name = x.Name,
                     TotalParts = x.Parts.Count
                 })
                .ToList();


        public IEnumerable<SupplierModel> All()
            => this.db
                .Suppliers
                .OrderBy(x => x.Name)
                .Select(x => new SupplierModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();
    }
}
