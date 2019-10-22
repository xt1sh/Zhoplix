using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zhoplix.Models;
using Zhoplix.Models.Media;
using Zhoplix.Services;
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
        private readonly IRepository<Title> _titleContext;
        private readonly IRepository<Season> _seasonContext;
        private readonly IRepository<Episode> _episodeContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;
        private readonly IMediaService _mediaService;
        private readonly IFfMpegProvider _ffMpeg;

        public AdminController(IRepository<Title> titleContext,
            IRepository<Season> seasonContext,
            IRepository<Episode> episodeContext,
            IMapper mapper,
            ILogger<AdminController> logger,
            IMediaService mediaService,
            IFfMpegProvider ffMpeg)
        {
            _titleContext = titleContext;
            _seasonContext = seasonContext;
            _episodeContext = episodeContext;
            _mapper = mapper;
            _logger = logger;
            _mediaService = mediaService;
            _ffMpeg = ffMpeg;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitle(CreateTitleViewModel model)
        {
            var title = _mapper.Map<Title>(model);
            await _titleContext.AddObjectAsync(title);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Title/{title.Id}", _mapper.Map<TitleViewModel>(title));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeason(CreateSeasonViewModel model)
        {
            var season = _mapper.Map<Season>(model);
            await _seasonContext.AddObjectAsync(season);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Season/{season.Id}", _mapper.Map<SeasonViewModel>(season));
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode(CreateEpisodeViewModel model)
        {
            var episode = _mapper.Map<Episode>(model);
            await _episodeContext.AddObjectAsync(episode);
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
            await _mediaService.CreateResizedPhoto(photo, 0.1f, "small");
            await _mediaService.CreateResizedPhoto(photo, 0.5f, "medium");
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

        [HttpPost]
        public async Task<IActionResult> GetTitlesPage(Page page)
        {
            var titles = await _titleContext.GetObjectsByPageAsync(page.PageNumber, page.PageSize);
            var toShow = _mapper.Map<IEnumerable<TitleViewModel>>(titles);
            return Ok(toShow);
        }

        public async Task<IActionResult> CreateThumbnails()
        {
            await Task.Run(() => 
            {
                _ffMpeg.ResizeVideo("C:\\Coding\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\ElCamino.mp4", 120);
                _ffMpeg.CreateThumbnails("C:\\Coding\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\ElCamino_120.mp4",
                    "C:\\Coding\\Zhoplix\\Zhoplix\\wwwroot\\Videos\\Uploaded\\ElCamino\\Thumbnails");
            });
            return Ok();
        }
    }
}
