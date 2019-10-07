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
using Zhoplix.ViewModels;

namespace Zhoplix.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TitleController : ControllerBase
    {
        private readonly IRepository<Title> _repository;
        private readonly ILogger<TitleController> _logger;
        private readonly IMapper _mapper;

        public TitleController(IRepository<Title> repository,
            ILogger<TitleController> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTitle(TitleViewModel title)
        {
            var newTitle = _mapper.Map<Title>(title);
            await _repository.AddObjectAsync(newTitle);
            return Ok();
        }
    }
}