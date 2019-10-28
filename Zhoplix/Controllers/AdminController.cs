using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zhoplix.Models;
using Zhoplix.Models.Media;
using Zhoplix.Services;
using Zhoplix.Services.CRUD;
using Zhoplix.Services.Media;
using Zhoplix.ViewModels;
using Zhoplix.ViewModels.Episode;
using Zhoplix.ViewModels.Season;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ITitleService _titleService;
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Title> _titleContext;
        private readonly DbSet<Season> _seasonContext;
        private readonly DbSet<Episode> _episodeContext;
        private readonly DbSet<Genre> _genreContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IMediaService _mediaService;
        private readonly IFfMpegProvider _ffMpeg;

        public AdminController(ITitleService titleService,
            ApplicationDbContext context,
            IMapper mapper,
            ILogger<AdminController> logger,
            IMediaService mediaService,
            IFfMpegProvider ffMpeg)
        {
            _titleService = titleService;
            _context = context;
            _titleContext = _context.Titles;
            _seasonContext = _context.Seasons;
            _episodeContext = _context.Episodes;
            _genreContext = _context.Genres;
            _mapper = mapper;
            _logger = logger;
            _mediaService = mediaService;
            _ffMpeg = ffMpeg;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitle(CreateTitleViewModel model)
        {
            var title = await _titleService.CreateTitleFromCreateViewModelAsync(model);
            if (title != null)
                return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Title/{title.Id}", _mapper.Map<TitleViewModel>(title));

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeason(CreateSeasonViewModel model)
        {
            var season = _mapper.Map<Season>(model);
            await _seasonContext.AddAsync(season);
            await _context.SaveChangesAsync();
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Season/{season.Id}", _mapper.Map<SeasonViewModel>(season));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode(CreateEpisodeViewModel model)
        {
            var episode = _mapper.Map<Episode>(model);
            await _episodeContext.AddAsync(episode);
            await _context.SaveChangesAsync();
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Episode/{episode.Id}", _mapper.Map<SeasonViewModel>(episode));
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadVideo()
        {
            var file = Request.Form.Files[0];
            var id = Guid.NewGuid().ToString();
            if (await _mediaService.UploadVideo(file, id))
                return Ok(new { id });

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(UploadPhoto photo)
        {
            photo.PhotoId = Guid.NewGuid().ToString();
            await _mediaService.CreatePhoto(photo);
            //await _mediaService.CreateResizedPhoto(photo, 0.1f, "small");
            //await _mediaService.CreateResizedPhoto(photo, 0.5f, "medium");
            return Ok(new { photo.PhotoId });
        }

        [HttpDelete]
        public IActionResult DeletePhoto(DeletePhoto photo)
        {
            _mediaService.DeletePhoto(photo.Name);
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteAllPhotos(DeleteAllPhotos id)
        {
            _mediaService.DeleteAllPhotosWithId(id.Id);
            return Ok();
        }

        public async Task<IActionResult> CreateThumbnails()
        {
            await Task.Run(() => 
            {
                _ffMpeg.ResizeVideo("C:\\Code\\ASP.NET\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\ElCamino.mp4", 120);
                _ffMpeg.CreateThumbnails("C:\\Code\\ASP.NET\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\ElCamino_120.mp4",
                    "C:\\Code\\ASP.NET\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\Thumbnails");
            });
            return Ok();
        }
    }
}
