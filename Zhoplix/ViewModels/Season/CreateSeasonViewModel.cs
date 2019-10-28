using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.ViewModels.Episode;

namespace Zhoplix.ViewModels.Season
{
    public class CreateSeasonViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageId { get; set; }
        public int TitleId { get; set; }
    }
}
