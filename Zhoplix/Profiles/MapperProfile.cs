using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.ViewModels;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels.Title;
using TitleViewModel = Zhoplix.ViewModels.TitleViewModel;

namespace Zhoplix.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegistrationViewModel, User>().ReverseMap();
            CreateMap<Title, TitleViewModel>().ReverseMap();
            CreateMap<Title, CreateTitleViewModel>().ReverseMap();
        }
    }
}
