using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Services;

namespace Recycling.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository
            , IUserService userService
           )
        {
            _userRepository = userRepository;
            _userService = userService;
        }
        //
        // GET: /User/
        public ActionResult Index()
        {
            var users = _userRepository.GetAll().ToList();
            return View(users);
        }

        //
        // GET: /User/Details/5
        public ActionResult Details(int id)
        {
            var user = _userRepository.GetById(id);
            return View(user);
        }

        //
        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create
        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var userExisted = _userRepository.Query.Where(u => u.Username == user.Username).FirstOrDefault();
                if (userExisted == null)
                {
                    //TODO: Add checking if user exists already.
                    user.Password = Crypto.HashPassword(user.Password);

                    _userRepository.SaveOrUpdate(user);
                    TempData["success"] = "A user is successfully added!";
                    return RedirectToAction("Error", "Home", new { Error = "Thank you, your account has been successfully created." });
                }
                else
                {
                    return RedirectToAction("Error", "Home", new { Error = "Your Email or Username has been taken, please try another one." });
                }

            }

            return View();
        }

        //
        // GET: /User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5
        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                var _user = _userRepository.Query.Where(u => u.Id == user.Id).FirstOrDefault();
                var updatedUser = AutoMapper.Mapper.Map(user, _user);

                _userRepository.SaveOrUpdate(updatedUser);

                return RedirectToAction("Details", new { id = user.Id });
            }

            return View();
        }

        //
        // GET: /User/Delete/5
        public ActionResult Delete(int id)
        {
           return RedirectToAction("Index");
        }

        public ActionResult Projects(int id)
        {
            return View();
        }

        public ActionResult AddProject(int id)
        {
            return View();
        }

        public ActionResult AddProjectToUser(int id)
        {
            var user = _userRepository.GetById((int)Session["UserId"]);
           return RedirectToAction("Projects", new { id = (int)Session["UserId"] });
        }

        public ActionResult DeleteRelation(int id, int userid)
        {
            return RedirectToAction("Projects", new { id = userid });
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection user)
        {
            var username = user["Username"];
            var password = user["Password"];

            if ((HttpContext.User != null) &&
                (HttpContext.User.Identity.IsAuthenticated))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if (_userService.isValid(username, password))
                {

                    var user_data = _userRepository.FindByUsername(username).FirstOrDefault();
                    var role = "";

                    role = user_data.Role;

                    Session["Role"] = role;
                    Session["UserID"] = user_data.Id;

                    if (user_data.Role != "Admin")
                    {
                    }

                    bool isPersistent = false;
                    string userData = role;
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                             username,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(30),
                             isPersistent,
                             userData,
                             FormsAuthentication.FormsCookiePath);
                    // Encrypt the ticket using the machine key
                    string encryptedTicket = FormsAuthentication.Encrypt(ticket);

                    // Add the cookie to the request to save it
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    // Your redirect logic
                    //_userService.Login(user.Username);
                    return RedirectToAction("Index", "Home");

                }
                else
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Role"] = null;
            _userService.Logout();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Warning()
        {
            return View();
        }
        public string IdentifyRole()
        {
            return HttpContext.User.Identity.Name;

        }

        public ActionResult ResetPassword()
        {
            ViewBag.IsUser = false;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(User user)
        {
            ViewBag.IsUser = false;
            User _user = _userRepository.Query.Where(u => u.Username == user.Username).FirstOrDefault();
            if (_user != null)
            {
                ViewBag.IsUser = true;
                ViewBag.Error = "";
                if (String.IsNullOrEmpty(_user.SecurityQuestion))
                {
                    return RedirectToAction("Error", "Home", new { error = " Your security question is not set, please contact administrator." });
                }
                else if (user.SecurityAnswer != _user.SecurityAnswer)
                {
                    ViewBag.Error = "Your answer is not correct, please try again";
                    return View(_user);
                }
                else if (user.SecurityAnswer == _user.SecurityAnswer)
                {
                    return RedirectToAction("ResetForm", new { Id = _user.Id });
                }
                else
                {
                    return View();
                }
            }

            return View(user);
        }

        public ActionResult ResetForm(int Id)
        {
            User user = _userRepository.GetById(Id);

            return View(user);
        }

        [HttpPost]
        public ActionResult ResetForm(User user)
        {
            if (!String.IsNullOrWhiteSpace(user.Password))
            {
                var _user = _userRepository.GetById(user.Id);
                _user.Password = user.Password;
                _user.Password = Crypto.HashPassword(user.Password);


                _userRepository.SaveOrUpdate(_user);
                TempData["success"] = "A user is successfully edited!";
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Home", new { error = "Your password is not valid, please enter again." });
        }


        public string IdentifyUser()
        {
            HttpCookie authCookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                return ticket.UserData;
            }
            return null;
        }
    }
}
