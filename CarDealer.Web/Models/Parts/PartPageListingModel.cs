namespace CarDealer.Web.Models.Parts
{
    using CarDealer.Services.Models.Parts;
    using System.Collections.Generic;

    public class PartPageListingModel
    {
        public IEnumerable<PartListingModel> Parts;

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage 
            => this.CurrentPage == 1 ? 1 : this.CurrentPage - 1;

        public int NextPage 
            => this.CurrentPage == this.TotalPages ? this.TotalPages : this.CurrentPage + 1;
    }
}
