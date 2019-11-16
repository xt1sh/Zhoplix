using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zhoplix.Models;
using Zhoplix.Services.CRUD;
using Zhoplix.ViewModels.Episode;
using Zhoplix.ViewModels.Media;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class EpisodeController : ControllerBase
    {
        private readonly IEpisodeService _episodeService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public EpisodeController(IEpisodeService episodeService,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _episodeService = episodeService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTitleById(int id)
        {
            var episode = await _episodeService.GetEpisodeAsync(id);
            var videos = await _context.Video.Where<Video>(x => x.EpisodeId == episode.Id).ToListAsync();
            episode.Videos = videos;
            if (episode == null)
                return BadRequest();

            var toShow = _mapper.Map<EpisodeForPlayerViewModel>(episode);

            toShow.Videos = episode.Videos.Select(x => _mapper.Map<VideoForPlayerViewModel>(x)).ToList();
            for(var i = 0; i < toShow.Videos.Count; i++)
            {
                toShow.Videos[i].Location = Path.Combine("Videos", "Uploaded", Path.GetFileNameWithoutExtension(toShow.Videos[0].Location), Path.GetFileName(toShow.Videos[i].Location));
            }

            toShow.ThumbnailLocation = Path.Combine("Videos", "Uploaded", Path.GetFileNameWithoutExtension(toShow.Videos[0].Location), "Thumbnails");

            return Ok(toShow);
        }
    }
}