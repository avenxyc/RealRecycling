using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recycling.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IConstituentRepository _constituentRepository;
        private readonly IProductHasConstituentRepository _productHasConstituentRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ILocatedInRepository _locatedInRepository;
        private readonly ICatetoryRepository _categoryRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;


        public ProductController(IProductRepository productRepository
            , IConstituentRepository constituentRepository
            , IProductHasConstituentRepository productHasConstituentRepository
            , IRegionRepository regionRepository
            , ILocatedInRepository locatedInRepository
            , ICatetoryRepository categoryRepository
            , IUnitOfWorkFactory unitOfWorkFactory)
        {
            _productRepository = productRepository;
            _constituentRepository = constituentRepository;
            _productHasConstituentRepository = productHasConstituentRepository;
            _regionRepository = regionRepository;
            _locatedInRepository = locatedInRepository;
            _categoryRepository = categoryRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        // GET: Product
        public ActionResult Index()
        {
            var products = _productRepository.GetAll();
            var constituents = _constituentRepository.GetAll();
            var phasc = _productHasConstituentRepository.GetAll();
            var regions = _regionRepository.GetAll();
            var locatedIn = _locatedInRepository.GetAll();
            var categories = _categoryRepository.GetAll();
            return View(products);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = _productRepository.GetById(id);
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var Categories = _categoryRepository.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            });
            ViewBag.Categories = Categories.OrderBy(x => x.Text);
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductDTO productdto, FormCollection collection)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.UnitOfWork)
                {
                    productdto.product.Category = _categoryRepository.GetById(Int32.Parse(collection["CategoryId"]));
                    HttpPostedFileBase file = Request.Files["ProductImage"];
                    // Verify that the user selected a file
                    if (file != null && file.ContentLength > 0)
                    {
                        if (file.ContentType == "image/jpeg" ||
                            file.ContentType == "image/jpg" ||
                            file.ContentType == "image/png")
                        {
                            // extract only the fielname
                            var fileExtension = Path.GetExtension(file.FileName);
                            // store the file inside ~/App_Data/uploads folder
                            var path = Path.Combine(Server.MapPath("/Content/Images/Uploaded"), productdto.product.UPC + fileExtension);
                            file.SaveAs(path);
                            productdto.product.ImagePath = path;
                        }

                    }

                    productdto.product.LastUpdated = DateTime.Now;

                    string[] cnames = collection["cform[cname][]"].Split(',');
                    string[] cpweights = collection["cform[pweight][]"].Split(',');
                    string[] cType = collection["cform[Type][]"].Split(',');
                    string[] cclassifications = collection["cform[classification][]"].Split(',');


                    for (int i = 0; i < cnames.Length; i++)
                    {
                        Constituent constituent = _constituentRepository.Query.Where(x => x.ConstituentName.Equals(cnames[i])).FirstOrDefault();
                        if (constituent == null)
                        {
                           constituent = new Constituent
                           {
                               ConstituentName = cnames[i]
                           };
                        };


                        var newPhasC = new ProductHasConstituent
                        {
                            Product = productdto.product,
                            PartWeight = Double.Parse(cpweights[i]),
                            Constituent = constituent,
                            ConstituentName = cnames[i],
                            UPC = productdto.product.UPC

                        };

                        var newLocatedIn = new LocatedIn
                        {
                            Constituent = constituent,
                            Region = _regionRepository.Query.Where(x => x.RegionName == productdto.region.RegionName).FirstOrDefault(),
                            Classification = cclassifications[i],
                            ConstituentName = constituent.ConstituentName
                        };

                        _productRepository.SaveOrUpdate(productdto.product);
                        _constituentRepository.SaveOrUpdate(constituent);
                        _productHasConstituentRepository.SaveOrUpdate(newPhasC);
                        _locatedInRepository.SaveOrUpdate(newLocatedIn);
                    }



                    uow.SaveChanges();
                    return RedirectToAction("Index");

                }

            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
