using Recycling.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recycling.Controllers
{
    public class ConstituentController : Controller
    {
        private readonly IConstituentRepository _constituentRepository;
        public ConstituentController(IConstituentRepository constituentRepository)
        {
            _constituentRepository = constituentRepository;
        }
        // GET: Constituent
        public ActionResult Index()
        {
            var constituents = _constituentRepository.GetAll().ToList();
            return View(constituents);
        }
    }
}