using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLDummy
{
    public class Movie
    {
        #region Variables
        public string Title { get; set; }
        public string Year { get; set; }
        //public decimal Rating { get; set; }
        public string Runtime { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Country { get; set; }

        public decimal Metascore { get; set; }
        public decimal IMDBRating { get; set; }
        public decimal TomatoRating { get; set; }
        public int Likes { get; set; }        
        public int Shares { get; set; }
        #endregion

        #region Static Methods
        #endregion

        #region Methods
        #endregion
    }
}
