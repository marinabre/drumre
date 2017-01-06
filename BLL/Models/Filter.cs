using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class Filter
    {
        /// <summary>
        /// If empty, you receive "null"
        /// </summary>
        public IList<string> Genres { get; set; }
        /// <summary>
        /// If empty, you receive "null"
        /// </summary>
        public IList<string> Actors { get; set; }

        public IList<string> Directors { get; set; }

        /// <summary>
        /// Also "null" if nothing; get the value using YearFrom.Value
        /// all the rest are "null" as well
        /// </summary>
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public double? IMDBRatingFrom { get; set; }
        public double? IMDBRatingTo { get; set; }

        public int? MetascoreRatingFrom { get; set; }
        public int? MetascoreRatingTo { get; set; }

        public int? TomatoRatingFrom { get; set; }
        public int? TomatoRatingTo { get; set; }

        public int? FBSharesFrom { get; set; }
        public int? FBSharesTo { get; set; }

        public int? FBLikesFrom { get; set; }
        public int? FBLikesTo { get; set; }

        public Filter(IList<string> genres)
        {
            this.Genres = genres;
            this.Actors = null;
            this.Directors = null;
            this.YearFrom = null;
            this.YearTo = null;
            this.IMDBRatingFrom = null;
            this.IMDBRatingTo = null;
            this.MetascoreRatingFrom = null;
            this.MetascoreRatingTo = null;
            this.FBSharesFrom = null;
            this.FBSharesTo = null;
            this.FBLikesFrom = null;
            this.FBLikesTo = null;
        }

        public Filter()
        {
            this.Genres = null;
            this.Actors = null;
            this.Directors = null;
            this.YearFrom = null;
            this.YearTo = null;
            this.IMDBRatingFrom = null;
            this.IMDBRatingTo = null;
            this.MetascoreRatingFrom = null;
            this.MetascoreRatingTo = null;
            this.FBSharesFrom = null;
            this.FBSharesTo = null;
            this.FBLikesFrom = null;
            this.FBLikesTo = null;
        }

        public bool isEmpty()
        {
            if (this == new Filter()) return true;
            return false;
        }
    }
}

    
