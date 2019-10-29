using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zhoplix.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SeasonId { get; set; }
        public Season Season { get; set; }
        public IList<UserEpisode> UserEpisodes { get; set; }
        public TimeSpan Duration { get; set; }
        public bool HasOpening => OpeningStart != null && OpeningFinish != null;
        public TimeSpan? OpeningStart { get; set; }
        public TimeSpan? OpeningFinish { get; set; }
        public int ThumbnailsAmount { get; set; }
        public TimeSpan? CreditsStart { get; set; }
        public IList<Video> Videos { get; set; }
        public IList<Subtitles> Subtitles { get; set; }
        public IList<Audio> Audios { get; set; }
    }
}
