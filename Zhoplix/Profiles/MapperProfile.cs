﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.Models.Identity;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Episode;
using Zhoplix.ViewModels.Media;
using Zhoplix.ViewModels.Movie;
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
            CreateMap<CreateMovieViewModel, Movie>();
            CreateMap<CreateMovieViewModel, CreateTitleViewModel>();

            // ViewModels
            CreateMap<RegistrationViewModel, User>();
            CreateMap<Title, TitleViewModel>();
            CreateMap<Title, TitlePageViewModel>();
            CreateMap<Season, SeasonViewModel>();
            CreateMap<Season, SeasonIdName>();
            CreateMap<Episode, EpisodeViewModel>();
            CreateMap<Episode, EpisodeForPlayerViewModel>();
            CreateMap<EpisodeVideo, VideoForPlayerViewModel>();

            // ChangeViewModels

        }
    }
}
