using Recycling.Domain.Repository;
using System;
using System.Web.Mvc;

namespace Recycling.Controllers
{
    public class HomeController : Controller
    {

        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public ActionResult Index()
        {
            // By default set the region to be HRM
            Session["regionName"] = "HRM";
            return View();
        }


        public ActionResult Error(string error)
        {
            return View((Object)error);
        }
    }
}