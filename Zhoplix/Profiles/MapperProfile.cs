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
            CreateMap<RegistrationViewModel, User>().ReverseMap();
            CreateMap<CreateTitleViewModel, Title>()
                .ForMember(dest => dest.Seasons, opt => opt.Ignore()).ReverseMap();
            CreateMap<CreateSeasonViewModel, Season>()
                .ForMember(dest => dest.Episodes, opt => opt.Ignore()).ReverseMap();
            CreateMap<CreateEpisodeViewModel, Episode>().ReverseMap();
            CreateMap<List<CreateSeasonViewModel>, List<Season>>().ReverseMap();
            CreateMap<List<CreateEpisodeViewModel>, List<Episode>>().ReverseMap();

            // ViewModels
            CreateMap<Title, ViewModels.TitleViewModel>().ReverseMap();

            // ChangeViewModels

        }
    }
}
