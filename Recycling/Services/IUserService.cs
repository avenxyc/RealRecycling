using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Recycling.Services
{
    public interface IUserService
    {
        bool isValid(string _username, string _password);
        void Login(string _username);
        void Logout();
        bool IsLoggedIn();
        string GetLoggedInUsername();
    }
}