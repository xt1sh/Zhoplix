using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zhoplix.Models.Identity;

namespace Zhoplix.Services.ProfileManager
{
    public interface IProfileManager
    {
        Task<Profile> CreateProfileAsync(int userId);
    }
}
