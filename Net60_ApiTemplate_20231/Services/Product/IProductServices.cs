using Microsoft.AspNetCore.Mvc;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.DTOs.Products;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public interface IProductServices
    {
        Task<ProductResponseDto> CreateProduct(ProductRequestDto productRequestDto);
        Task<UpdateProductResponseDto> UpdateProduct(Guid ProductId, UpdateProductRequestDto updateProductRequestDto);
        Task<DeleteProductResponseDto> DeleteProduct(Guid productId);
        Task<ProductDto> GetProductById(Guid productId);
        Task<(List<ProductDto> productDtos, PaginationResultDto pagination)> GetAllProduct(
            [FromQuery] PaginationDto paginationDto
            , [FromQuery] QueryFilterDto filterDto
            , [FromQuery] QuerySortDto sortDto
            );
    }
}
