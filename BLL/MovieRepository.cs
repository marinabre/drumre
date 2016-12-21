using Facebook;
using OMDbSharp;
using OSDBnet;
using System;
using System.Linq;


namespace BLL
{
    public class MovieRepository
    {
        Baza baza = new Baza();

        //za dohvat omdb podataka o filmu, ako se flag stavi na true, znači da se updatea postojeći film
        public Movie OMDbData(Movie movie, bool refresh = false)
        {
            OMDbClient omdb = new OMDbClient(true);
            try
            {
                Item omdbResult = omdb.GetItemByID(movie.IMDbId).Result;
                if (omdbResult.imdbID != null)
                {
                    movie.Rated = omdbResult.Rated;
                    movie.Awards = omdbResult.Awards;
                    movie.Country = omdbResult.Country;
                    movie.Language = omdbResult.Language;

                    if (omdbResult.Metascore != "N/A")
                    {
                        movie.Metascore = Int32.Parse(omdbResult.Metascore);
                    }
                    if (omdbResult.tomatoFresh != "N/A")
                    {
                        movie.TomatoFresh = Int32.Parse(omdbResult.tomatoFresh);
                    }
                    if (omdbResult.tomatoRotten != "N/A")
                    {
                        movie.TomatoRotten = Int32.Parse(omdbResult.tomatoRotten);
                    }
                    if (omdbResult.tomatoRating != "N/A")
                    {
                        movie.TomatoRating = Decimal.Parse(omdbResult.tomatoRating);
                    }
                    if (omdbResult.tomatoReviews != "N/A")
                    {
                        movie.TomatoReviews = Int32.Parse(omdbResult.tomatoReviews);
                    }
                    if (omdbResult.tomatoUserMeter != "N/A")
                    {
                        movie.TomatoUserMeter = Int32.Parse(omdbResult.tomatoUserMeter);
                    }
                    if (omdbResult.tomatoUserRating != "N/A")
                    {
                        movie.TomatoUserRating = Decimal.Parse(omdbResult.tomatoUserRating);
                    }
                    if (omdbResult.tomatoUserReviews != "N/A")
                    {
                        movie.TomatoUserReviews = Int32.Parse(omdbResult.tomatoUserReviews);
                    }
                    if (refresh)
                    {
                        baza.updateMovie(movie);
                    }
                }
            }
            catch { }
            return movie;
        }

        public Movie SubtitleData(Movie movie, bool refresh = false)
        {
            try
            {
                var osdb = Osdb.Login("eng", "FileBot");
                var subtitles = osdb.SearchSubtitlesFromImdb("eng", movie.IMDbId.Substring(2));
                if (subtitles.Count > 0)
                {
                    movie.MovieSubtitle = subtitles.First();
                    if (refresh)
                    {
                        baza.updateMovie(movie);
                    }

                }
                return movie;
            }
            catch (Exception e)
            {
                return movie;
            }
        }

        public static void FBData (Movie movie, bool refresh = false)
        {
            //var claimsforUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            //var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            //var fb = new FacebookClient(access_token);
            var fb = new FacebookClient();
            dynamic result = fb.Get("oauth/access_token", new
            {
                client_id = "1279180495466644",
                client_secret = "780860b63672ff262370699538722160",
                grant_type = "client_credentials"
            });
            String imdbURL = "http://www.imdb.com/title/" + movie.IMDbId;
            dynamic Info = fb.Get("?fields=og_object{ likes.limit(0).summary(true), engagement, title, id, image }, share &ids=" + imdbURL);
            Info = GetProperty(Info, imdbURL);
            movie.Title = Info.og_object.title;
            movie.FBLikes = Info.og_object.engagement.count;
            movie.FBShares = Info.share.share_count;
        }

        public static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }

    }
}
