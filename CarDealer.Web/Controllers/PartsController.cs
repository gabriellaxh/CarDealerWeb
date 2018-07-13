namespace CarDealer.Web.Controllers
{
    using System;
    using System.Linq;
    using CarDealer.Services;
    using Microsoft.AspNetCore.Mvc;
    using CarDealer.Web.Models.Parts;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;

    [Route("parts")]
    public class PartsController : Controller
    {
        private const int PageSize = 25;

        private readonly IPartService parts;
        private readonly ISupplierService suppliers;

        public PartsController(IPartService parts, ISupplierService suppliers)
        {
            this.parts = parts;
            this.suppliers = suppliers;
        }

        [Route(nameof(Create))]
        public IActionResult Create()
            => View(new PartFormModel
            {
                Suppliers = this.GetSuppliersList()
            });

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(PartFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Suppliers = this.GetSuppliersList();
                return View(model);
            }

            this.parts.Create(model.Name, model.Price, model.Quantity, model.SupplierId);

            return RedirectToAction(nameof(All));
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id) => View(id);
        
        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            this.parts.Delete(id);

            return RedirectToAction(nameof(All));
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var part = this.parts.ById(id);

            if (part == null)
                return NotFound();

            return View(new PartFormModel
            {
                Name = part.Name,
                Price = part.Price,
                Quantity = part.Quantity,
                IsEdit = true
            });
        }

        [HttpPost]
        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id, PartFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.IsEdit = true;
                return View(model);
            }

            this.parts.Edit(id, model.Price, model.Quantity);

            return RedirectToAction(nameof(All));
        }

        private IEnumerable<SelectListItem> GetSuppliersList()
             => this.suppliers
                                .All()
                                .Select(x => new SelectListItem
                                {
                                    Text = x.Name,
                                    Value = x.Id.ToString()
                                });

        [Route("all")]
        public IActionResult All(int page = 1)
            => View(new PartPageListingModel
            {
                Parts = this.parts.AllListings(page, PageSize),
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(this.parts.Total() / (double)PageSize)
            });


    }
}
