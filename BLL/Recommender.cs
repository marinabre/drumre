
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using BLL.Models;

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
            if (person == null) return new List<Movie>();
            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            return RecommendMoviesFromProfile(person.Profile, genre, actor, director).Where(p => !person.Watches.Any(p2 => p2.Title == p.Title)).ToList();
        }
        public static IList<Movie> RecommendMoviesFromProfile(Profile profile, int genre, int actor, int director)
        {
            if (profile == null) return null;
            if (genre == 0 && actor == 0 && director == 0) return null;
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");

            var genres = profile.TopGenres(genre);
            var actors = profile.TopActors(actor);
            var directors = profile.TopDirectors(director);

            var genreFilter = GenresFilter(genres);
            var actorsFilter = ActorsFilter(actors);
            var directorsFilter = DirectorsFilter(directors);

            if (genreFilter == Builders<Movie>.Filter.Empty & actorsFilter == Builders<Movie>.Filter.Empty & directorsFilter == Builders<Movie>.Filter.Empty)
                return new List<Movie>();

            var filter = genreFilter & actorsFilter & directorsFilter;
            return movies.Find(filter).Limit(150).ToList().Where(x => !profile.LikedMovies.Any(y => y.IMDbId == x.IMDbId)).ToList();
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
            if (person == null) return new List<Movie>();
            IList<Person> friends = PersonRepository.FilterFriends(person, sameGender, maxAgeDiff, minFriends, minMovies); //all friends
            IList<Movie> recommendation = new List<Movie>();
            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            foreach (Person p in friends)
            {
                Person friend = p;
                if (p.Profile == null)
                    friend = PersonRepository.BuildAndGetProfile(p);
                recommendation = recommendation.Union(friend.Profile.LikedMovies).ToList();
            }
            return recommendation.Where(x => !person.Profile.LikedMovies.Any(y => y.IMDbId == x.IMDbId)).Where(p => !person.Watches.Any(p2 => p2.Title == p.Title)).ToList();
        }
        public static IList<Movie> RecommendMoviesFromEverybody(Person person, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            if (person == null) return new List<Movie>();
            IList<Person> people = PersonRepository.FilterPeople(person, sameGender, maxAgeDiff, minFriends, minMovies); //all friends
            IList<Movie> recommendation = new List<Movie>();

            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            foreach (Person somebody in people)
            {
                Person p = somebody;

                if (somebody.Profile == null)
                    p = PersonRepository.BuildAndGetProfile(somebody);
                recommendation = recommendation.Union(p.Profile.LikedMovies).ToList();
            }
            return recommendation.Where(x => !person.Profile.LikedMovies.Any(y => y.IMDbId == x.IMDbId)).Where(p => !person.Watches.Any(p2 => p2.Title == p.Title)).ToList();
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
        //Prva funkcija uzima sve parametre u obzir, ali isti uvjeti vrijede za frendove i community- Recomendus Maximus
        //Druga funkcija uzima u obzir samo koje vrste preporuka se koriste; ostale vrijednosti budu default (4,5,10,false,-1,0,0), isti uvjeti vrijede za friends i everybody
        //Treća funkcija je najseksi - to je u pravilu druga funkcija sa (true, true, true) - Recomendus Minimus
        //Četvrta funkcija je GODLIKE - posebno postavlja za frendače, a posebno za comunity uvjete
        public static IList<Movie> Recommend(Person person, bool profile, bool friends, bool everybody, int genre, int actor, int director, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            if (person == null) return new List<Movie>();
            IList<Movie> recommendation = new List<Movie>();
            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            if (profile) recommendation = recommendation.Union(RecommendMoviesFromProfile(person.Profile, genre, actor, director)).ToList();

            if (friends) recommendation = recommendation.Union(RecommendMoviesFromFriends(person, sameGender, maxAgeDiff, minFriends, minMovies)).ToList();
            if (everybody) recommendation = recommendation.Union(RecommendMoviesFromEverybody(person, sameGender, maxAgeDiff, minFriends, minMovies)).ToList();
            return recommendation;
        }
        public static IList<Movie> Recommend(Person person, bool profile, bool friends, bool everybody)
        {
            if (person == null) return new List<Movie>();
            IList<Movie> recommendation = new List<Movie>();
            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            if (profile) recommendation = recommendation.Union(RecommendMoviesFromProfile(person.Profile)).ToList();

            if (friends) recommendation = recommendation.Union(RecommendMoviesFromFriends(person, false, -1, 0, 0)).ToList();
            if (everybody) recommendation = recommendation.Union(RecommendMoviesFromEverybody(person, false, -1, 0, 1)).ToList();
            return recommendation;
        }
        public static IList<Movie> Recommend(Person person)
        {
            return Recommend(person, true, true, true);
        }

        //Recommender GODLIKE - uzima sve parametre, ide točno po redu kak smo nacrtali na papiru(prvo frendaći, a onda svi); naravno predaš boolove jel se uopće koristi koja pretraga
        public static IList<Movie> Recommend(Person person, bool profile, bool friends, bool everybody, int genre, int actor, int director,
            bool FsameGender, int FmaxAgeDiff, int FminFriends, int FminMovies, bool sameGender, int maxAgeDiff, int minFriends, int minMovies)
        {
            if (person == null) return new List<Movie>();
            IList<Movie> recommendation = new List<Movie>();
            if (person.Profile == null)
                person = PersonRepository.BuildAndGetProfile(person);
            if (profile) recommendation = recommendation.Union(RecommendMoviesFromProfile(person, genre, actor, director)).ToList();

            if (friends) recommendation = recommendation.Union(RecommendMoviesFromFriends(person, FsameGender, FmaxAgeDiff, FminFriends, FminMovies)).ToList();
            if (everybody) recommendation = recommendation.Union(RecommendMoviesFromEverybody(person, sameGender, maxAgeDiff, minFriends, minMovies)).ToList();
            return recommendation;
        }



        public static IList<Movie> FilterResults(IList<Movie> movies, Filter filter)
        {

            if (movies == null) return new List<Movie>();
            if (filter == null) return movies;
            if (filter.isEmpty()) return movies;
            IEnumerable<Movie> result = movies;

            if (filter.Genres != null)
                result = result.Where(m => m.Genres.Any(g => filter.Genres.Contains(g.Name)));
            if (filter.Actors != null)
                result = result.Where(m => m.Credits.Cast.Any(g => filter.Actors.Contains(g.Name)));
            if (filter.Directors != null)
                result = result.Where(m => m.Credits.Cast.Any(g => filter.Directors.Contains(g.Name)));

            if (filter.YearFrom != null)
                result = result.Where(m => m.ReleaseDate.HasValue == true).Where(m => m.ReleaseDate.Value.Year >= filter.YearFrom);
            if (filter.YearTo != null)
                result = result.Where(m => m.ReleaseDate.HasValue == true).Where(m => m.ReleaseDate.Value.Year <= filter.YearTo);

            if (filter.RuntimeFrom != null)
                result = result.Where(m => m.Runtime >= filter.RuntimeFrom);
            if (filter.RuntimeTo != null)
                result = result.Where(m => m.Runtime <= filter.RuntimeTo);

            if (filter.IMDBRatingFrom != null)
                result = result.Where(m => m.VoteAverage >= filter.IMDBRatingFrom);
            if (filter.IMDBRatingTo != null)
                result = result.Where(m => m.VoteAverage <= filter.IMDBRatingTo);

            if (filter.MetascoreRatingFrom != null)
                result = result.Where(m => m.Metascore >= filter.MetascoreRatingFrom);
            if (filter.MetascoreRatingTo != null)
                result = result.Where(m => m.Metascore <= filter.MetascoreRatingTo);

            if (filter.TomatoRatingFrom != null)
                result = result.Where(m => m.TomatoRating >= filter.TomatoRatingFrom);
            if (filter.TomatoRatingTo != null)
                result = result.Where(m => m.TomatoRating <= filter.TomatoRatingTo);

            if (filter.FBSharesFrom != null)
                result = result.Where(m => m.FBShares >= filter.FBSharesFrom);
            if (filter.FBSharesTo != null)
                result = result.Where(m => m.FBShares <= filter.FBSharesTo);

            if (filter.FBLikesFrom != null)
                result = result.Where(m => m.FBLikes >= filter.FBLikesFrom);
            if (filter.FBLikesTo != null)
                result = result.Where(m => m.FBLikes <= filter.FBLikesTo);

            return result.ToList();
        }


        #region helpers
        [Obsolete]
        public static decimal calculateActorSimilarity (Movie movieA, Movie movieB)
        {
            IList<Cast> castA = movieA.Credits.Cast;
            IList<Cast> castB = movieB.Credits.Cast;

            decimal intersection = castA.Select(a => a.Name).Intersect(castB.Select(b => b.Name)).Count();
            //decimal joined = castA.Count() + castB.Count();

            return intersection;
        }
        [Obsolete]
        public static decimal calculateDirectorSimilarity(Movie movieA, Movie movieB)
        {
            IList<Crew> directorsA = movieA.Credits.Crew.Where(x => x.Job == "Director").ToList();
            IList<Crew> directorsB = movieB.Credits.Crew.Where(x => x.Job == "Director").ToList();

            decimal intersection = directorsA.Select(a => a.Name).Intersect(directorsB.Select(b => b.Name)).Count();
            //decimal joined = directorsA.Count() + directorsB.Count();

            return intersection;
        }
        [Obsolete]
        public static decimal calculateGenreSimilarity(Movie movieA, Movie movieB)
        {
            IList<Genre> genresA = movieA.Genres;
            IList<Genre> genresB = movieB.Genres;

            decimal intersection = genresA.Intersect(genresB).Count();
            decimal joined = genresA.Count() + genresB.Count();

            return 2 * intersection / joined;
        }
        [Obsolete]
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
        [Obsolete]
        public static IFindFluent<Movie,Movie> SimilarByGenres (IList<String> Genres)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection <Movie> ("movies");
            var filter = GenresFilter(Genres);
            if (filter != null)
                return movies.Find(filter);  
            return null;
        }
        [Obsolete]
        public static IFindFluent<Movie, Movie> SimilarByActors(IList<String> Actors)
        {
            var db = MongoInstance.GetDatabase;
            var movies = db.GetCollection<Movie>("movies");
            var filter = ActorsFilter(Actors);
            if (filter != null)
                return movies.Find(filter);
            return null;
        }
        [Obsolete]
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