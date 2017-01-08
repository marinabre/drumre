using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class MovieViewModel
    {
        #region Variables
        public string htmlClass { get; set; }

        public string Title { get; set; }
        public string PosterURL { get; set; }
        public string IMDBID { get; set; }

        public int Runtime { get; set; }
        public Dictionary<string, string> Crew { get; set; }
        public Dictionary<string, string> Cast { get; set; }
        //public Credits Credits { get; set; }
        public string Genres { get; set; }
        public string Keywords { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        [DisplayName("Release date")]
        public DateTime? ReleaseDate { get; set; }

        public List<string> Reviews { get; set; }
        //public SearchContainer<TMDbLib.Objects.Search.SearchMovie> Similar { get; set; }
        public string Status { get; set; }
        //public ResultContainer<Video> Videos { get; set; }
        [DisplayName("Vote Average")]
        public double VoteAverage { get; set; }
        [DisplayName("Vote Count")]
        public int VoteCount { get; set; }

        [DisplayName("Facebook Likes")]
        public long FBLikes { get; set; }
        [DisplayName("Facebook Shares")]
        public long FBShares { get; set; }
        public String Rated { get; set; }
        public String Language { get; set; }
        public String Country { get; set; }
        public String Awards { get; set; }
        public int Metascore { get; set; }
        [DisplayName("Rotten Tomatoes Rating")]
        public decimal TomatoRating { get; set; }
        [DisplayName("Rotten Tomatoes Reviews")]
        public int TomatoReviews { get; set; }
        [DisplayName("Rotten Tomatoes Fresh")]
        public int TomatoFresh { get; set; }
        [DisplayName("Rotten Tomatoes Rotten")]
        public int TomatoRotten { get; set; }
        [DisplayName("Rotten Tomatoes User Meter")]
        public int TomatoUserMeter { get; set; }
        [DisplayName("Rotten Tomatoes User Rating")]
        public decimal TomatoUserRating { get; set; }        
        //public int TomatoUserReviews { get; set; }

        [DisplayName("Download link")]
        public string SubtitleDownloadLink { get; set; }
        [DisplayName("Page link")]
        public string SubtitlePageLink { get; set; }

        [DisplayName("Trailer")]
        public string TrailerURL { get; set; }
        #endregion

        #region Methods
        public void CastSimpleFromMovie(BLL.Movie movie) 
        {
            Title = movie.Title;
            IMDBID = movie.IMDbId;
            PosterURL = "http://image.tmdb.org/t/p/w185" + movie.PosterPath;
        }

        public void CastSearchFromMovie(BLL.Movie movie)
        {
            CastSimpleFromMovie(movie);
            VoteAverage = movie.VoteAverage;
            Metascore = movie.Metascore;
            TomatoRating = movie.TomatoRating;
        }

        public void CastFromMovie(BLL.Movie movie)
        {
            CastSimpleFromMovie(movie);

            TrailerURL = BLL.MovieRepository.GetYTURLForMovie(movie);

            if (movie.Runtime != null)
            {
                Runtime = movie.Runtime.Value;
            } 
            else
            {
                Runtime = 0;
            }
            Genres = "";
            foreach (var genre in movie.Genres)
            {
                Genres += genre.Name + ", ";
            }
            Keywords = "";
            if (movie.Keywords != null)
            {
                foreach (var keyword in movie.Keywords.Keywords)
                {
                    Keywords += keyword.Name + ", ";
                }
            }            
            Overview = movie.Overview;
            Popularity = movie.Popularity;
            ReleaseDate = movie.ReleaseDate;
            Status = movie.Status;
            VoteAverage = movie.VoteAverage;
            VoteCount = movie.VoteCount;

            FBLikes = movie.FBLikes;
            FBShares = movie.FBShares;
            Rated = movie.Rated;
            Language = movie.Language;
            Country = movie.Country;
            Awards = movie.Awards;
            Metascore = movie.Metascore;
            TomatoRating = movie.TomatoRating;
            TomatoReviews = movie.TomatoReviews;
            TomatoFresh = movie.TomatoFresh;
            TomatoRotten = movie.TomatoRotten;
            TomatoUserMeter = movie.TomatoUserMeter;
            TomatoUserRating = movie.TomatoUserRating;

            Crew = new Dictionary<string, string>();
            foreach (var crew in movie.Credits.Crew)
            {
                Crew.Add(crew.Name, crew.Job);
            }

            Cast = new Dictionary<string, string>();
            foreach (var cast in movie.Credits.Cast)
            {
                Cast.Add(cast.Name, cast.Character);
            }

            if (movie.MovieSubtitle != null)
            {
                SubtitleDownloadLink = movie.MovieSubtitle.SubTitleDownloadLink.ToString();
                SubtitlePageLink = movie.MovieSubtitle.SubtitlePageLink.ToString();
            }                        
    }
        #endregion

    }
}