using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class UserViewModel
    {
        // ID
        public string ID { get; set; }
        // Name
        public string Name { get; set; }
        // Surname
        public string Surname { get; set; }
        // Email
        public string Email { get; set; }
        // Gender
        public string Gender { get; set; }
        // Birthday
        public DateTime Birthday { get; set; }

        // Liked movies
        [DisplayName("Liked Movies")]
        public List<SimpleMovieViewModel> LikedMovies { get; set; }
        // Watches
        public List<SimpleMovieViewModel> Watches { get; set; }
        // Wants
        public List<SimpleMovieViewModel> Wants { get; set; }

        // Fav Actors
        [DisplayName("Favourite Actors")]
        public List<string> FavActors { get; set; }
        // Fav Directors
        [DisplayName("Favourite Directors")]
        public List<string> FavDirectors { get; set; }
        // Fav Genres
        [DisplayName("Favourite Genres")]
        public List<string> FavGenres { get; set; }

        public void CastPersonToUser(BLL.Person person)
        {
            ID = person.Id.ToString();
            Name = person.Name;
            Surname = person.Surname;
            Email = person.Email;
            Gender = person.Gender;
            Birthday = person.Birthday;

            LikedMovies = new List<SimpleMovieViewModel>();
            foreach (var movie in person.Profile.LikedMovies)
            {
                var movieView = new SimpleMovieViewModel();
                movieView.CastSimpleFromMovie(movie);
                LikedMovies.Add(movieView);
            }

            var movieRepo = new BLL.MovieRepository();

            Watches = new List<SimpleMovieViewModel>();
            if (person.Watches.Count > 0)
            {
                var watches = movieRepo.GetMoviesByFB(person.Watches);
                foreach (var movie in watches)
                {

                    var movieView = new SimpleMovieViewModel();
                    movieView.CastSimpleFromMovie(movie);
                    Watches.Add(movieView);
                }
            }            

            Wants = new List<SimpleMovieViewModel>();
            if (person.Wants.Count > 0)
            {
                var wants = movieRepo.GetMoviesByFB(person.Wants);
                foreach (var movie in wants)
                {
                    var movieView = new SimpleMovieViewModel();
                    movieView.CastSimpleFromMovie(movie);
                    Wants.Add(movieView);
                }
            }
            
            // FavActors
            FavActors = person.Profile.TopActors(5);

            // FavDirectors
            FavDirectors = person.Profile.TopDirectors(5);

            // FavGenres
            FavGenres = person.Profile.TopGenres(5);
        }
    }
}