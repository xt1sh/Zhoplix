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
        public Title Title { get; set; }
        public List<Episode> Episodes { get; set; }
    }
}
