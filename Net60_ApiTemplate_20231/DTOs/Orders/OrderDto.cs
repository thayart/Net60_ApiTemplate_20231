using System.ComponentModel.DataAnnotations.Schema;
using Net60_ApiTemplate_20231.DTOs.Products;

namespace Net60_ApiTemplate_20231.DTOs.Orders
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Net { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }
   
    public class OrderRequestDto
    {
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Net { get; set; }
        public List<OrderDetailsRequestDto>? OrderDetails { get; set; }

    }
    public class OrderDetailsRequestDto
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
    }
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal Net { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public List<OrderDetailsResponseDto>? OrderDetails { get; set; }

    }

    public class OrderDetailsResponseDto
    {
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
        public ProductDto? Products { get; set; }
    }

    public class DeleteOrderResponseDto
    {
        public Guid OrderId { get; set; }
        public bool isActive { get; set; }
    }



}
