using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }

        [Range(1,1000)]
        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

        //global refrence with domain name
        public string? ImageUrl { get; set; }

        //path respective to wwwroot
		public string? ImageLocalPath { get; set; }



	}
}
