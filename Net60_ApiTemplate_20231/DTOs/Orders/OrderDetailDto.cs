namespace Net60_ApiTemplate_20231.DTOs.Orders
{
    public class OrderDetailDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool isActive { get; set; }
    }
}
