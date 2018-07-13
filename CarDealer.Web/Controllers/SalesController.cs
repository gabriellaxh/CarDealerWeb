﻿namespace CarDealer.Web.Controllers
{
    using Services;
    using Microsoft.AspNetCore.Mvc;
    using Infrastructure.Extensions;

    [Route("sales")]
    public class SalesController : Controller
    {
        private readonly ISaleService sales;

        public SalesController(ISaleService sales)
        {
            this.sales = sales;
        }

        [Route("")]
        public IActionResult All() => View(this.sales.All());

        [Route("{id}")]
        public IActionResult Details(int id)
            => this.ViewOrNotFound(this.sales.Details(id));
    }
}
