using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;

namespace BLL
{
    public class Profile
    {
        [BsonDictionaryOptions(Representation = DictionaryRepresentation.ArrayOfArrays)]
        public IDictionary<String, int> FavouriteActors;
        [BsonDictionaryOptions(Representation = DictionaryRepresentation.ArrayOfArrays)]
        public IDictionary<String, int> FavouriteDirectors;
        [BsonDictionaryOptions(Representation = DictionaryRepresentation.ArrayOfArrays)]
        public IDictionary<String, int> FavouriteGenres;
        public IList<Movie> LikedMovies = new List<Movie>();

        public Profile(Person person)
        {
            var repo = new MovieRepository();
            FavouriteActors = new Dictionary<String, int>();
            FavouriteDirectors = new Dictionary<String, int>();
            FavouriteGenres = new Dictionary<String, int>();
            LikedMovies = repo.GetMoviesByFB(person.LikedMovies);
            foreach (Movie movie in LikedMovies)
            {
                if (movie.Credits.Cast.Count > 0)
                {
                    foreach (var actor in movie.Credits.Cast)
                    {
                        if (FavouriteActors.ContainsKey(actor.Name))
                            FavouriteActors[actor.Name]++;
                        else
                            FavouriteActors.Add(actor.Name, 1);
                    }
                }

                if (movie.Credits.Crew.Count > 0)
                {
                    foreach (var crewMember in movie.Credits.Crew)
                    {
                        if (crewMember.Job == "Director")
                        {
                            if (FavouriteDirectors.ContainsKey(crewMember.Name))
                                FavouriteDirectors[crewMember.Name]++;
                            else
                                FavouriteDirectors.Add(crewMember.Name, 1);
                        }
                    }
                }

                if (movie.Genres.Count > 0)
                {
                    foreach (Genre genre in movie.Genres)
                    {
                        if (FavouriteGenres.ContainsKey(genre.Name))
                            FavouriteGenres[genre.Name]++;
                        else
                            FavouriteGenres.Add(genre.Name, 1);
                    }
                }
            }           
        }
        public Profile(IList<Movie> LikedMovies)
        {
            FavouriteActors = new Dictionary<String, int>();
            FavouriteDirectors = new Dictionary<String, int>();
            FavouriteGenres = new Dictionary<String, int>();

            foreach (Movie movie in LikedMovies)
            {
                if (movie.Credits.Cast.Count > 0)
                {
                    foreach (var actor in movie.Credits.Cast)
                    {
                        if (FavouriteActors.ContainsKey(actor.Name))
                            FavouriteActors[actor.Name]++;
                        else
                            FavouriteActors.Add(actor.Name, 1);
                    }
                }

                if (movie.Credits.Crew.Count > 0)
                {
                    foreach (var crewMember in movie.Credits.Crew)
                    {
                        if (crewMember.Job == "Director")
                        {
                            if (FavouriteDirectors.ContainsKey(crewMember.Name))
                                FavouriteDirectors[crewMember.Name]++;
                            else
                                FavouriteDirectors.Add(crewMember.Name, 1);
                        }
                    }
                }

                if (movie.Genres.Count > 0)
                {
                    foreach (Genre genre in movie.Genres)
                    {
                        if (FavouriteGenres.ContainsKey(genre.Name))
                            FavouriteGenres[genre.Name]++;
                        else
                            FavouriteGenres.Add(genre.Name, 1);
                    }
                }
            }
        }

        public List<string> TopActors (int limit)
        {
            if (limit == 0) return new List<string>();
            return FavouriteActors.OrderByDescending(pair => pair.Value).Take(limit)
               .ToDictionary(pair => pair.Key, pair => pair.Value).Keys.ToList();
        }
        public List<string> TopDirectors(int limit)
        {
            if (limit == 0) return new List<string>();
            return FavouriteDirectors.OrderByDescending(pair => pair.Value).Take(limit)
               .ToDictionary(pair => pair.Key, pair => pair.Value).Keys.ToList();
        }
        public List<String> TopGenres(int limit)
        {
            if (limit == 0) return new List<String>();
            return FavouriteGenres.OrderByDescending(pair => pair.Value).Take(limit)
               .ToDictionary(pair => pair.Key, pair => pair.Value).Keys.ToList();
        }
    }
}
