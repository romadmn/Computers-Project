using AutoMapper;
using ComputersApp.Application.DataTransferObjects;
using ComputersApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ComputersApp.Application.MapperProfilers
{
    public class CpuProfiler : Profile
    {
        public CpuProfiler()
        {
            CreateMap<CpuDto, Cpu>().ReverseMap();
        }
    }
}
