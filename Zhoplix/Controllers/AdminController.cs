using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
        private readonly ISeasonService _seasonService;
        private readonly IEpisodeService _episodeService;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IMediaService _mediaService;
        private readonly IFfMpegProvider _ffMpeg;

        public AdminController(ITitleService titleService,
            ISeasonService seasonService,
            IEpisodeService episodeService,
            IMapper mapper,
            ILogger<AdminController> logger,
            IMediaService mediaService,
            IFfMpegProvider ffMpeg)
        {
            _titleService = titleService;
            _seasonService = seasonService;
            _episodeService = episodeService;
            _mapper = mapper;
            _logger = logger;
            _mediaService = mediaService;
            _ffMpeg = ffMpeg;
        }

        [HttpGet("{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetTitlesPage(int pageNumber, int pageSize)
        {
            var titles = await _titleService.GetTitlePageAsync(pageNumber, pageSize);
            var toShow = titles.Select(x => _mapper.Map<TitlePageViewModel>(x));
            return Ok(toShow);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetTitle(string name)
        {
            var title = await _titleService.GetTitleByNameAsync(name);
            var toShow = _mapper.Map<TitlePageViewModel>(title);
            return Ok(toShow);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> FindTitles(string name)
        {
            var titles = await _titleService.FindTitlesAsync(name);
            var toShow = titles.Select(x => _mapper.Map<TitlePageViewModel>(x));
            return Ok(toShow);
        }

        [HttpGet("{titleId}")]
        public async Task<IActionResult> GetAllSeasonsOfTitle(int titleId)
        {
            var seasons = await _seasonService.GetAllSeasonsOfTitleAsync(titleId);

            if (seasons == null)
                return Ok(null);

            var toShow = seasons.Select(x => _mapper.Map<SeasonIdName>(x));

            return Ok(toShow);
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
            var season = await _seasonService.CreateSeasonFromCreateViewModelAsync(model);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Season/{season.Id}", _mapper.Map<SeasonViewModel>(season));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode(CreateEpisodeViewModel model)
        {
            var episode = await _episodeService.CreateEpisodeFromCreateViewModelAsync(model);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Episode/{episode.Id}", _mapper.Map<EpisodeViewModel>(episode));
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadVideo()
        {
            var file = Request.Form.Files[0];
            var id = Guid.NewGuid().ToString();
            var videoPath = Path.Combine(_mediaService.UploadVideosPath, id);
            var paths = new List<string>() { Path.Combine(videoPath, $"{id}.mp4") };
            if (!await _mediaService.UploadVideo(file, id))
                return BadRequest();

            var resizedVideoPath = await Task.FromResult(_ffMpeg.ResizeVideo(Path.Combine(videoPath, id + ".mp4"), 120));

            paths.Add(resizedVideoPath);

            await Task.Run(() => _ffMpeg.CreateThumbnails(Path.Combine(_mediaService.UploadVideosPath, id, resizedVideoPath)));

            return Ok(paths);
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
    }
}
