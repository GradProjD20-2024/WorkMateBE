using AutoMapper;
using WorkMateBE.Dtos.DepartmentDto;
using WorkMateBE.Models;

namespace WorkMateBE.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, DepartmentCreateDto>().ReverseMap();
            CreateMap<Department, DepartmentGetDto>().ReverseMap();
        }
    }
}
