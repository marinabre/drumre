using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MovieSimilarity
    {
        public String movieA;
        public String movieB;
        public decimal actorSimilarity;
        public decimal directorSimilarity;
        public decimal genreSimilarity;

        public MovieSimilarity() { }


        public MovieSimilarity (Movie A, Movie B)
        {
            //
        }
    }
}
