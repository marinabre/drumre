using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class MovieSimilarity
    {
        public String AimdbID;
        public String BimdbID;
        public String combination;
        public decimal actorSimilarity;
        public decimal directorSimilarity;
        public decimal genreSimilarity;

        public MovieSimilarity() { }
        
        public MovieSimilarity (Movie A, Movie B)
        {    
            int idA = Int32.Parse(A.IMDbId.Substring(2));
            int idB = Int32.Parse(B.IMDbId.Substring(2));

            if (idA < idB)
            {
                this.AimdbID = A.IMDbId;
                this.BimdbID = B.IMDbId;
            }
            else
            {
                this.BimdbID = A.IMDbId;
                this.AimdbID = B.IMDbId;
            }

            combination = AimdbID + BimdbID;
            this.actorSimilarity = Recommender.calculateActorSimilarity(A, B);
            this.directorSimilarity = Recommender.calculateDirectorSimilarity(A, B);
            this.genreSimilarity = Recommender.calculateGenreSimilarity(A, B);
        }


    }
}
