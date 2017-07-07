using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TimeTidy.DTOs;
using TimeTidy.Models;

namespace TimeTidy.App_Start {
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<WorkSite, WorkSiteDTO>();
            CreateMap<WorkSiteDTO, WorkSite>();
        }
    }
}