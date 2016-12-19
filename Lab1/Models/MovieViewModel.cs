using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab1.Models
{
    public class MovieViewModel
    {
        #region Variables
        public string Title { get; set; }
        public string Year { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Country { get; set; }

        [DisplayName("Metascore")]
        public decimal Metascore { get; set; }
        [DisplayName("IMDB Rating")]
        public decimal IMDBRating { get; set; }
        [DisplayName("Tomato Rating")]
        public decimal TomatoRating { get; set; }
        [DisplayName("Facebook likes")]
        public int Likes { get; set; }
        [DisplayName("Facebook shares")]
        public int Shares { get; set; }
        #endregion

        #region Methods
        public void CastFromMovie(BLLDummy.Movie movie) 
        {
            Title = movie.Title;
            Year = movie.Year;
            Runtime = movie.Runtime;
            Genre = movie.Genre;
            Director = movie.Director;
            Writer = movie.Writer;
            Country = movie.Country;
            Metascore = movie.Metascore;
            IMDBRating = movie.IMDBRating;
            TomatoRating = movie.TomatoRating;
            Likes = movie.Likes;
            Shares = movie.Shares;
        }
        #endregion

    }
}