using OSDBnet;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class TVShow
    {
        public int Id { get; set; }
        public string IMDbId { get; set; }
        public string Name { get; set; }
        public TMDbLib.Objects.TvShows.Credits Credits { get; set; }
        public TMDbLib.Objects.General.ResultContainer<TMDbLib.Objects.TvShows.ContentRating> ContentRatings { get; set; }
        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }

        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public List<TMDbLib.Objects.General.Genre> Genres { get; set; }
        public List<TMDbLib.Objects.Search.SearchTvSeason> Seasons { get; set; }
        public TMDbLib.Objects.General.ResultContainer<TMDbLib.Objects.TvShows.TvShow> Similar { get; set; }
        public TMDbLib.Objects.General.ResultContainer<TMDbLib.Objects.General.Video> Videos { get; set; }
        public double VoteAverage { get; set; }


        public long FBLikes { get; set; }
        public long FBShares { get; set; }


        public String Rated { get; set; }
        public String Language { get; set; }
        public String Country { get; set; }
        public String Awards { get; set; }
        public int Metascore { get; set; }
        public decimal TomatoRating { get; set; }
        public int TomatoReviews { get; set; }
        public int TomatoFresh { get; set; }
        public int TomatoRotten { get; set; }
        public int TomatoUserMeter { get; set; }
        public decimal TomatoUserRating { get; set; }
        public int TomatoUserReviews { get; set; }


        public Subtitle Subtitle { get; set; }
    }
}
