using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Vigilance.DTOs;
using Vigilance.Models;

namespace Vigilance.App_Start {
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<WorkSite, WorkSiteDTO>();
            Mapper.CreateMap<WorkSiteDTO, WorkSite>();
        }
    }
}