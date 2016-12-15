using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lab1.Models
{
    public class Person
    {
        [BsonId]
        [BsonIgnore]
        public ObjectId Id { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public DateTime Birthday { get; set; }
        public IList<FBMovie> LikedMovies { get; set; }
        public IList<FBMovie> Watches { get; set; }
        public IList<FBMovie> Wants { get; set; }
        public IList<Person> Friends { get; set; }
    }
}