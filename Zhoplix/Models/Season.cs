using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Season
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageId { get; set; }
        public string ImageLocation { get; set; }
        public int TitleId { get; set; }
        public Title Title { get; set; }
        public IList<Episode> Episodes { get; set; }
    }
}
