using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using NuGet.Protocol.Core.Types;
using Serilog;
using Net60_ApiTemplate_20231.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public class ProductGroupServices : ServiceBase, IProductGroupServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpContext? _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;

        public ProductGroupServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext.HttpContext;
            _loginDetailServices = loginDetailServices;
        }

        public async Task<ProductGroupDto> CreateProductGroup(CreateProductGroupDto createProductGroupDto)
        {
            const string _serviceName = nameof(CreateProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);
            var productgroup = _mapper.Map<ProductGroup>(createProductGroupDto);

            var userId = _loginDetailServices.GetClaim().UserId;
            productgroup.ProductGrouptId = Guid.NewGuid();
            productgroup.ProductGroupName = createProductGroupDto.ProductGroupName == null ? "DefaultName" : createProductGroupDto.ProductGroupName;
            productgroup.CreatedDate = DateTime.Now;
            productgroup.UpdatedDate = DateTime.Now;
            productgroup.UpdatedByUserId = userId;
            productgroup.CreatedByUserId = userId;
            productgroup.isActive = true;


            _dbContext.Add(productgroup);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<ProductGroupDto>(productgroup);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<UpdateProductGroupResponseDto> UpdateProductGroup(Guid productGroupId, UpdateProductGroupDto updateProductGroupDto)
        {
            const string _serviceName = nameof(UpdateProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var productGroup = _mapper.Map<ProductGroup>(updateProductGroupDto);
            var getProductGroup = _dbContext.ProductGroups.FirstOrDefault(f => f.ProductGrouptId == productGroupId);

            getProductGroup.ProductGroupName = productGroup.ProductGroupName;
            getProductGroup.UpdatedDate = DateTime.Now;
            getProductGroup.UpdatedByUserId = userId;
            getProductGroup.isActive = productGroup.isActive;


            _dbContext.Update(getProductGroup);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<UpdateProductGroupResponseDto>(getProductGroup);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<DeleteProductGroupResponseDto> DeleteProductGroup(Guid productGroupId)
        {
            const string _serviceName = nameof(DeleteProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var userId = _loginDetailServices.GetClaim().UserId;

            var getProductGroup = _dbContext.ProductGroups.FirstOrDefault(f => f.ProductGrouptId == productGroupId);

            getProductGroup.UpdatedDate = DateTime.Now;
            getProductGroup.UpdatedByUserId = userId;
            getProductGroup.isActive = false;

            _dbContext.Update(getProductGroup);
            await _dbContext.SaveChangesAsync();

            var dto = _mapper.Map<DeleteProductGroupResponseDto>(getProductGroup);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }

        public async Task<(List<ProductGroupDto> productGroupDtos , PaginationResultDto pagination)> GetAllProductGroup(FilterDataDto filterDataDto)
        {

            const string _serviceName = nameof(GetAllProductGroup);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);
            var paginationDto = _mapper.Map<PaginationDto>(filterDataDto);
            var filterDto = _mapper.Map<QueryFilterDto>(filterDataDto);
            var sortDto = _mapper.Map<QuerySortDto>(filterDataDto);

            var getProductGroups = _dbContext.ProductGroups.AsQueryable();

            // Flag IsActive
            if (filterDataDto.isActive != null)
            {
                getProductGroups.Where(w => w.isActive == filterDataDto.isActive);
            }

            // Filter Data By Field
            getProductGroups.FilterQuery(filterDto);

            /// Sort Data By Field
            getProductGroups.SortQuery(sortDto);

            // Check Pagination
            var ordersPaginationResult = await _httpContext.InsertPaginationParametersInResponse(getProductGroups, paginationDto.RecordsPerPage, paginationDto.Page);

            // Add Pagination
            var addPagination = await getProductGroups.Paginate(paginationDto).ToListAsync();


            var dto = _mapper.Map<List<ProductGroupDto>>(addPagination);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return (dto, ordersPaginationResult);
        }

        public async Task<ProductGroupDto> GetProductGroupById(Guid productGroupId)
        {
            const string _serviceName = nameof(GetProductGroupById);

            Log.Debug("[{_serviceName}] - Started: {date}", _serviceName, DateTime.Now);

            var getProductGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(f => f.ProductGrouptId == productGroupId);

            var dto = _mapper.Map<ProductGroupDto>(getProductGroup);

            Log.Debug("[{_serviceName}] - Sussess: {date}", _serviceName, DateTime.Now);

            return dto;
        }


    }
}
