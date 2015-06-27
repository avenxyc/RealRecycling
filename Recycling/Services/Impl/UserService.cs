using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc.Filters;
using System.Web.Security;
using Recycling.Domain.Repository;

namespace Recycling.Services.Impl
{

    public class UserService : IUserService
    {
        #region Implementation of IUserService

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool isValid(string _username, string _password)
        {
            var userList = _userRepository.GetAll().ToList();


            foreach (var user in userList)
            {
                if (user.Username == _username)
                {
                    if (Crypto.VerifyHashedPassword(user.Password, _password))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public void Login(string username)
        {
           
            FormsAuthentication.SetAuthCookie(username, false);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public bool IsLoggedIn()
        {
            var context = HttpContext.Current;
            return context != null && context.User != null && context.User.Identity != null &&
                   context.User.Identity.IsAuthenticated;
        }

        public string GetLoggedInUsername()
        {
            return HttpContext.Current.User.Identity.Name;
        }

        #endregion

    }
}