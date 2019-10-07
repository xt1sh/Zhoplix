using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public AdminController(IRepository<Title> titleContext,
            IMapper mapper)
        {
            _titleContext = titleContext;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTitle(CreateTitleViewModel model)
        {
            var obj = _mapper.Map<Title>(model);
            await _titleContext.AddObjectAsync(obj);
            var toView = _mapper.Map<TitleViewModel>(obj);
            return Created($"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Title/{toView.Id}", obj);
        }
    }
}
