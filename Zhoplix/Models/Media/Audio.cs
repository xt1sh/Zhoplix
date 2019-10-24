using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Audio
    {
        public int Id { get; set; }
        public string Language { get; set; }
        public string Translation { get; set; }
        public Episode Episode { get; set; }
    }
}
