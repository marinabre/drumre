using OMDbSharp;
using OSDBnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ShowRepository
    {
        //micanje iz baze propertya : db.getCollection('shows').update({}, {$unset: {ImePRopertya:1}} , {multi: true});

        Baza baza = new Baza();
        public TVShow OMDbData(TVShow tvShow, bool refresh = false)
        {
            OMDbClient omdb = new OMDbClient(true);
            try
            {
                Item omdbResult = omdb.GetItemByID(tvShow.IMDbId).Result;
                if (omdbResult.imdbID != null)
                {
                    tvShow.Rated = omdbResult.Rated;
                    tvShow.Awards = omdbResult.Awards;
                    tvShow.Country = omdbResult.Country;
                    tvShow.Language = omdbResult.Language;

                    if (omdbResult.Metascore != "N/A")
                    {
                        tvShow.Metascore = Int32.Parse(omdbResult.Metascore);
                    }
                    if (omdbResult.tomatoFresh != "N/A")
                    {
                        tvShow.TomatoFresh = Int32.Parse(omdbResult.tomatoFresh);
                    }
                    if (omdbResult.tomatoRotten != "N/A")
                    {
                        tvShow.TomatoRotten = Int32.Parse(omdbResult.tomatoRotten);
                    }
                    if (omdbResult.tomatoRating != "N/A")
                    {
                        tvShow.TomatoRating = Decimal.Parse(omdbResult.tomatoRating);
                    }
                    if (omdbResult.tomatoReviews != "N/A")
                    {
                        tvShow.TomatoReviews = Int32.Parse(omdbResult.tomatoReviews);
                    }
                    if (omdbResult.tomatoUserMeter != "N/A")
                    {
                        tvShow.TomatoUserMeter = Int32.Parse(omdbResult.tomatoUserMeter);
                    }
                    if (omdbResult.tomatoUserRating != "N/A")
                    {
                        tvShow.TomatoUserRating = Decimal.Parse(omdbResult.tomatoUserRating);
                    }
                    if (omdbResult.tomatoUserReviews != "N/A")
                    {
                        tvShow.TomatoUserReviews = Int32.Parse(omdbResult.tomatoUserReviews);
                    }
                    if (refresh)
                    {
                        baza.updateTVShow(tvShow);
                    }
                }
            }
            catch { }
            return tvShow;
        }

    }
}
