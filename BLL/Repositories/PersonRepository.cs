using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PersonRepository
    {
        public static Person GetPersonById (string id)
        {
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var result = people.Find(p => p.PersonID == id);
            if (result.Count() > 0) return result.First();
            else return null;
        }

        public static Person GetPersonByName(string name)
        {
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var result = people.Find(p => p.Name == name);
            if (result.Count() > 0) return result.First();
            else return null;
        }

        public static Person GetPersonByName(string name, string collection)
        {
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>(collection);
            var result = people.Find(p => p.Name == name);
            if (result.Count() > 0) return result.First();
            else return null;
        }

        public static IList<Person> FilterFriends(Person me, bool gender, int maxAgeDiff, int minFriends, int minMovies)
        {
            //gender - true ako je relevantno, false ako ignororamo gender
            //maxAgeDiff - stavi -1 ako hoćeš igrnorirat to polje
            IList<Person> result = new List<Person>();
            foreach (string id in me.Friends)
            {
                Person friend = GetPersonById(id);
                Match match = new Match(me, friend);
                if (match.sameGender == false) continue;
                if (maxAgeDiff == -1 || match.ageDiff > maxAgeDiff) continue;
                if (match.commonFriends.Count < minFriends) continue;
                if (match.commonMovies.Count < minMovies) continue;
                result.Add(friend);
            }
            return result;
        }

        public static IList<Person> FilterPeople(Person me, bool gender, int maxAgeDiff, int minFriends, int minMovies)
        {
            //gender - true ako je relevantno, false ako ignororamo gender
            //maxAgeDiff - stavi -1 ako hoćeš igrnorirat to polje

            IList<Person> result = new List<Person>();
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var everybody = people.Find(new BsonDocument()).ToList();
            foreach (Person somebody in everybody)
            {
                Match match = new Match(me, somebody);
                if (match.sameGender == false) continue;
                if (maxAgeDiff == -1 || match.ageDiff > maxAgeDiff) continue;
                if (match.commonFriends.Count < minFriends) continue;
                if (match.commonMovies.Count < minMovies) continue;
                result.Add(somebody);
            }
            return result;
        }
    }
}
