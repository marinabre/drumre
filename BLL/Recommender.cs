
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace BLL
{
    public class Recommender
    {
        //Preporuke na temelju FBLike kategorije
        //genre - uzima u obzir prvih n najdražih žanrova tog korisnika
        //actor, director - uzima u obzir prvih n najdražih glumaca/redatelja tog korisnika
        //Svaki film iz rezultata ima barem jednog odabranog glumca AND redatelja AND žanr!
        //Ako se bilo koji od njih postavi na 0, taj se ignorira ---> to ukupno daje više rezultata
        public static IList<Movie> RecommendMoviesFromProfile(Person person, int genre, int actor, int director)
        {
            if (person.Profile == null)
            {
                person.Profile = new Profile(person);
            }
            return RecommendMoviesFromProfile(person.Profile, genre, actor, director);
        }
        public static IList<Movie> RecommendMoviesFromProfile(Profile profile, int genre, int actor, int director)
        {
            if (genre == 0 && actor == 0 && director == 0) return null;
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");

            var genres = profile.TopGenres(genre);
            var actors = profile.TopActors(actor);
            var directors = profile.TopDirectors(director);

            var genreFilter = GenresFilter(genres);
            var actorsFilter = ActorsFilter(actors);
            var directorsFilter = DirectorsFilter(directors);

            var filter = genreFilter & actorsFilter & directorsFilter;
            return movies.Find(filter).ToList().Where(x => !profile.LikedMovies.Any(y => y.IMDbId == x.IMDbId)).ToList();
        }
        public static IList<Movie> RecommendMoviesFromProfile(Profile profile)
        {
            return RecommendMoviesFromProfile(profile, 4, 5, 10);
        }

        //Društvene preporuke, jedna uzima u obzir samo prijatelje, a druga sve u bazi; inače rade jednako
        //sameGender, maxAgeDiff, minFriends(zajednički) i minMovies(zajednički) služe da se isfiltriraju prijatelji/ljudi
        //Ako se hoće ignorirati koje od polja treba stavit:
        //  sameGender = false, maxAgeDiff= -1, minFriends = 0, minMovies = 0
        //Nakon što se isfiltriraju prijatelji/ljudi, preporuka se svodi na to da se preporuče njihovi filmovi
        //(naravno, iz liste se prije izbace svi filmovi koje je korisnik već odgledao (Liked, Watched))
        public static IList<Movie> RecommendMoviesFromFriends(Person person, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            IList<Person> friends = PersonRepository.FilterFriends(person, sameGender, maxAgeDiff, minFriends, minMovies); //all friends
            IList<Movie> recommendation = new List<Movie>();
            foreach (Person friend in friends)
            {
                recommendation = recommendation.Union(friend.Profile.LikedMovies).ToList();
            }
            return recommendation.Except(person.Profile.LikedMovies).Where(p => !person.Watches.Any(p2 => p2.Title == p.Title)).ToList();
        }
        public static IList<Movie> RecommendMoviesFromEverybody(Person person, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            IList<Person> people = PersonRepository.FilterPeople(person, sameGender, maxAgeDiff, minFriends, minMovies); //all friends
            IList<Movie> recommendation = new List<Movie>();
            foreach (Person p in people)
            {
                recommendation = recommendation.Union(p.Profile.LikedMovies).ToList();
            }
            return recommendation.Except(person.Profile.LikedMovies).Where(p => !person.Watches.Any(p2 => p2.Title == p.Title)).ToList();
        }

        //Preporuka na temelju liste filmova
        //Radi veoma sneaky&tricky: na temelju te liste izgradi se profil (kao za korisnika)
        //Taj profil se predaje u f-ju RecommendMoviesFromProfile i vraća se taj rezultat :)
        public static IList<Movie> RecommendMoviesFromList(IList<Movie> movies, int genre, int actor, int director)
        {
            Profile profil = new Profile(movies);
            return RecommendMoviesFromProfile(profil, genre, actor, director);
        }

        //Kombiniranje preporuka:
        //Prva funkcija uzima sve parametre u obzir - Recomendus Maximus
        //Druga funkcija uzima u obzir samo koje vrste preporuka se koriste; ostale vrijednosti budu default (4,5,10,false,-1,0,0)
        //Treća funkcija je najseksi - to je u pravilu druga funkcija sa (true, true, true) - Recomendus Minimus
        public static IList<Movie> Recommend(Person person, bool profile, bool friends, bool everybody, int genre, int actor, int director, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            IList<Movie> recommendation = new List<Movie>();
            if (profile) recommendation = recommendation.Union(RecommendMoviesFromProfile(person.Profile, genre, actor, director)).ToList();

            //nemoj dohvaćat od frendača ako ćeš od svih ljudi, jer frendači ionak spadaju pod sve ljude:
            if (friends && !everybody) recommendation = recommendation.Union(RecommendMoviesFromFriends(person, sameGender, maxAgeDiff, minFriends, minMovies)).ToList();
            if (everybody) recommendation = recommendation.Union(RecommendMoviesFromEverybody(person, sameGender, maxAgeDiff, minFriends, minMovies)).ToList();
            return recommendation;
        }
        public static IList<Movie> Recommend(Person person, bool profile, bool friends, bool everybody)
        {
            IList<Movie> recommendation = new List<Movie>();
            if (profile) recommendation = recommendation.Union(RecommendMoviesFromProfile(person.Profile)).ToList();

            //nemoj dohvaćat od frendača ako ćeš od svih ljudi, jer frendači ionak spadaju pod sve ljude:
            if (friends && !everybody) recommendation = recommendation.Union(RecommendMoviesFromFriends(person, false, -1, 0, 0)).ToList();
            if (everybody) recommendation = recommendation.Union(RecommendMoviesFromEverybody(person, false, -1, 0, 0)).ToList();
            return recommendation;
        }
        public static IList<Movie> Recommend(Person person)
        {
            return Recommend(person, true, true, true);
        }

        #region helpers
        public static decimal calculateActorSimilarity (Movie movieA, Movie movieB)
        {
            IList<Cast> castA = movieA.Credits.Cast;
            IList<Cast> castB = movieB.Credits.Cast;

            decimal intersection = castA.Select(a => a.Name).Intersect(castB.Select(b => b.Name)).Count();
            //decimal joined = castA.Count() + castB.Count();

            return intersection;
        }
        public static decimal calculateDirectorSimilarity(Movie movieA, Movie movieB)
        {
            IList<Crew> directorsA = movieA.Credits.Crew.Where(x => x.Job == "Director").ToList();
            IList<Crew> directorsB = movieB.Credits.Crew.Where(x => x.Job == "Director").ToList();

            decimal intersection = directorsA.Select(a => a.Name).Intersect(directorsB.Select(b => b.Name)).Count();
            //decimal joined = directorsA.Count() + directorsB.Count();

            return intersection;
        }
        public static decimal calculateGenreSimilarity(Movie movieA, Movie movieB)
        {
            IList<Genre> genresA = movieA.Genres;
            IList<Genre> genresB = movieB.Genres;

            decimal intersection = genresA.Intersect(genresB).Count();
            decimal joined = genresA.Count() + genresB.Count();

            return 2 * intersection / joined;
        }
        public static MovieSimilarity getSimilarity (Movie movieA, Movie movieB)
        {
            var db = MongoInstance.GetDatabase;
            var similar = db.GetCollection<MovieSimilarity>("similar");
            var builder = Builders<MovieSimilarity>.Filter;
            Movie first = MovieSimilarity.getFirst(movieA, movieB);
            Movie second = MovieSimilarity.getSecond(movieA, movieB);
            var filter = builder.Eq("movieA", first.IMDbId) | builder.Eq("movieB", second.IMDbId);
            var result = similar.Find(filter);

            //ako je u bazi:
            if (result.Count() > 0)
            {
                return result.First();
            }
            else
            {
                //ako nije u bazi:
                MovieSimilarity newSimilarity = new MovieSimilarity(movieA, movieB);
                similar.InsertOne(newSimilarity);
                return newSimilarity;
            } 
        }
        public static IFindFluent<Movie,Movie> SimilarByGenres (IList<String> Genres)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection <Movie> ("movies");
            var filter = GenresFilter(Genres);
            if (filter != null)
                return movies.Find(filter);  
            return null;
        }
        public static IFindFluent<Movie, Movie> SimilarByActors(IList<String> Actors)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var filter = ActorsFilter(Actors);
            if (filter != null)
                return movies.Find(filter);
            return null;
        }
        public static IFindFluent<Movie, Movie> SimilarByDirectors(IList<String> Directors)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var filter = DirectorsFilter(Directors);
            if (filter != null)
                return movies.Find(filter);
            return null;
        }
        public static FilterDefinition<Movie> GenresFilter(IList<String> Genres)
        {
            var builder = Builders<Movie>.Filter;
            if (Genres.Count > 0)
            {
                var name = Genres.ElementAt(0);
                var filter = builder.ElemMatch(x => x.Genres, x => x.Name == name);

                for (int i = 1; i < Genres.Count; i++)
                {
                    var name2 = Genres.ElementAt(i);
                    filter = filter | builder.ElemMatch(x => x.Genres, x => x.Name == name2);
                }
                return filter;
            }
            return builder.Empty;
        }
        public static FilterDefinition<Movie> ActorsFilter(IList<String> Actors)
        {
            var builder = Builders<Movie>.Filter;
            if (Actors.Count > 0)
            {
                var name = Actors.ElementAt(0);
                var filter = builder.ElemMatch(x => x.Credits.Cast, x => x.Name == name);

                for (int i = 1; i < Actors.Count; i++)
                {
                    var name2 = Actors.ElementAt(i);
                    filter = filter | builder.ElemMatch(x => x.Credits.Cast, x => x.Name == name2);
                }
                return filter;
            }
            return builder.Empty;
        }
        public static FilterDefinition<Movie> DirectorsFilter(IList<String> Directors)
        {
            var builder = Builders<Movie>.Filter;
            if (Directors.Count > 0)
            {
                var name = Directors.ElementAt(0);
                var filter = builder.ElemMatch(x => x.Credits.Crew, x => x.Name == name);

                for (int i = 1; i < Directors.Count; i++)
                {
                    var name2 = Directors.ElementAt(i);
                    filter = filter | builder.ElemMatch(x => x.Credits.Crew, x => x.Name == name2);
                }
                return filter;
            }
            return builder.Empty;
        }
        #endregion

    }
}