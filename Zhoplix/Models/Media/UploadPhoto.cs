using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class UploadPhoto
    {
        public int PhotoId { get; set; }
        public byte[] Photo { get; set; }
    }
}
