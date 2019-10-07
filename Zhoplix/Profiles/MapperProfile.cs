﻿using AutoMapper;
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
            // CreateViewModels
            CreateMap<CreateTitleViewModel, Title>()
                .ForMember(dest => dest.Seasons, opt => opt.Ignore()).ReverseMap();
            CreateMap<CreateSeasonViewModel, Season>()
                .ForMember(dest => dest.Episodes, opt => opt.Ignore()).ReverseMap();
            CreateMap<CreateEpisodeViewModel, Episode>().ReverseMap();
            CreateMap<List<CreateSeasonViewModel>, List<Season>>().ReverseMap();
            CreateMap<List<CreateEpisodeViewModel>, List<Episode>>().ReverseMap();

            // ViewModels
            CreateMap<RegistrationViewModel, User>().ReverseMap();
            CreateMap<Title, TitleViewModel>().ReverseMap();
            CreateMap<Title, CreateTitleViewModel>().ReverseMap();
            CreateMap<Title, TitleViewModel>().ReverseMap();
        }
    }
}
