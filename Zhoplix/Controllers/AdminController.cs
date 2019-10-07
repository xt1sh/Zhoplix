using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zhoplix.Models;
using Zhoplix.Services;
using Zhoplix.ViewModels.Title;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRepository<Title> _titleContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IRepository<Title> titleContext,
            IMapper mapper,
            ILogger<AdminController> logger)
        {
            _titleContext = titleContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTitle(CreateTitleViewModel model)
        {
            var toWrite = _mapper.Map<Title>(model);

            toWrite.Seasons = model.Seasons.Select(season => _mapper.Map<Season>(season)).ToList();

            for (int i = 0; i < toWrite.Seasons.Count; i++)
                toWrite.Seasons[i].Episodes = model.Seasons[i].Episodes.Select(episode => _mapper.Map<Episode>(episode)).ToList();

            await _titleContext.AddObjectAsync(toWrite);
            var toView = _mapper.Map<TitleViewModel>(toWrite);
            _logger.LogInformation($"Created title \"{toWrite.Name}\" with id {toWrite.Id}");
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Title/{toView.Id}", toView);
        }
    }
}
