namespace CarDealer.Services.Models.Customers
{
    using CarDealer.Services.Models.Sales;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomerTotalSalesModel
    {
        public string Name { get; set; }

        public bool IsYoungDriver { get; set; }

        public IEnumerable<SalesModel> BoughtCars { get; set; }
        
        public decimal TotalMoneySpend
            => this.BoughtCars
                    .Sum(c => c.Price * (decimal)(1 - c.Discount))
                    * (this.IsYoungDriver ? 0.95m : 1);



    }
}
