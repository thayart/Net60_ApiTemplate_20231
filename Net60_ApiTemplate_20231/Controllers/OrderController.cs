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
    /// <summary>
    /// Order Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderServices _orderServices;
        private readonly Serilog.ILogger _logger;

        public OrderController(IOrderServices  orderServices, Serilog.ILogger? logger = null)
        {
            _orderServices = orderServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductController)) : logger.ForContext("ServiceName", nameof(ProductController));
        }


        /// <summary>
        ///     CreateOrder
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost(Name = "CreateOrder")]
        public async Task<ServiceResponse<OrderResponseDto>> CreateOrder([FromBody] OrderRequestDto orderRequestDto)
        {
            const string actionName = nameof(CreateOrder);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var result = await _orderServices.CreateOrder(orderRequestDto);

            _logger.Information("[{actionName}] - Sussess: {date} - Id: {OrderId}", actionName, DateTime.Now , result.OrderId);

            return ResponseResult.Success(result);
        }

        /// <summary>
        ///     DeleteOrder
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost("{orderId}/delete", Name = "DeleteOrder")]
        public async Task<ServiceResponse<DeleteOrderResponseDto>> DeleteOrder(Guid orderId)
        {
            const string actionName = nameof(DeleteOrder);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            DeleteOrderResponseDto result = await _orderServices.DeleteOrder(orderId);

            _logger.Information("[{actionName}] - Sussess: {date} - Id: {OrderId}", actionName, DateTime.Now, orderId);

            return ResponseResult.Success(result);
        }

        /// <summary>
        ///     GetOrderById
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<ServiceResponse<OrderResponseDto>> GetOrderById (Guid orderId)
        {
            const string actionName = nameof(GetOrderById);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var result = await _orderServices.GetOrderById(orderId);


            _logger.Information("[{actionName}] - Sussess: {date} - Id: {OrderId}", actionName, DateTime.Now, result.OrderId);

            return ResponseResult.Success(result);
        }
    }
}
