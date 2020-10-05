using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.MapperProfilers
{
    public class UserProfiler : Profile
    {
        public UserProfiler()
        {
            CreateMap<RegisterDto, User>();
        }
    }
}
