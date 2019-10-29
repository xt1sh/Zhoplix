using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models.Identity
{
    public class Session
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RefreshToken { get; set; }
        public string Fingerprint { get; set; }
        public string IP { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }

    }
}
