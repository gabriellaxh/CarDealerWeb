namespace CarDealer.Web.Controllers
{
    using Infrastructure.Extensions;
    using CarDealer.Services;
    using Services.Models;
    using Microsoft.AspNetCore.Mvc;
    using CarDealer.Web.Models.Customers;

    [Route("customers")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService customers;

        public CustomersController(ICustomerService customers)
        {
            this.customers = customers;
        }

        [Route(nameof(Create))]
        public IActionResult Create() => View();

        [HttpPost]
        [Route(nameof(Create))]
        public IActionResult Create(CustomerFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            this.customers.Create(model.Name, model.Birthday, model.IsYoungDriver);

            //new { order = OrderDirection.Ascending.ToString() }) - this is because the action All requires string order
            return RedirectToAction(nameof(All), new { order = OrderDirection.Ascending.ToString() });
        }

        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id)
        {
            var customer = this.customers.ById(id);

            if (customer == null)
                return NotFound();

            return View(new CustomerFormModel
            {
                Name = customer.Name,
                Birthday = customer.BirthDay,
                IsYoungDriver = customer.IsYoungDriver
            });
        }

        [HttpPost]
        [Route(nameof(Edit) + "/{id}")]
        public IActionResult Edit(int id, CustomerFormModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool customerExists = this.customers.Exists(id);

            if (!customerExists)
                return NotFound();

            this.customers.Edit(id, model.Name, model.Birthday, model.IsYoungDriver);

            //new { order = OrderDirection.Ascending.ToString() }) - this is because the action All requires string order
            return RedirectToAction(nameof(All), new { order = OrderDirection.Ascending.ToString() });
        }

        [Route("delete/{id}")]
        public IActionResult Delete(int id) => View(id);

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            this.customers.Delete(id);

            return RedirectToAction(nameof(All), new { order = OrderDirection.Ascending });
        }

        [Route("all/{order}")]
        public IActionResult All(string order)
        {
            var orderDirection = order.ToLower() == "descending"
                ? OrderDirection.Descending
                : OrderDirection.Ascending;

            var customers = this.customers.Ordered(orderDirection);

            return View(new AllCustomersModel
            {
                Customers = customers,
                OrderDirection = orderDirection
            });
        }

        [Route("{id}")]
        public IActionResult TotalSales(int id)
             => this.ViewOrNotFound(this.customers.TotalSalesById(id));

    }
}
