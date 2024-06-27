using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Order;
using Net60_ApiTemplate_20231.Services.Product;
using Serilog;

namespace Net60_ApiTemplate_20231.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;

        public OrderController(IOrderServices  orderServices)
        {
            _orderServices = orderServices;
        }


        [HttpPost]
        public async Task<ServiceResponse<OrderResponseDto>> CreateOrder([FromBody] OrderRequestDto orderRequestDto)
        {
            const string _serviceName = nameof(CreateOrder);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            OrderResponseDto result = await _orderServices.CreateOrder(orderRequestDto);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }

        [HttpPost("{orderId}/delete")]
        public async Task<ServiceResponse<DeleteOrderResponseDto>> DeleteOrder(Guid orderId)
        {
            const string _serviceName = nameof(DeleteOrder);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            DeleteOrderResponseDto result = await _orderServices.DeleteOrder(orderId);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }

        [HttpGet("{orderId}")]
        public async Task<ServiceResponse<OrderDto>> GetOrderById (Guid orderId)
        {
            const string _serviceName = nameof(GetOrderById);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            OrderDto result = await _orderServices.GetOrderById(orderId);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }
    }
}
