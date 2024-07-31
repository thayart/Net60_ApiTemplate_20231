using AutoMapper;
using Net60_ApiTemplate_20231.DTOs;
using Net60_ApiTemplate_20231.DTOs.Orders;
using Net60_ApiTemplate_20231.DTOs.Products;
using Net60_ApiTemplate_20231.DTOs.Hospital;
using Net60_ApiTemplate_20231.Models;
using static Net60_ApiTemplate_20231.DTOs.Hospital.HospitalResultDto;

namespace Net60_ApiTemplate_20231
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /*
             * CreateMap<SampleMessage, ExampleModels>()
             *     .ForMember(_ => _.ExampleName, _ => _.MapFrom(_ => _.Name))
             *     .ReverseMap();
             * 
             * CreateMap<ExampleModels, GetExampleReponseDto>();
             */


            CreateMap<FilterDataDto, PaginationDto>();
            CreateMap<FilterDataDto, QueryFilterDto>();
            CreateMap<FilterDataDto, QuerySortDto>();


            #region Product Group
            // /// 
            // /// Create Set
            // /// 

            CreateMap<ProductGroupDto, ProductGroup>().ReverseMap();

            CreateMap<CreateProductGroupDto, ProductGroup>();

            // /// 
            // /// Create Update Set
            // /// 
            CreateMap<UpdateProductGroupDto, ProductGroup>();

            CreateMap<ProductGroup, UpdateProductGroupResponseDto>();

            // /// 
            // /// Create Get Set
            // /// 
            CreateMap<DeleteProductGroupDto, ProductGroup>();
           

            // /// 
            // /// Create Delete Set
            // /// 
            CreateMap<ProductGroup, DeleteProductGroupResponseDto>();

            #endregion

            #region Product

            // /// 
            // /// Create Set
            // /// 

            CreateMap<ProductRequestDto, Product>();

            CreateMap<Product, ProductResponseDto>();

            // /// 
            // /// Update Set
            // /// 
            CreateMap<UpdateProductRequestDto, Product>();

            CreateMap<Product, UpdateProductResponseDto>();

            // /// 
            // /// Create Get Set
            // /// 
            CreateMap<Product, ProductDto>();

            // /// 
            // /// Create Delete Set
            // /// 
            CreateMap<Product, DeleteProductResponseDto>();



            #endregion

            #region Order

            // /// 
            // /// Create Set
            // /// 
            CreateMap<OrderRequestDto, Order>();

            CreateMap<OrderDetailsRequestDto, OrderDetail>();

            CreateMap<Order, OrderResponseDto>();

            CreateMap<OrderDetail, OrderDetailsResponseDto>();

            CreateMap<ProductDto, OrderDetailsResponseDto>();


            // /// 
            // /// Delete Set
            // /// 
            CreateMap<Order, DeleteOrderResponseDto>();

            // /// 
            // /// Delete Set
            // /// 
            CreateMap<Order , OrderDto>();
            #endregion

            #region Hospital
            // /// 
            // /// Create Set
            // /// 

            #endregion

        }
    }
}