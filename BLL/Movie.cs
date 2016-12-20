using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TMDbLib.Objects.General;

namespace BLL
{
    public class Movie
    {
        [BsonId]
        [BsonIgnore]
        
        public ObjectId Id { get; set; }
        public String IMDbId { get; set; }
        public bool Adult { get; set; }
        public String Title { get; set; }
        public IList<Genre> Genres { get; set; }
        public String PosterPath { get; set; }
        public DateTime ReleaseDate { get; set; }
        

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
    }
}