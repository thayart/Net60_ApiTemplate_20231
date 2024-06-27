using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Helpers;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Serilog;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public class ProductServices : ServiceBase, IProductServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpContext? _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;
        private readonly Serilog.ILogger _logger;

        public ProductServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices, Serilog.ILogger? logger = null) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext.HttpContext;
            _loginDetailServices = loginDetailServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductServices)) : logger.ForContext("ServiceName", nameof(ProductServices));
        }

        public async Task<ProductResponseDto> CreateProduct(ProductRequestDto productRequestDto)
        {
            const string actionName = nameof(CreateProduct);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var user = _loginDetailServices.GetClaim();

            // Setup Data
            var product = _mapper.Map<Models.Product>(productRequestDto);

            product.ProductId = Guid.NewGuid();
            product.CreatedByUserId = user.UserId;
            product.CreatedDate = DateTime.Now;
            product.UpdatedDate = DateTime.Now;
            product.UpdatedByUserId = user.UserId;
            product.isActive = true;
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<ProductResponseDto>(product);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductId} ", actionName, DateTime.Now, dto.ProductId);

            return dto;
        }

        public async Task<UpdateProductResponseDto> UpdateProduct(Guid productId, UpdateProductRequestDto updateProductRequestDto)
        {
            const string actionName = nameof(Product);
            //var logContext = _logger.ForContext("ActionName", nameof(UpdateProduct));
            //logContext.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            var user = _loginDetailServices.GetClaim();

            // Setup UpdateData
            var getProduct = _dbContext.Products.FirstOrDefault(f => f.ProductId == productId);
            getProduct = _mapper.Map(updateProductRequestDto , getProduct);
            getProduct.UpdatedDate = DateTime.Now;
            getProduct.UpdatedByUserId = user.UserId;
            
            _dbContext.Update(getProduct);
            await _dbContext.SaveChangesAsync();

            //Return Data MaptoDto
            var dto = _mapper.Map<UpdateProductResponseDto>(getProduct);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            return dto;
        }

        public async Task<DeleteProductResponseDto> DeleteProduct(Guid productId)
        {
            const string actionName = nameof(DeleteProduct);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            var user = _loginDetailServices.GetClaim();

            // Setup DeleteData
            var getProduct = _dbContext.Products.FirstOrDefault(f => f.ProductId == productId);
            getProduct.UpdatedDate = DateTime.Now;
            getProduct.UpdatedByUserId = user.UserId;
            getProduct.isActive = false;

            _dbContext.Update(getProduct);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<DeleteProductResponseDto>(getProduct);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            return dto;
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            const string actionName = nameof(GetProductById);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            // Setup GetData
            var getProduct =await _dbContext.Products.Where(f => f.ProductGroupId == productId).ToListAsync();
            //var product = await _dbContext.Products
            //    .Where(f => f.ProductId == productId)
            //    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            //    .FirstOrDefaultAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<ProductDto>(getProduct);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductId} ", actionName, DateTime.Now, productId);

            return dto;
        }

        public async Task<(List<ProductDto> productDtos, PaginationResultDto pagination)> GetAllProduct(
            [FromQuery] PaginationDto paginationDto
            , [FromQuery] QueryFilterDto filterDto
            , [FromQuery] QuerySortDto sortDto
            )
        {
            const string actionName = nameof(GetAllProduct);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            // Setup GetListData
            var getProducts = _dbContext.Products.AsQueryable();

            // Flag IsActive
            if (paginationDto.isActive != null)
            {
                getProducts = getProducts.Where(w => w.isActive == paginationDto.isActive);
            }

            // Filter Data By Field
            getProducts = getProducts.FilterQuery(filterDto);

            // Sort Data By Field
            getProducts = getProducts.SortQuery(sortDto);

            // Check Pagination
            var ordersPaginationResult = await _httpContext.InsertPaginationParametersInResponse(
                getProducts, paginationDto.RecordsPerPage, paginationDto.Page);

            // Add Pagination
            var addPagination = await getProducts.Paginate(paginationDto).ToListAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<List<ProductDto>>(addPagination);

            _logger.Debug("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return (dto, ordersPaginationResult);
        }
    }
}
