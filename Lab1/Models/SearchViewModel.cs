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
        public double? IMDBRatingFrom { get; set; }
        [DisplayName("To")]
        public double? IMDBRatingTo { get; set; }

        [DisplayName("From")]
        public int? MetascoreRatingFrom { get; set; }
        [DisplayName("To")]
        public int? MetascoreRatingTo { get; set; }

        [DisplayName("From")]
        public int? TomatoRatingFrom { get; set; }
        [DisplayName("To")]
        public int? TomatoRatingTo { get; set; }

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