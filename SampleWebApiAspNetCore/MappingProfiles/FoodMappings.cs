using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class FoodMappings : Profile
    {
        public FoodMappings()
        {
            CreateMap<EmployeeEntity, FoodDto>().ReverseMap();
            CreateMap<EmployeeEntity, FoodUpdateDto>().ReverseMap();
            CreateMap<EmployeeEntity, FoodCreateDto>().ReverseMap();
        }
    }
}
