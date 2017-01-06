using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class SearchViewModel
    {
        //I haz time for : minute
        //I’m in the mood for: žanr
        //I’d like smth like : movie
        //I like his face : actor
        //I like his taste : frend
        //I like his hitlers : redatelj
        //Gimmie something(bez ovih ostalih, nebitno mi je)

        //*==> rating se podrazumijeva

        //Stvari na movie imdb objektu:
        //Movie.credits.crew
        //Movie.similar

        // no trouble
        public int Time { get; set; }
        // otklen?
        public List<string> Genres { get; set; }
        // Njegovi likeani filmovi
        public List<string> Movies { get; set; }
        // otklen?
        public List<string> Actors { get; set; }
        // OK, ovo nije problem
        public List<string> Friends { get; set; }
        // Ovo je, isti problem - otklen?
        public List<string> Directors { get; set; }
    }
}