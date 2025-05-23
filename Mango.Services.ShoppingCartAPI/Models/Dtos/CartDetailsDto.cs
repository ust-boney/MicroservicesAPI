﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.Dtos
{
    public class CartDetailsDto
    {
        public int CartDetailId { get; set; }
        public int CartHeaderId { get; set; }
        //public CartHeader? CartHeader { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        //public ProductDto Product { get; set; }
    }
}
