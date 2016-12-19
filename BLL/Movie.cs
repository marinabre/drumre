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
        public long FBLikes { get; set; }
        public long FBShares { get; set; }
    }
}