﻿using AutoMapper;
using WorkMateBE.Dtos.AccountDto;
using WorkMateBE.Dtos.DepartmentDto;
using WorkMateBE.Dtos.EmployeeDto;
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
        }
    }
}
