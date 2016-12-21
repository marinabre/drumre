using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.General;
using OSDBnet;

namespace BLL
{
    public class Movie
    {
        [BsonId]
        [BsonIgnore]
        
        public ObjectId Id { get; set; }
        public String IMDbId { get; set; }
        public String Title { get; set; }
        public int? Runtime { get; set; }
        public Credits Credits { get; set; }
        public IList<Genre> Genres { get; set; }
        public KeywordsContainer Keywords { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public String PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public SearchContainer<TMDbLib.Objects.Reviews.ReviewBase> Reviews { get; set; }
        public SearchContainer<TMDbLib.Objects.Search.SearchMovie> Similar { get; set; }
        public string Status { get; set; }
        public ResultContainer<Video> Videos { get; set; }
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }



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

        public Subtitle MovieSubtitle { get; set; } 
    }
}