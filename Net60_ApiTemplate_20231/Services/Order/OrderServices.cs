using System.Linq.Dynamic.Core;
using AutoMapper;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Net60_ApiTemplate_20231.Services.Product;
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
        private readonly Serilog.ILogger _logger;
        public OrderServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices, Serilog.ILogger? logger = null) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext;
            _loginDetailServices = loginDetailServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductServices)) : logger.ForContext("ServiceName", nameof(ProductServices));
        }

        public async Task<OrderResponseDto> CreateOrder(OrderRequestDto orderRequestDto)
        {
            const string actionName = nameof(CreateOrder);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var user = _loginDetailServices.GetClaim();
            var orders = _mapper.Map<Models.Order>(orderRequestDto);

            #region Setup CreateData
            // Set Order Table
            orders.OrderId = Guid.NewGuid();
            orders.ItemCount = orderRequestDto.OrderDetails == null ? 0 : orderRequestDto.OrderDetails.Count();
            orders.CreatedByUserId = user.UserId;
            orders.CreatedDate = DateTime.Now;
            orders.UpdatedByUserId = user.UserId;
            orders.UpdatedDate = DateTime.Now;
            orders.isActive = true;

            // Set OrderDetail Table

            foreach (var item in orders.OrderDetails)
            {
                item.OrderDetailId = Guid.NewGuid();
                item.OrderId = orders.OrderId;
                item.CreatedByUserId = user.UserId;
                item.CreatedDate = DateTime.Now;
                item.UpdatedByUserId = user.UserId;
                item.UpdatedDate = DateTime.Now;
                item.isActive = true;
            }

            #endregion

            _dbContext.Add(orders);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<OrderResponseDto>(orders);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{OrderId} ", actionName, DateTime.Now, dto.OrderId);

            return dto;
        }

        public async Task<DeleteOrderResponseDto> DeleteOrder(Guid orderId)
        {
            const string actionName = nameof(DeleteOrder);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{OrderId} ", actionName, DateTime.Now, orderId);

            var user = _loginDetailServices.GetClaim();
            var getOrder = _dbContext.Orders.FirstOrDefault(f => f.OrderId == orderId);
            var getOrderDetail = _dbContext.OrderDetails.Where(f => f.OrderId == orderId).ToList();

            #region Setup DeleteData

            // Set Order Table
            getOrder.UpdatedDate = DateTime.Now;
            getOrder.UpdatedByUserId = user.UserId;
            getOrder.isActive = false;

            // Set OrderDetail Table
            foreach (var item in getOrder.OrderDetails)
            {
                item.UpdatedByUserId = user.UserId;
                item.UpdatedDate = DateTime.Now;
                item.isActive = false;
            }
            #endregion

            _dbContext.Update(getOrder);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<DeleteOrderResponseDto>(getOrder);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{OrderId} ", actionName, DateTime.Now, orderId);

            return dto;
        }

        public async Task<OrderResponseDto> GetOrderById(Guid orderId)
        {
            const string actionName = nameof(GetOrderById);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{OrderId} ", actionName, DateTime.Now, orderId);

            // Setup GetData
            var getOrder = await _dbContext.Orders.FirstOrDefaultAsync(f => f.OrderId == orderId);
            // var getOrderDetail = await _dbContext.OrderDetails.Where(f => f.OrderId == orderId).ToListAsync();
            var getOrderDetail = await _dbContext.OrderDetails.Include(i => i.ProductId).ToListAsync();


            // Return Data MaptoDto
            var dto = _mapper.Map<OrderResponseDto>(getOrder);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{OrderId} ", actionName, DateTime.Now, orderId);

            return dto;
        }
    }
}
