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
        public List<string> Genres { get; set; }
        /// <summary>
        /// If empty, you receive "null"
        /// </summary>
        public List<string> Actors { get; set; }

        /// <summary>
        /// Also "null" if nothing; get the value using YearFrom.Value
        /// all the rest are "null" as well
        /// </summary>
        public int? YearFrom { get; set; }
        public int? YearTo { get; set; }

        public decimal? IMDBRatingFrom { get; set; }
        public decimal? IMDBRatingTo { get; set; }

        public decimal? MetascoreRatingFrom { get; set; }
        public decimal? MetascoreRatingTo { get; set; }

        public decimal? TomatoRatingFrom { get; set; }
        public decimal? TomatoRatingTo { get; set; }

        public int? FBSharesFrom { get; set; }
        public int? FBSharesTo { get; set; }

        public int? FBLikesFrom { get; set; }
        public int? FBLikesTo { get; set; }
    }
}
