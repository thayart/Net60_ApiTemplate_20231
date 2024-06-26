using Net60_ApiTemplate_20231.DTOs.Orders;

namespace Net60_ApiTemplate_20231.Services.Order
{
    public interface IOrderServices
    {
        Task<OrderResponseDto> CreateOrder(OrderRequestDto orderRequestDto);
        Task<DeleteOrderResponseDto> DeleteOrder(Guid orderId);
        Task<OrderDto> GetOrderById(Guid orderId);
    }
}
