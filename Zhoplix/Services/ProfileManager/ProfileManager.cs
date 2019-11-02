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

            var imageId = _avatarGenerator.GenerateAvatar(12, 120);
            var imagePath = Path.Combine("Images", "Avatars", imageId, $"{imageId}.png"); // hardcode
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

        public async Task<Profile> GetProfileByIdAsync(int userId) =>
            await _profileContext.FirstOrDefaultAsync(p => p.Id == userId);
    }
}
