using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Person(string id, string name, string surname, string email, string gender,
            DateTime birthday, IList<FBMovie> liked, IList<FBMovie> watches,
            IList<FBMovie> wants)
        {
            this.PersonID = id;
            this.Name = name;
            this.Surname = surname;
            this.Email = email;
            this.Gender = gender;
            this.Birthday = birthday;
            this.LikedMovies = liked;
            this.Watches = watches;
            this.Wants = wants;
        }

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

        public string GetBestFriend()
        {
            //try
            //{
                if (this.Friends != null)
                {
                    if (this.Friends.Count > 0)
                    {
                        Dictionary<String, int> Besties = new Dictionary<string, int>();
                        foreach (string f in Friends)
                        {
                            Person friend = PersonRepository.GetPersonById(f);
                            if (friend == null) continue;
                            int commonMovies = this.LikedMovies.Select(m => m.Title).Intersect(friend.LikedMovies.Select(n => n.Title)).Count();
                            if (commonMovies > 0) ;
                            Besties.Add(friend.Name + " " + friend.Surname, commonMovies);
                        }
                        if (Besties.Count < 1) throw new Exception();
                        return Besties.OrderByDescending(pair => pair.Value).Take(1).ToDictionary(pair => pair.Key, pair => pair.Value).Keys.First();
                    }
                }
                return "You don't seem to have any friends using CocoaDuck\n. Why could recommend it...";                    
            //} catch (Exception e)
            //{
            //    return "Sorry, we could not find your friend :(";
            //}
        }




    }
}