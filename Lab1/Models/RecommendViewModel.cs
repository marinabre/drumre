using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class RecommendViewModel
    {
        // Profile based
        public int Genres { get; set; }
        public int Actors { get; set; }
        public int Directors { get; set; }

        // Friends based
        public bool Gender { get; set; }
        public int MaxAgeDifference { get; set; }
        public int MinimalFriendsTogether { get; set; }
        public int MinimalMoviesTogether { get; set; }

        // Based on community
        public bool GenderComm { get; set; }
        public int MaxAgeDifferenceComm { get; set; }
        public int MinimalFriendsTogetherComm { get; set; }
        public int MinimalMoviesTogetherComm { get; set; }

        // OR

        // Based on movies
        public List<MovieViewModel> Movies { get; set; }

        // Based on friends
        public List<string> Friends { get; set; }
    }
}