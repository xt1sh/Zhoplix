﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.Models.Media;
using Zhoplix.ViewModels.Movie;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Services.CRUD
{
    public interface IMovieService
    {
        Task<bool> CreateMovieAsync(Movie model);
        Task<Movie> CreateMovieFromCreateViewModelAsync(CreateMovieViewModel model);
    }

    public class MovieService : IMovieService
    {
        private readonly ITitleService _titleService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly DbSet<Movie> _movieContext;
        private readonly DbSet<MovieVideo> _movieVideoContext;

        public MovieService(ITitleService titleService,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _titleService = titleService;
            _context = context;
            _mapper = mapper;
            _movieContext = _context.Movies;
            _movieVideoContext = _context.MovieVideos;
        }

        public async Task<Movie> CreateMovieFromCreateViewModelAsync(CreateMovieViewModel model)
        {
            var newTitle = _mapper.Map<CreateTitleViewModel>(model);
            var title = await _titleService.CreateTitleFromCreateViewModelAsync(newTitle);

            if (title == null)
                return null;

            var videoInfo = new VideoInfo();

            var videos = model.VideoPaths.Select(path => new MovieVideo()
            {
                Id = Path.GetFileNameWithoutExtension(path),
                Location = path,
                VideoInfo = videoInfo
            }).ToList();

            var newMovie = new Movie
            {
                TitleId = title.Id
            };

            newMovie.Videos.Select(x => x.VideoInfo = new VideoInfo());
            newMovie.Videos = videos;
            newMovie.Location = Path.GetDirectoryName(videos.First().Location);
            newMovie.ThumbnailsAmount = Directory.GetFiles(Path.Combine(newMovie.Location, "Thumbnails"), "*", SearchOption.TopDirectoryOnly).Length;

            if (await CreateMovieAsync(newMovie))
                return newMovie;

            return null;
        }

        public async Task<bool> CreateMovieAsync(Movie model)
        {
            await _movieContext.AddAsync(model);
            return await SaveChangesAsync();
        }

        private async Task<bool> SaveChangesAsync() =>
            await _context.SaveChangesAsync() > 0;
    }
}