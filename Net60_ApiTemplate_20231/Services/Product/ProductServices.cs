using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Serilog;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public class ProductServices : ServiceBase , IProductServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;

        public ProductServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext;
            _loginDetailServices = loginDetailServices;
        }

        public async Task<ProductResponseDto> CreateProduct(ProductRequestDto productRequestDto)
        {
            const string _serviceName = nameof(Product);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var product = _mapper.Map<Models.Product>(productRequestDto);

            product.ProductId = Guid.NewGuid();
            product.CreatedByUserId = userId;
            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;
            product.UpdatedByUserId = userId;
            product.isActive = true;
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<ProductResponseDto>(product);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async  Task<UpdateProductResponseDto> UpdateProduct(Guid ProductId , UpdateProductRequestDto updateProductRequestDto)
        {
            const string _serviceName = nameof(Product);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var updateproduct = _mapper.Map<Models.Product>(updateProductRequestDto);

            var getProduct = _dbContext.Products.FirstOrDefault(f => f.ProductId == ProductId);
            getProduct.ProductGroupId = updateproduct.ProductGroupId;
            getProduct.ProductName = updateproduct.ProductName;
            getProduct.ProductPrice = updateproduct.ProductPrice;
            getProduct.isActive = updateproduct.isActive;

            _dbContext.Update(getProduct);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<UpdateProductResponseDto>(getProduct);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<DeleteProductResponseDto> DeleteProduct(Guid productId)
        {
            const string _serviceName = nameof(DeleteProduct);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var getProduct = _dbContext.Products.FirstOrDefault(f => f.ProductId == productId);

            getProduct.UpdatedDate = DateTime.Now;
            getProduct.UpdatedByUserId = userId;
            getProduct.isActive = false;

            _dbContext.Update(getProduct);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<DeleteProductResponseDto>(getProduct);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            const string _serviceName = nameof(GetProductById);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var getProduct = await _dbContext.Products.FirstOrDefaultAsync(f => f.ProductId == productId);

            var dto = _mapper.Map<ProductDto>(getProduct);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }
    }
}
