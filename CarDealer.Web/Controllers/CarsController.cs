namespace CarDealer.Web.Controllers
{
    using CarDealer.Services;
    using CarDealer.Web.Models.Cars;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    
    [Route("cars")]
    public class CarsController : Controller
    {
        private readonly ICarService cars;
        private readonly IPartService parts;

        public CarsController(ICarService cars, IPartService parts)
        {
            this.cars = cars;
            this.parts = parts;
        }

        [Authorize]
        [Route(nameof(Create))]
        public IActionResult Create()
            => View(new CarFormModel
            {
                AllParts = this.GetPartsListing()
            });

        [Authorize]
        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CarFormModel carModel)
        {
            if (!ModelState.IsValid)
            {
                carModel.AllParts = this.GetPartsListing();
                return View(carModel);
            }

            this.cars.Create(carModel.Make, carModel.Model, carModel.TravelledDistrance, carModel.SelectedParts);

            return RedirectToAction(nameof(Parts));
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id) => View(id);

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            this.cars.Delete(id);

            return RedirectToAction(nameof(Parts));
        }

        [Route("{make}", Order = 2)]
        public IActionResult ByMake(string make)
        {
            var cars = this.cars.ByMake(make);

            return View(new CarsByMakeModel
            {
                Cars = cars,
                Make = make
            });
        }

        [Route("parts", Order = 1)]
        public IActionResult Parts()
            => View(this.cars.WithParts());

        private IEnumerable<SelectListItem> GetPartsListing()
              => this.parts
                     .All()
                     .Select(x => new SelectListItem
                     {
                         Text = x.Name,
                         Value = x.Id.ToString()
                     });


    }
}
