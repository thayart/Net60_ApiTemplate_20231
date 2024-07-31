using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Product;
using Serilog;

namespace Net60_ApiTemplate_20231.Controllers
{
    /// <summary>
    /// Product Controller
    /// </summary>
    [Route("api/product")]
    [ApiController]
   // [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        private readonly Serilog.ILogger _logger;

        public ProductController(IProductServices productServices, Serilog.ILogger? logger = null)
        {
            _productServices = productServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductController)): logger.ForContext("ServiceName", nameof(ProductController));
        }

        /// <summary>
        ///     CreateProduct
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost(Name = "CreateProduct")]
        public async Task<ServiceResponse<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto productRequestDto)
        {
            const string actionName = nameof(CreateProduct);
            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            ProductResponseDto result = await _productServices.CreateProduct(productRequestDto);

            _logger.Information("[{actionName}] - Sussess: {date} - Id: {ProductId}", actionName, DateTime.Now , result.ProductId);

            return ResponseResult.Success(result);
        }

        /// <summary>
        ///     UpdateProduct
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost("{productId}" , Name = "UpdateProduct")]
        public async Task<ServiceResponse<UpdateProductResponseDto>> UpdateProduct(Guid productId , UpdateProductRequestDto updateProductRequestDto)
        {
            const string actionName = nameof(UpdateProduct);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            UpdateProductResponseDto result = await _productServices.UpdateProduct(productId, updateProductRequestDto);


            _logger.Information("[{actionName}] - Sussess: {date} - Id: {ProductId}", actionName, DateTime.Now, productId);

            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     DeleteProduct
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost("{productId}/delete" , Name = "DeleteProduct")]
        public async Task<ServiceResponse<DeleteProductResponseDto>> DeleteProduct(Guid productId)
        {
            const string actionName = nameof(DeleteProduct);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            DeleteProductResponseDto result = await _productServices.DeleteProduct(productId);


            _logger.Information("[{actionName}] - Sussess: {date} - Id: {ProductId}", actionName, DateTime.Now, productId);

            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     GetProductById
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("{productId}" ,Name = "GetProductById")]
        public async Task<ServiceResponse<ProductDto>> GetProductById (Guid productId)
        {
            const string actionName = nameof(GetProductById);

            _logger.Debug("[{actionName}] - Started: {date}- Id: {ProductId}", actionName, DateTime.Now, productId);

            ProductDto result = await  _productServices.GetProductById(productId);


            _logger.Information("[{actionName}] - Sussess: {date} - Id: {ProductId}", actionName, DateTime.Now, productId);

            return ResponseResult.Success(result);
        }

        /// <summary>
        ///     GetAllProduct
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("GetAllProduct" ,Name = "GetAllProduct")]
        public async Task<ServiceResponse<(List<ProductDto> productDtos, PaginationResultDto pagination)>> GetAllProduct(
           [FromQuery] PaginationDto paginationDto
            , [FromQuery] QueryFilterDto filterDto
            , [FromQuery] QuerySortDto sortDto
            )
        {
            const string actionName = nameof(GetProductById);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var result = await _productServices.GetAllProduct(paginationDto,filterDto,sortDto);

            _logger.Information("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return ResponseResult.Success(result);
        }
    }
}
