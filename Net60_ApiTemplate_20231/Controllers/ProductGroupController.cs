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
    //[Authorize]
    public class ProductGroupController : ControllerBase
    {
        private readonly IProductGroupServices _productGroupServices;
        private readonly Serilog.ILogger _logger;

        public ProductGroupController(IProductGroupServices productGroupServices, Serilog.ILogger? logger = null)
        {
            _productGroupServices = productGroupServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductGroupServices)) : logger.ForContext("ServiceName", nameof(ProductGroupServices));
        }

        /// <summary>
        ///     CreateProductGroup
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost(Name = "CreateProductGroup")]
        public async Task<ServiceResponse<ProductGroupDto>> CreateProductGroup([FromBody] CreateProductGroupDto creatProductGroupDto)
        {
            const string actionName = nameof(CreateProductGroup);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            ProductGroupDto result = await _productGroupServices.CreateProductGroup(creatProductGroupDto);

            _logger.Information("[{actionName}] - Sussess: {date} - Id:{ProductGrouptId}", actionName, DateTime.Now, result.ProductGrouptId);

            return ResponseResult.Success(result);
        }

        /// <summary>
        ///     UpdateProductGroup
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost("{productGroupId}", Name = "UpdateProductGroup")]
        public async Task<ServiceResponse<UpdateProductGroupResponseDto>> UpdateProductGroup(Guid productGroupId, [FromBody] UpdateProductGroupDto updateProductGroupDto)
        {
            const string actionName = nameof(UpdateProductGroup);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            UpdateProductGroupResponseDto result = await _productGroupServices.UpdateProductGroup(productGroupId, updateProductGroupDto);

            _logger.Information("[{actionName}] - Sussess: {date} - Id:{ProductGrouptId}", actionName, DateTime.Now , productGroupId);
            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     DeleteProductGroup
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpPost("{productGroupId}/delete", Name = "DeleteProductGroup")]
        public async Task<ServiceResponse<DeleteProductGroupResponseDto>> DeleteProductGroup(Guid productGroupId)
        {
            const string actionName = nameof(DeleteProductGroup);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            DeleteProductGroupResponseDto result = await _productGroupServices.DeleteProductGroup(productGroupId);


            _logger.Information("[{actionName}] - Sussess: {date} - Id:{ProductGrouptId}", actionName, DateTime.Now , productGroupId);
            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     GetProductGroupById
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("{productGroupId}", Name = "GetProductGroupById")]
        public async Task<ServiceResponse<ProductGroupDto>> GetProductGroupById(Guid productGroupId)
        {
            const string actionName = nameof(GetProductGroupById);
            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            ProductGroupDto result = await _productGroupServices.GetProductGroupById(productGroupId);

            _logger.Information("[{actionName}] - Sussess: {date} - Id:{ProductGrouptId}", actionName, DateTime.Now , productGroupId);

            return ResponseResult.Success(result);

        }

        /// <summary>
        ///     GetAllProductGroups
        /// </summary>
        /// <remarks>
        ///     No Remark
        /// </remarks>
        [HttpGet("filter", Name = "GetAllProductGroups")]
        public async Task<ServiceResponse<(List<ProductGroupDto> productGroupDtos, PaginationResultDto pagination)>> GetAllProductGroups(
            [FromQuery]   PaginationDto paginationDto 
            , [FromQuery] QueryFilterDto filterDto
            , [FromQuery] QuerySortDto sortDto
            )
        {
            const string actionName = nameof(GetAllProductGroups);
            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var result = await _productGroupServices.GetAllProductGroup(paginationDto,filterDto, sortDto);

            _logger.Information("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return ResponseResult.Success(result);
        }
    }
}
