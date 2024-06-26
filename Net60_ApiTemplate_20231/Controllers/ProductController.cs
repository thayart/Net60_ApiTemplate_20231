using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Product;
using Serilog;

namespace Net60_ApiTemplate_20231.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<ServiceResponse<ProductResponseDto>> CreateProduct([FromBody] ProductRequestDto productRequestDto)
        {
            const string _serviceName = nameof(CreateProduct);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productServices.CreateProduct(productRequestDto);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }

        [HttpPost("{productId}")]
        public async Task<ServiceResponse<UpdateProductResponseDto>> UpdateProduct(Guid productId , UpdateProductRequestDto updateProductRequestDto)
        {
            const string _serviceName = nameof(UpdateProduct);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productServices.UpdateProduct(productId, updateProductRequestDto);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);

        }


        [HttpPost("{productId}/delete")]
        public async Task<ServiceResponse<DeleteProductResponseDto>> DeleteProduct(Guid productId)
        {
            const string _serviceName = nameof(DeleteProduct);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productServices.DeleteProduct(productId);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);

        }

        [HttpGet("{productId}")]
        public async Task<ServiceResponse<ProductDto>> GetProductById (Guid productId)
        {
            const string _serviceName = nameof(GetProductById);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await  _productServices.GetProductById(productId);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }
    }
}
