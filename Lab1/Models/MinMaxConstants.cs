using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekt.Models
{
    public class MinMaxConstants
    {
        public static int RuntimeMin = 0;
        public static int RuntimeMax = 350;
        public static int YearMin = 1900;
        public static int YearMax = DateTime.Now.Year;
        public static double IMDBRatingMin = 0;
        public static double IMDBRatingMax = 10;
        public static int MetascoreMin = 0;
        public static int MetascoreMax = 100;
        public static int TomatoMin = 0;
        public static int TomatoMax = 100;
        public static int FBSharesMin = 0;
        public static int FBSharesMax = 140000;
        public static int FBLikesMin = 0;
        public static int FBLikesMax = 176000;
    }
}