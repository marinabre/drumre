using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{
    public class Match
    {
        public Person personA;
        public Person personB;
        public bool sameGender;
        public int ageDiff;
        public bool friends;
        public IList<string> commonFriends;
        public IList<FBMovie> commonMovies;

        public Match()
        {
            sameGender = false;
            commonFriends = new List<string>();
            commonMovies = new List<FBMovie>();
        }

        public Match (Person A, Person B)
        {
            if (A.Gender == B.Gender) this.sameGender = true;
            this.ageDiff = Math.Abs(A.Birthday.Year - B.Birthday.Year);
            this.commonFriends = A.Friends.Intersect(B.Friends).ToList();
            IList<FBMovie> moviesA = A.LikedMovies.Union(A.Watches).ToList();
            IList<FBMovie> moviesB = B.LikedMovies.Union(B.Watches).ToList();
            this.commonMovies = moviesA.Intersect(moviesB).ToList();
            if (A.Friends.Contains(B.PersonID) && B.Friends.Contains(A.PersonID)) friends = true;
            else friends = false;
        }
    }
}
