using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMDbSharp;

namespace BLL
{
    public class MovieDetails
    {
        public Item item { get; set; }
        public Uri downloadLink { get; set; }

    }
}