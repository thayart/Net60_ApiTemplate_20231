using System.Linq.Dynamic.Core;
using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Serilog;
using static NuGet.Packaging.PackagingConstants;

namespace Net60_ApiTemplate_20231.Services.Order
{
    public class OrderServices : ServiceBase, IOrderServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;
        public OrderServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext;
            _loginDetailServices = loginDetailServices;
        }

        public async Task<OrderResponseDto> CreateOrder(OrderRequestDto orderRequestDto)
        {
            const string _serviceName = nameof(CreateOrder);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            int userId = _loginDetailServices.GetClaim().UserId;
            Models.Order orders = _mapper.Map<Models.Order>(orderRequestDto);

            // Set Order Table
            orders.OrderId = Guid.NewGuid();
            orders.ItemCount = orderRequestDto.OrderDetails == null ? 0 : orderRequestDto.OrderDetails.Count();
            orders.CreatedByUserId = userId;
            orders.CreatedDate = DateTime.Now;
            orders.UpdatedByUserId = userId;
            orders.UpdatedDate = DateTime.Now;
            orders.isActive = true;

            // Set OrderDetail Table

            foreach (var item in orders.OrderDetails)
            {
                item.OrderDetailId = Guid.NewGuid();
                item.OrderId = orders.OrderId;
                item.CreatedByUserId = userId;
                item.CreatedDate = DateTime.Now;
                item.UpdatedByUserId = userId;
                item.UpdatedDate = DateTime.Now;
                item.isActive = true;
            }
            _dbContext.Add(orders);
            await _dbContext.SaveChangesAsync();


            OrderResponseDto dto = _mapper.Map<OrderResponseDto>(orders);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<DeleteOrderResponseDto> DeleteOrder(Guid orderId)
        {
            const string _serviceName = nameof(DeleteOrder);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            int userId = _loginDetailServices.GetClaim().UserId;
            Models.Order? getOrder = _dbContext.Orders.FirstOrDefault(f => f.OrderId == orderId);
            List<OrderDetail> getOrderDetail = _dbContext.OrderDetails.Where(f => f.OrderId == orderId).ToList();

            // Set Order Table
            getOrder.UpdatedDate = DateTime.Now;
            getOrder.UpdatedByUserId = userId;
            getOrder.isActive = false;

            // Set OrderDetail Table
            foreach (var item in getOrder.OrderDetails)
            {
                item.UpdatedByUserId = userId;
                item.UpdatedDate = DateTime.Now;
                item.isActive = false;
            }

            _dbContext.Update(getOrder);
            await _dbContext.SaveChangesAsync();

            DeleteOrderResponseDto dto = _mapper.Map<DeleteOrderResponseDto>(getOrder);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<OrderDto> GetOrderById(Guid orderId)
        {
            const string _serviceName = nameof(GetOrderById);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            int userId = _loginDetailServices.GetClaim().UserId;
            Models.Order? getOrder = await _dbContext.Orders.FirstOrDefaultAsync(f => f.OrderId == orderId);

            OrderDto dto = _mapper.Map<OrderDto>(getOrder);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }
    }
}
