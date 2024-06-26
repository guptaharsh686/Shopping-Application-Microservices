﻿using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ProductAPI.Models.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string CategoryName { get; set; }

		//global refrence with domain name
		public string? ImageUrl { get; set; }

		//path respective to wwwroot
		public string? ImageLocalPath { get; set; }

		public IFormFile? Image { get; set; }

	}
}
