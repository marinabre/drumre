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
        // Friends
        public List<string> Friends { get; set; }

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
            foreach (var movie in person.LikedMovies)
            {
                var movieView = new SimpleMovieViewModel();
                movieView.CastSimpleFromMovie(movie);
                LikedMovies.Add(movieView);
            }

            Watches = new List<SimpleMovieViewModel>();
            foreach (var movie in person.Watches)
            {
                var movieView = new SimpleMovieViewModel();
                movieView.CastSimpleFromMovie(movie);
                Watches.Add(movieView);
            }

            Wants = new List<SimpleMovieViewModel>();
            foreach (var movie in person.Wants)
            {
                var movieView = new SimpleMovieViewModel();
                movieView.CastSimpleFromMovie(movie);
                Wants.Add(movieView);
            }
        }
    }
}