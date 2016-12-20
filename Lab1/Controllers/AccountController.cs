using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Projekt.Models;
using Facebook;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using Projekt.Controllers;
using MongoDB.Driver;
using OMDbSharp;
using OSDBnet;
using System.Configuration;
using MongoDB.Bson;
using Projekt.App_Start;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using BLL;

namespace Projekt.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                //Save the FacebookToken in the database if not already there
                await StoreFacebookAuthToken(user);
                await SignInManager.ExternalSignInAsync (loginInfo, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }

            // Sign in the user with this external login provider if the user already has a login
            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            //switch (result)
            //{
            //    case SignInStatus.Success:
            //        return RedirectToLocal(returnUrl);
            //    case SignInStatus.LockedOut:
            //        return View("Lockout");
            //    case SignInStatus.RequiresVerification:
            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
            //    case SignInStatus.Failure:
            //    default:
            //        // If the user does not have an account, then prompt the user to create an account
            //        ViewBag.ReturnUrl = returnUrl;
            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
            //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            //}
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Manage");
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await StoreFacebookAuthToken(user);
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }


        //GET: Account/FacebookInfo
        [Authorize]
        public async Task<ActionResult> FacebookInfo()
        {

            //Open connection to database:
            var db = MongoInstance.GetDatabase;

            var pack = new ConventionPack();
            pack.Add(new IgnoreExtraElementsConvention(true));
            ConventionRegistry.Register("ignore extra elements", pack, t => true);

            //Prepare database collections:
            var persons = db.GetCollection<Person>("Person");

            //Get FacebookAccessToken from ExternalCookie and use it to get user information from Facebook:
            var claimsforUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            var fb = new FacebookClient(access_token);
            //Specify what info we are getting from Facebook:
            dynamic myInfo = fb.Get("me?fields=id,email,first_name,last_name,birthday,movies,video.watches,video.wants_to_watch");

            //Process results:
            IList<BLL.FBMovie> movieList = new List<BLL.FBMovie>();
            IList<BLL.FBMovie> watchesList = new List<BLL.FBMovie>();
            IList<BLL.FBMovie> wantsList = new List<BLL.FBMovie>();
            IList<BLL.MovieDetails> detailedMovieList = new List<BLL.MovieDetails>();

            //dohvat i spremanje kategorije Likes:
            foreach (dynamic fbMovie in myInfo.movies.data)
            {
                BLL.FBMovie movie = new BLL.FBMovie()
                {
                    Title = fbMovie.name,
                    Id = fbMovie.id
                };
                movieList.Add(movie);
                detailedMovieList.Add(AddToDbIfNotExist(movie));
            }

            //dohvat i spremanje kategorije Watched:
            try
            {
                dynamic watches = GetProperty(myInfo, "video.watches");
                foreach (dynamic fbMovie in watches.data)
                {
                    BLL.FBMovie movie = new BLL.FBMovie()
                    {
                        Title = fbMovie.data.movie.title,
                        Id = fbMovie.data.movie.id
                    };
                    watchesList.Add(movie);
                    detailedMovieList.Add(AddToDbIfNotExist(movie));
                }
            } catch (RuntimeBinderException)
            {
                
            }


            //dohvat i spremanje kategorije Wants To Watch:
            try
            {
                dynamic wants = GetProperty(myInfo, "video.wants_to_watch");
                foreach (dynamic fbMovie in wants.data)
                {
                    BLL.FBMovie movie = new BLL.FBMovie()
                    {
                        Title = fbMovie.data.movie.title,
                        Id = fbMovie.data.movie.id
                    };
                    wantsList.Add(movie);
                    detailedMovieList.Add(AddToDbIfNotExist(movie));
                }
            } catch (RuntimeBinderException)
            {
                
            }

            Person me = new Person
            {
                Name = myInfo.first_name,
                Surname = myInfo.last_name,
                Birthday = DateTime.ParseExact(myInfo.birthday + " 08:00", "MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture),
                LikedMovies = movieList,
                Watches = watchesList,
                Wants = wantsList,
                Email = myInfo.email
            };
            //persons.InsertOne(me);
            persons.ReplaceOne(p => p.Email == me.Email,
                me,
                new UpdateOptions { IsUpsert = true });

            //testiranje dohvata informacija o popularnosti s fejsa. Ovaj dio koda tu nikako ne pripada:
            var muviz = db.GetCollection<BLL.Movie>("Muviz");
            BLL.Movie testni = new BLL.Movie();
            refreshMovieInfoFromFB(testni, "http://www.imdb.com/title/tt0068646");
            muviz.InsertOne(testni);




            //return View(detailedMovieList);
            return RedirectToAction("Index", "Home");
        }

        public async void refreshMovieInfoFromFB(BLL.Movie Movie, string imdbURL)
        {
            var claimsforUser = await UserManager.GetClaimsAsync(User.Identity.GetUserId());
            var access_token = claimsforUser.FirstOrDefault(x => x.Type == "FacebookAccessToken").Value;
            var fb = new FacebookClient(access_token);
            dynamic Info = fb.Get("?fields=og_object{ likes.limit(0).summary(true), engagement, title, id, image }, share &ids=" + imdbURL);
            Info = GetProperty(Info, imdbURL);
            Movie.Title = Info.og_object.title;
            Movie.FBLikes = Info.og_object.engagement.count;
            Movie.FBShares = Info.share.share_count;
        }

        public static object GetProperty(object target, string name)
        {
            var site = System.Runtime.CompilerServices.CallSite<Func<System.Runtime.CompilerServices.CallSite, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.GetMember(0, name, target.GetType(), new[] { Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo.Create(0, null) }));
            return site.Target(site, target);
        }

        public BLL.MovieDetails AddToDbIfNotExist(FBMovie movie)
        {
            //Open connection to database:
            var db = MongoInstance.GetDatabase;

            //Initialize OMDb and OSDb clients:
            OMDbClient omdb = new OMDbClient(true);
            var osdb = Osdb.Login("eng", "FileBot");

            //Prepare database collections:
            var movies = db.GetCollection<Item>("Movies");
            var subtitles = db.GetCollection<Subtitle>("Subtitles");
            //If movie is not in the database, add it and the subtitles

            Item dbMovie = null;
            Subtitle dbSubtitle = null;
            var mquery = movies.Find(p => p.Title == movie.Title);
            if (mquery.Count() > 0)
            {
                dbMovie = mquery.First();
                var squery = subtitles.Find(p => p.MovieName == movie.Title);
                if (squery.Count() > 0)
                {
                    dbSubtitle = squery.First();
                }

            }

            if (dbMovie == null)
            {
                Item omdbResult = omdb.GetItemByTitle(movie.Title).Result;
                if (omdbResult != null)
                {
                    movies.ReplaceOne(p => p.Title == omdbResult.Title,
                        omdbResult,
                        new UpdateOptions { IsUpsert = true });
                    dbMovie = omdbResult;

                    var squery = subtitles.Find(p => p.MovieName == movie.Title);
                    if (squery.Count() > 0)
                    {
                        dbSubtitle = squery.First();
                    }
                    if (dbSubtitle == null)
                    {
                        var osdbResult = osdb.SearchSubtitlesFromQuery("eng", movie.Title);
                        if (osdbResult.Count > 0)
                        {
                            subtitles.ReplaceOne(p => p.SubtitleId == osdbResult.First().SubtitleId,
                                osdbResult.First(),
                                new UpdateOptions { IsUpsert = true });
                            dbSubtitle = osdbResult.First();
                        }
                    }
                }
            }

            Uri link = null;
            if (dbSubtitle != null)
            {
                link = dbSubtitle.SubTitleDownloadLink;
            }
            return new BLL.MovieDetails
            {
                item = dbMovie,
                downloadLink = link
            };
        }


        //GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = Projekt.Controllers.ManageController.ManageMessageId.Error });
            }
            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                var currentUser = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                //Add the Facebook Claim
                await StoreFacebookAuthToken(currentUser);
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = Projekt.Controllers.ManageController.ManageMessageId.Error });
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        private async Task StoreFacebookAuthToken(ApplicationUser user)
        {
            var claimsIdentity = await AuthenticationManager.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
            if (claimsIdentity != null)
            {
                // Retrieve the existing claims for the user and add the FacebookAccessTokenClaim
                var currentClaims = await UserManager.GetClaimsAsync(user.Id);
                var facebookAccessToken = claimsIdentity.FindAll("FacebookAccessToken").First();
                if (currentClaims.Count() <= 0)
                {
                    await UserManager.AddClaimAsync(user.Id, facebookAccessToken);
                }
            }
        }



        #endregion
    }
}