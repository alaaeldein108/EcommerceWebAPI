using System.ComponentModel.DataAnnotations;

namespace Store.Service.BasketService.Dtos
{
    public class BasketItemDto
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(1,double.MaxValue,ErrorMessage ="Price Must be Grater Than Zero")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 10,ErrorMessage ="Quantity must be Between 1 and 10 Pieces")]
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
        public string BrandName { get; set; }
        public string TypeName { get; set; }
    }
}