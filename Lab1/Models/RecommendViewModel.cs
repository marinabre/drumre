using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class RecommendViewModel
    {
        // Profile based
        public bool Profile { get; set; }
        [DisplayName("Genres")]
        public int SGenres { get; set; }
        public int Actors { get; set; }
        public int Directors { get; set; }

        // Friends based
        public bool Friends { get; set; }
        [DisplayName("Same Gender")]
        public bool Gender { get; set; }
        [DisplayName("Maximal age difference")]
        public int MaxAgeDifference { get; set; }
        [DisplayName("Minimal friends together")]
        public int MinimalFriendsTogether { get; set; }
        [DisplayName("Minimal movies together")]
        public int MinimalMoviesTogether { get; set; }

        // Based on community
        public bool Community { get; set; }
        [DisplayName("Same Gender")]
        public bool GenderComm { get; set; }
        [DisplayName("Maximal age difference")]
        public int MaxAgeDifferenceComm { get; set; }
        [DisplayName("Minimal friends together")]
        public int MinimalFriendsTogetherComm { get; set; }
        [DisplayName("Minimal movies together")]
        public int MinimalMoviesTogetherComm { get; set; }

        public bool Filter { get; set; }
        // OR

        // Based on movies
        public List<SimpleMovieViewModel> Movies { get; set; }

        // Based on friends
        public List<string> FriendsList { get; set; }

        public RecommendViewModel()
        {
            Profile = false;
            Friends = false;
            Community = false;
            MaxAgeDifference = -1;
            MaxAgeDifferenceComm = -1;
        }
    }
}