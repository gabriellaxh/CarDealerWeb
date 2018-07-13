namespace CarDealer.Services.Models.Parts
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PartListingModel : PartModel
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public string SupplierName { get; set; }
    }
}
