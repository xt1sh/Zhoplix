using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Title, CreateTitleViewModel>().ReverseMap();
            CreateMap<Title, TitleViewModel>().ReverseMap();
        }
    }
}
