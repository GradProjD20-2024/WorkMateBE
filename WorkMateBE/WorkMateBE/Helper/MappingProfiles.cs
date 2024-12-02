using AutoMapper;
using WorkMateBE.Dtos.AccountDto;
using WorkMateBE.Dtos.AssetDto;
using WorkMateBE.Dtos.CommentDto;
using WorkMateBE.Dtos.DepartmentDto;
using WorkMateBE.Dtos.EmployeeDto;
using WorkMateBE.Dtos.PostDto;
using WorkMateBE.Dtos.RoleDto;
using WorkMateBE.Models;

namespace WorkMateBE.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Department, DepartmentCreateDto>().ReverseMap();
            CreateMap<Department, DepartmentGetDto>().ReverseMap();
            CreateMap<Employee, EmployeeCreateDto>().ReverseMap();
            CreateMap<Employee, EmployeeGetDto>().ReverseMap();
            CreateMap<Account, AccountCreateDto>().ReverseMap();
            CreateMap<Account, AccountGetDto>().ReverseMap();
            CreateMap<Asset, AssetCreateDto>().ReverseMap();
            CreateMap<Asset, AssetGetDto>().ReverseMap();
            CreateMap<Role, NewRoleDto>().ReverseMap();
            CreateMap<Post, PostCreateDto>().ReverseMap();
            CreateMap<Post, PostUpdateDto>().ReverseMap();
            CreateMap<Post, PostGetDto>().ReverseMap();
            CreateMap<Comment, GetCommentDto>().ReverseMap();

        }
    }
}
