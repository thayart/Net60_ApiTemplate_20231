using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Net60_ApiTemplate_20231.Data;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.Models;
using Net60_ApiTemplate_20231.Services.Auth;
using Serilog;
using Net60_ApiTemplate_20231.Helpers;
using Net60_ApiTemplate_20231.Exceptions;
using System.Linq.Dynamic.Core;

namespace Net60_ApiTemplate_20231.Services.Product
{
    public class ProductGroupServices : ServiceBase, IProductGroupServices
    {
        private readonly AppDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpContext? _httpContext;
        private readonly ILoginDetailServices _loginDetailServices;
        private readonly Serilog.ILogger _logger;
        
        public ProductGroupServices(AppDBContext dbContext, IMapper mapper, IHttpContextAccessor httpContext, ILoginDetailServices loginDetailServices , Serilog.ILogger? logger = null) : base(dbContext, mapper, httpContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContext = httpContext.HttpContext;
            _loginDetailServices = loginDetailServices;
            _logger = logger is null ? Log.ForContext("ServiceName", nameof(ProductServices)) : logger.ForContext("ServiceName", nameof(ProductServices));
        }

        public async Task<ProductGroupDto> CreateProductGroup(CreateProductGroupDto createProductGroupDto)
        {
            const string actionName = nameof(CreateProductGroup);
            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            var user = _loginDetailServices.GetClaim();

            // Setup CreateData
            ProductGroup productgroup = _mapper.Map<ProductGroup>(createProductGroupDto);

            productgroup.ProductGrouptId = Guid.NewGuid();
            productgroup.ProductGroupName = createProductGroupDto.ProductGroupName;
            productgroup.CreatedDate = DateTime.Now;
            productgroup.UpdatedDate = DateTime.Now;
            productgroup.UpdatedByUserId = user.UserId;
            productgroup.CreatedByUserId = user.UserId;
            productgroup.isActive = true;

            _dbContext.Add(productgroup);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            ProductGroupDto dto = _mapper.Map<ProductGroupDto>(productgroup);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductGroupId} ", actionName, DateTime.Now, dto.ProductGrouptId);

            return dto;
        }

        public async Task<UpdateProductGroupResponseDto> UpdateProductGroup(Guid productGroupId, UpdateProductGroupDto updateProductGroupDto)
        {
            const string actionName = nameof(UpdateProductGroup);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductGroupId}", actionName, DateTime.Now, productGroupId);

            var user = _loginDetailServices.GetClaim();

            // Setup UpdateData
            //if (productGroup == null) 
            //    throw new NotFoundException($"Product group {productGroupId} not found");

            var productGroup = _dbContext.ProductGroups.FirstOrDefault(f => f.ProductGrouptId == productGroupId);
            productGroup = _mapper.Map(updateProductGroupDto, productGroup);
            productGroup.UpdatedDate = DateTime.Now;
            productGroup.UpdatedByUserId = user.UserId;

            _dbContext.Update(productGroup);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<UpdateProductGroupResponseDto>(productGroup);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductGroupId}", actionName, DateTime.Now , productGroupId);

            return dto;
        }

        public async Task<DeleteProductGroupResponseDto> DeleteProductGroup(Guid productGroupId)
        {
            const string actionName = nameof(DeleteProductGroup);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductGroupId}", actionName, DateTime.Now, productGroupId);

            var user = _loginDetailServices.GetClaim();

            // Setup DeleteData
            var getProductGroup = _dbContext.ProductGroups.FirstOrDefault(f => f.ProductGrouptId == productGroupId);

            getProductGroup.UpdatedDate = DateTime.Now;
            getProductGroup.UpdatedByUserId = user.UserId;
            getProductGroup.isActive = false;

            _dbContext.Update(getProductGroup);
            await _dbContext.SaveChangesAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<DeleteProductGroupResponseDto>(getProductGroup);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductGroupId}", actionName, DateTime.Now, productGroupId);

            return dto;
        }

        public async Task<(List<ProductGroupDto> productGroupDtos, PaginationResultDto pagination)> GetAllProductGroup(
            PaginationDto paginationDto
            , QueryFilterDto filterDto
            , QuerySortDto sortDto
            )
        {
            const string actionName = nameof(GetAllProductGroup);

            _logger.Debug("[{actionName}] - Started: {date}", actionName, DateTime.Now);

            // Setup GetListData
            var getProductGroups = _dbContext.ProductGroups.AsQueryable();

            // Flag IsActive
            if (paginationDto.isActive != null)
            {
                getProductGroups = getProductGroups.Where(w => w.isActive == paginationDto.isActive);
            }

            // Filter Data By Field
            // getProductGroups = getProductGroups.WhereInterpolated($"{} ");
            getProductGroups = getProductGroups.FilterQuery(filterDto);


            // Sort Data By Field
            //getProductGroups = getProductGroups.OrderBy($"{sortDto.SortColumn} {sortDto.Ordering}");
            getProductGroups = getProductGroups.SortQuery(sortDto);

            
            // Check Pagination
            var ordersPaginationResult = await _httpContext.InsertPaginationParametersInResponse(
                getProductGroups, paginationDto.RecordsPerPage, paginationDto.Page
                );

            // Add Pagination
            var addPagination = await getProductGroups.Paginate(paginationDto).ToListAsync();

            // Return Data MaptoDto
            var dto = _mapper.Map<List<ProductGroupDto>>(addPagination);

            _logger.Debug("[{actionName}] - Sussess: {date}", actionName, DateTime.Now);

            return (dto, ordersPaginationResult);
        }

        public async Task<ProductGroupDto> GetProductGroupById(Guid productGroupId)
        {
            const string actionName = nameof(GetProductGroupById);

            _logger.Debug("[{actionName}] - Started: {date} - Id:{ProductGroupId}", actionName, DateTime.Now, productGroupId);
            
            // Setup GetData
            var getProductGroup = await _dbContext.ProductGroups.FirstOrDefaultAsync(f => f.ProductGrouptId == productGroupId);


            // Return Data MaptoDto
            var dto = _mapper.Map<ProductGroupDto>(getProductGroup);

            _logger.Debug("[{actionName}] - Sussess: {date} - Id:{ProductGroupId}", actionName, DateTime.Now, productGroupId);

            return dto;
        }


    }
}
