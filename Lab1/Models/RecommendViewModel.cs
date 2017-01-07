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
        public int Genres { get; set; }
        public int Actors { get; set; }
        public int Directors { get; set; }

        // Friends based
        public bool Gender { get; set; }
        [DisplayName("Maximal age difference")]
        public int MaxAgeDifference { get; set; }
        [DisplayName("Minimal friends together")]
        public int MinimalFriendsTogether { get; set; }
        [DisplayName("Minimal movies together")]
        public int MinimalMoviesTogether { get; set; }

        // Based on community
        [DisplayName("Gender")]
        public bool GenderComm { get; set; }
        [DisplayName("Maximal age difference")]
        public int MaxAgeDifferenceComm { get; set; }
        [DisplayName("Minimal friends together")]
        public int MinimalFriendsTogetherComm { get; set; }
        [DisplayName("Minimal movies together")]
        public int MinimalMoviesTogetherComm { get; set; }

        // OR

        // Based on movies
        public List<SimpleMovieViewModel> Movies { get; set; }

        // Based on friends
        public List<string> Friends { get; set; }
    }
}