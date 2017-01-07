using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace BLL
{
    [BsonIgnoreExtraElements]
    public class Person
    {
        [BsonId]
        [BsonIgnore]
        public ObjectId Id { get; set; }
        public String PersonID { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public String Email { get; set; }
        public String Gender { get; set; }
        public DateTime Birthday { get; set; }
        public IList<FBMovie> LikedMovies { get; set; }
        public IList<FBMovie> Watches { get; set; }
        public IList<FBMovie> Wants { get; set; }
        public IList<string> Friends { get; set; }
        public Profile Profile { get; set; }

        public Person() { }

        public Person (Person p)
        {
            this.Profile = Profile;
            this.PersonID = p.PersonID;
            this.Name = p.Name;
            this.Surname = p.Surname;
            this.Email = p.Email;
            this.Gender = p.Gender;
            this.Birthday = p.Birthday;
            this.LikedMovies = new List<FBMovie>(p.LikedMovies);
            this.Watches = new List<FBMovie>(p.Watches);
            this.Wants = new List<FBMovie>(p.Wants);
            this.Friends = new List<string>(p.Friends);
            

        }




    }
}