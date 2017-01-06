using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

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

        public static Person GetPersonById(string id, bool buildProfileIfNotExists)
        {
            Person person;
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var result = people.Find(p => p.PersonID == id);
            if (result.Count() > 0)
            {
                person = result.First();
                if (person.Profile == null && buildProfileIfNotExists)
                    person = BuildAndGetProfile(person);
                return person;
            }
            else return null;
        }

        //samo dohvati osobu:
        public static Person GetPersonByEmail(string email)
        {
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var result = people.Find(p => p.Email == email);
            if (result.Count() > 0) return result.First();
            else return null;
        }

        //dohvati osobu i izbuildaj joj profil ako ga nema (predaj true za checkProfile):
        public static Person GetPersonByEmail(string email, bool buildProfileIfNotExists)
        {
            Person person;
            var db = MongoInstance.GetDatabase;
            var people = db.GetCollection<Person>("Person");
            var result = people.Find(p => p.Email == email);
            if (result.Count() > 0)
            {
                person = result.First();
                if (person.Profile == null && buildProfileIfNotExists)
                    person = BuildAndGetProfile(person);
                return person;
            }
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
                if (gender != false && match.sameGender == false) continue;
                if (maxAgeDiff != -1 && match.ageDiff > maxAgeDiff) continue;
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
                if (somebody.PersonID == me.PersonID) continue; // :)
                Match match = new Match(me, somebody);
                if (gender != false && match.sameGender == false) continue;
                if (maxAgeDiff != -1 && match.ageDiff > maxAgeDiff) continue;
                if (match.commonFriends.Count < minFriends) continue;
                if (match.commonMovies.Count < minMovies) continue;
                result.Add(somebody);
            }
            return result;
        }
        
        public static void BuildAllProfiles()
        {
            var db = MongoInstance.GetDatabase;
            var persons = db.GetCollection<Person>("Person");
            var everybody = persons.Find(new BsonDocument()).ToList();
            foreach (Person person in everybody)
            {
                person.Profile = new Profile(person);
                persons.ReplaceOne(p => p.Email == person.Email,
                    person,
                    new UpdateOptions { IsUpsert = true });
            }
        }

        public static void BuildProfile(Person person)
        {
            var db = MongoInstance.GetDatabase;
            var persons = db.GetCollection<Person>("Person");
            person.Profile = new Profile(person);
            persons.ReplaceOne(p => p.Email == person.Email,
                person,
                new UpdateOptions { IsUpsert = true });
        }

        public static Person BuildAndGetProfile(Person person)
        {
            var db = MongoInstance.GetDatabase;
            var persons = db.GetCollection<Person>("Person");
            person.Profile = new Profile(person);
            persons.ReplaceOne(p => p.Email == person.Email,
                person,
                new UpdateOptions { IsUpsert = true });
            return person;
        }

    }
}
