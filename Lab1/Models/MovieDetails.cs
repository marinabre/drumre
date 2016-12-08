using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMDbSharp;

namespace Lab1.Models
{
    public class MovieDetails
    {
        public Item item { get; set; }
        public Uri downloadLink { get; set; }

    }
}