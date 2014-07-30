using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Security;
using NLog;
using services.Models;
using services.Resources;

namespace services.Controllers
{
    public class AccountResult
    {
        public bool Success = false;
        public string Message = "";
        public User User = null;
    }

    public class AccountController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public const String LOCAL_USER_AUTH = "LOCAL_AUTH";
        
        [HttpGet]
        public AccountResult Logout()
        {
            FormsAuthentication.SignOut();
            AccountResult result = new AccountResult();
            result.Success = true;
            result.Message ="Successfully logged out.";
            return result ;
        }

        [HttpPost]
        public AccountResult Login(LoginModel model)
        {
            //string result = "{\"message\": \"Failure'\"}";
            AccountResult result = new AccountResult();
            
            var resp = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.OK);

            logger.Debug("Hit: Login - " + model.Username + " / <SECRET>");

            var db = ServicesContext.Current;

            if (ModelState.IsValid)
            {
                var user = db.User.SingleOrDefault(x => x.Username == model.Username);

                if (Membership.ValidateUser(model.Username, model.Password) || isValidLocalUser(user, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, true);
                    logger.Debug("User authenticated : " + model.Username);
                    logger.Debug("--> " + System.Web.HttpContext.Current.Request.LogonUserIdentity.Name);

                    if (user == null) //If user doesn't exist in our system, create it.
                    {
                        user = new User(model.Username);
                        user.BumpLastLoginDate();
                        db.User.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        user.BumpLastLoginDate();
                        db.Entry(user).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                    }

                    var identity = new GenericIdentity(user.Username, "Basic");
                    string[] roles = (!String.IsNullOrEmpty(user.Roles)) ? user.Roles.Split(":".ToCharArray()) : new string[0];

                    logger.Debug("Roles == " + roles.ToString());

                    var principal = new GenericPrincipal(identity, roles);
                    Thread.CurrentPrincipal = principal;
                    System.Web.HttpContext.Current.User = principal;

                    result.Success = true;
                    result.User = user;
                    result.Message = "Successfully logged in.";

                }
                else
                {
                    logger.Debug("Authentication Failed from Membership provider.  Attempted username: " + model.Username);
                    result.Success = false;
                    result.Message = "Username or password were invalid.";
                }
            }
            else
                logger.Debug("model state invalid.");

            logger.Debug("Result = " + result);

            //NOTE: this is necessary because IE doesn't handle json returning from a POST properly.
            //resp.Content = new System.Net.Http.StringContent(result, System.Text.Encoding.UTF8, "text/plain");

            return result;
        }

        private bool isValidLocalUser(Models.User user, string password)
        {
            if (user == null)
            {
                logger.Debug("User not found");
                return false;
            }

            var salt = System.Configuration.ConfigurationManager.AppSettings["PasswordSalt"]; //in web.config
            logger.Debug("salt: " + salt);

            UserPreference local_auth = user.UserPreferences.Where(o => o.Name == LOCAL_USER_AUTH).SingleOrDefault();
            if (local_auth != null && MD5Util.VerifyMd5Hash(password + salt, local_auth.Value))
                return true;

            return false;

        }

    }
}
