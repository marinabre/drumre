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
        public string Title { get; set; }
        public string PosterURL { get; set; }
        public string IMDBID { get; set; }

        public int Runtime { get; set; }
        //public Credits Credits { get; set; }
        public List<string> Genres { get; set; }
        public string Keywords { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        [DisplayName("Release date")]
        public DateTime ReleaseDate { get; set; }

        public List<string> Reviews { get; set; }
        //public SearchContainer<TMDbLib.Objects.Search.SearchMovie> Similar { get; set; }
        public string Status { get; set; }
        //public ResultContainer<Video> Videos { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }



        //Marina, ovo do tuda možeš mjenjat, ovo ispod nemoj, to je za podatke iz mojih APIja. Ana :*
        //P.S. funkcije za dohvat možeš zgurat pod MovieRepository
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

        [DisplayName("Download link")]
        public string SubtitleDownloadLink { get; set; }
        [DisplayName("Page link")]
        public string SubtitlePageLink { get; set; }        
        #endregion

        #region Methods
        public void CastFromMovie(BLL.Movie movie) 
        {
            Title = movie.Title;
            IMDBID = movie.IMDbId;
            PosterURL = "http://image.tmdb.org/t/p/w185" + movie.PosterPath;
        }
        #endregion

    }
}