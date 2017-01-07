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
        public int? RuntimeFrom { get; set; }
        [DisplayName("To")]
        public int? RuntimeTo { get; set; }

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

        public SearchViewModel()
        {
            YearFrom = -1;
            YearTo = -1;
            RuntimeFrom = -1;
            RuntimeTo = -1;
            IMDBRatingFrom = -1;
            IMDBRatingTo = -1;
            TomatoRatingFrom = -1;
            TomatoRatingTo = -1;
            MetascoreRatingFrom = -1;
            MetascoreRatingTo = -1;
            FBSharesFrom = -1;
            FBSharesTo = -1;
            FBLikesFrom = -1;
            FBLikesTo = -1;        
        }
    }
}