using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Episode;
using Zhoplix.ViewModels.Season;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // CreateViewModels
            CreateMap<CreateTitleViewModel, Title>();
            CreateMap<CreateSeasonViewModel, Season>();
            CreateMap<CreateEpisodeViewModel, Episode>();

            // ViewModels
            CreateMap<RegistrationViewModel, User>();
            CreateMap<Title, TitleViewModel>();

            // ChangeViewModels

        }
    }
}
