using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zhoplix.Models.Identity;
using Zhoplix.Services.Media;

namespace Zhoplix.Services.ProfileManager
{
    public class ProfileManager: IProfileManager
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Profile> _profileContext;
        private readonly IAvatarGenerator _avatarGenerator;

        public ProfileManager(ApplicationDbContext context, IAvatarGenerator avatarGenerator)
        {
            _context = context;
            _profileContext = context.Profiles;
            _avatarGenerator = avatarGenerator;
        }
        public async Task<Profile> CreateProfileAsync(int userId)
        {
            if (userId == -1)
                return null;

            var imageId = _avatarGenerator.GenerateAvatar(12, 120);
            var imagePath = Path.Combine(_avatarGenerator.ImagePath, imageId, $"{imageId}.png");
            var profile = new Profile
            {
                Id = userId,
                ImagePath = imagePath
            };
            _profileContext.Add(profile);

            if (await _context.SaveChangesAsync() > 0)
                return profile;

            return null;

        }
    }
}
