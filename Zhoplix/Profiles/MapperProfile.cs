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
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            // CreateViewModels
            CreateMap<CreateTitleViewModel, Title>().ForMember(x => x.Genres, t => t.Ignore());
            CreateMap<CreateSeasonViewModel, Season>();
            CreateMap<CreateEpisodeViewModel, Episode>();
            CreateMap<CreateGenreViewModel, Genre>().ReverseMap();

            // ViewModels
            CreateMap<RegistrationViewModel, User>();
            CreateMap<Title, TitleViewModel>();
            CreateMap<Title, TitlePageViewModel>();
            CreateMap<Season, SeasonViewModel>();

            // ChangeViewModels

        }
    }
}
