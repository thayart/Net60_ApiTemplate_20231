using System.ComponentModel.DataAnnotations;

namespace Net60_ApiTemplate_20231.DTOs.Products
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductGroupDto : IValidatableObject
    {
        public Guid ProductGrouptId { get; set; }
        public string? ProductGroupName { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ProductGroupName))
                yield return new ValidationResult("Product group could not be null or empty", new[] { "PrductGroupName" });

            //if (CreatedByUserId != 1 && UpdatedByUserId != 1)
            //    yield return new ValidationResult("User must be 1", new[] { "CreatedByUserId", "UpdatedByUserId" });
        }
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

}