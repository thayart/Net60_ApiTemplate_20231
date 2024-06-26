using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Net60_ApiTemplate_20231.DTOs.Products
{
    public class ProductDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public class ProductRequestDto
    {
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
    }
    public class ProductResponseDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public class UpdateProductRequestDto
    {
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public bool isActive { get; set; }
    }
    public class UpdateProductResponseDto
    {
        public Guid ProductId { get; set; }
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public class DeleteProductResponseDto {
        public Guid ProductGroupId { get; set; }
        public string? ProductName { get; set; }
        public bool isActive { get; set; }
    }
}
