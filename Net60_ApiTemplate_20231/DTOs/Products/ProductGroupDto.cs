using System.ComponentModel.DataAnnotations;

namespace Net60_ApiTemplate_20231.DTOs.Products
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductGroupDto
    {
        public Guid ProductGrouptId { get; set; }
        public string? ProductGroupName { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public class CreateProductGroupDto
    {
        public string? ProductGroupName { get; set; }
    }

    public class UpdateProductGroupDto
    {
        public string? ProductGroupName { get; set; }
        public bool isActive { get; set; }
    }

    public class UpdateProductGroupResponseDto
    {
        public Guid ProductGrouptId { get; set; }
        public string? ProductGroupName { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public class DeleteProductGroupDto
    {
        public Guid ProductGrouptId { get; set; }
    }

    public class DeleteProductGroupResponseDto
    {
        public Guid ProductGrouptId { get; set; }
        public string? ProductGroupName { get; set; }
        public bool isActive { get; set; }
    }

    public class FilterDataDto
    {
        public bool? isActive { get; set; }
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        public string? Column { get; set; }
        public string? Contain { get; set; }
        public string? SortColumn { get; set; }
        public string? Ordering { get; set; }
    }
}