using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Title
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Season> Seasons { get; set; }
        [Required]
        public string Description { get; set; }
        [Range(0, 10)]
        public float Rating { get; set; }
        [Range(0, 18)]
        public int AgeRestriction { get; set; }
    }
}
