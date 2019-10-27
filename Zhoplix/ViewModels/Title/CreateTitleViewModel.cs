using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models;
using Zhoplix.ViewModels.Season;

namespace Zhoplix.ViewModels.Title
{
    public class CreateTitleViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<CreateGenreViewModel> Genres { get; set; }
        public int AgeRestriction { get; set; }
        public string ImageId { get; set; }
    }
}
