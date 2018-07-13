namespace CarDealer.Web.Controllers
{
    using Services;
    using Models.Suppliers;
    using Microsoft.AspNetCore.Mvc;

    public class SuppliersController : Controller
    {
        private const string SuppliersView = "Suppliers";

        private readonly ISupplierService suppliers;

        public SuppliersController(ISupplierService suppliers)
        {
            this.suppliers = suppliers;
        }

        public IActionResult Local()
            //false because of the bool importers in "GetSupplierModel" method below
            => this.View(SuppliersView, this.GetSupplierModel(false));
        

        public IActionResult Importers()
            => this.View(SuppliersView, this.GetSupplierModel(true));
        

        private SuppliersModel GetSupplierModel(bool importers)
        {
            var type = importers ? "Importer" : "Local";

            var suppliers = this.suppliers.All(importers);

            return new SuppliersModel
            {
                Type = type,
                Suppliers = suppliers
            };
        }
    }
}
