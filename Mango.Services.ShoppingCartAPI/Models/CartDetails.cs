using Mango.Services.ShoppingCartAPI.Models.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailId { get; set; }
        [ForeignKey("CartHeaderId")]
        public int CartHeaderId { get; set; }
        public CartHeader? CartHeader { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }

    }
}
