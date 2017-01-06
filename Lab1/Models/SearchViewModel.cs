using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class SearchViewModel
    {
        /// <summary>
        /// If empty, you receive "null"
        /// </summary>
        public List<string> Genres { get; set; }
        public string Actors { get; set; }
        public string Directors { get; set; }

        [DisplayName("From")]
        public int? YearFrom { get; set; }
        [DisplayName("To")]
        public int? YearTo { get; set; }

        [DisplayName("From")]
        public decimal? IMDBRatingFrom { get; set; }
        [DisplayName("To")]
        public decimal? IMDBRatingTo { get; set; }

        [DisplayName("From")]
        public decimal? MetascoreRatingFrom { get; set; }
        [DisplayName("To")]
        public decimal? MetascoreRatingTo { get; set; }

        [DisplayName("From")]
        public decimal? TomatoRatingFrom { get; set; }
        [DisplayName("To")]
        public decimal? TomatoRatingTo { get; set; }

        [DisplayName("From")]
        public int? FBSharesFrom { get; set; }
        [DisplayName("To")]
        public int? FBSharesTo { get; set; }

        [DisplayName("From")]
        public int? FBLikesFrom { get; set; }
        [DisplayName("To")]
        public int? FBLikesTo { get; set; }
    }
}