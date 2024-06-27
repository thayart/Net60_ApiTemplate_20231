using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Products;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public interface IProductGroupServices
    {
        Task<ProductGroupDto> CreateProductGroup(CreateProductGroupDto createProductGroupDto);
        Task<DeleteProductGroupResponseDto> DeleteProductGroup(Guid productGroupId);
        Task<(List<ProductGroupDto> productGroupDtos, PaginationResultDto pagination)> GetAllProductGroup(
            PaginationDto paginationDto
            , QueryFilterDto filterDto
            , QuerySortDto sortDto
            );
        Task<ProductGroupDto> GetProductGroupById(Guid productGroupId);
        Task<UpdateProductGroupResponseDto> UpdateProductGroup(Guid productGroupId, UpdateProductGroupDto updateProductGroupDto);
    }
}