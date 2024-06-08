﻿using Mango.Services.OrderAPI.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }

        //Capture the name and proce at the time of order
        public string ProductName { get; set; }

        public double Price { get; set; }

        [NotMapped]
        public ProductDto? Product { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }


    }
}
