using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Product;
using Net60_ApiTemplate_20231.DTOs.Products;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Net60_ApiTemplate_20231.DTOs;

namespace Net60_ApiTemplate_20231.Controllers
{

    /// <summary>
    /// ProductGroup Controller
    /// </summary>
    [Route("api/product-group")]
    [ApiController]
    public class ProductGroupController : ControllerBase
    {

        private readonly IProductGroupServices _productGroupServices;
        public ProductGroupController(IProductGroupServices productGroupServices)
        {
            _productGroupServices = productGroupServices;
        }

        [HttpPost]
        public async Task<ServiceResponse<ProductGroupDto>> CreateProductGroup([FromBody] CreateProductGroupDto creatProductGroupDto)
        {
            const string _serviceName = nameof(CreateProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productGroupServices.CreateProductGroup(creatProductGroupDto);

            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }


        [HttpPost("{productGroupId}")]
        public async Task<ServiceResponse<UpdateProductGroupResponseDto>> UpdateProductGroup(Guid productGroupId, [FromBody] UpdateProductGroupDto updateProductGroupDto)
        {
            const string _serviceName = nameof(UpdateProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productGroupServices.UpdateProductGroup(productGroupId, updateProductGroupDto);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);
            return ResponseResult.Success(result);

        }


        [HttpPost("{productGroupId}/Delete")]
        public async Task<ServiceResponse<DeleteProductGroupResponseDto>> DeleteProductGroup(Guid productGroupId)
        {
            const string _serviceName = nameof(DeleteProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productGroupServices.DeleteProductGroup(productGroupId);


            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);
            return ResponseResult.Success(result);

        }

        [HttpGet("{productGroupId}")]
        public async Task<ServiceResponse<ProductGroupDto>> GetProductGroupById(Guid productGroupId)
        {
            const string _serviceName = nameof(GetProductGroupById);
            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);


            var result = await _productGroupServices.GetProductGroupById(productGroupId);

            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);

        }


        [HttpGet("filter")]
        public async Task<ServiceResponse<(List<ProductGroupDto> productGroupDtos, PaginationResultDto pagination)>> GetAllProductGroups([FromQuery] FilterDataDto filterDataDto)
        {
            const string _serviceName = nameof(GetAllProductGroups);
            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var result = await _productGroupServices.GetAllProductGroup(filterDataDto);

            Log.Information("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return ResponseResult.Success(result);
        }
    }
}
