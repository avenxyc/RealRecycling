using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Domain.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;

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

        // GET: Product/Create
        public ActionResult Create()
        {
            var Categories = _categoryRepository.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            });
            ViewData["Category"] = Categories.OrderBy(x => x.Text);
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
                            var path = "pics/" + productdto.product.UPC + fileExtension;
                            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/") + path;
                            file.SaveAs(dirPath);
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

                        };

                        var newLocatedIn = new LocatedIn
                        {
                            Constituent = constituent,
                            Region = _regionRepository.Query.Where(x => x.RegionName == productdto.region.RegionName).FirstOrDefault(),
                            Classification = cclassifications[i],
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
            var Categories = _categoryRepository.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CategoryName
            });
            ViewData["Category"] = Categories.OrderBy(x => x.Text);
            var productdb = _productRepository.GetById(id);
            var productHasConstituents = _productHasConstituentRepository.Query.Where(x => x.Product == productdb).ToList();
            var constituentsdb = new List<Constituent>();
            foreach (var pHc in productHasConstituents)
            {
                constituentsdb.Add(_constituentRepository.GetById(pHc.Constituent.Id));
            }


            var productDto = new ProductDTO
            {
                product = productdb,
                constituents = constituentsdb,
                pHasCs = productHasConstituents
            };
            return View(productDto);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductDTO productdto, FormCollection collection)
        {
            try
            {
                using (var uow = _unitOfWorkFactory.UnitOfWork)
                {
                    productdto.product.Category = _categoryRepository.GetById(Int32.Parse(collection["CategoryId"]));
                    HttpPostedFileBase file = Request.Files["ProductImage"];
                    var productdb = _productRepository.GetById(productdto.product.Id);
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
                            var path = "pics/" + productdto.product.UPC + fileExtension;
                            string dirPath = System.Web.HttpContext.Current.Server.MapPath("~/") + path;
                            if (System.IO.File.Exists(dirPath))
                            {
                                System.IO.File.Delete(dirPath);
                                file.SaveAs(path);
                            }
                            else
                            {
                                file.SaveAs(dirPath);
                            }

                            productdto.product.ImagePath = path;
                        }
                        productdto.product.LastUpdated = DateTime.Now;
                    }
                    else if (productdb.ImagePath != null)
                    {
                        productdto.product.ImagePath = productdb.ImagePath;
                    }


                    AutoMapper.Mapper.Map(productdto.product, productdb);
                    _productRepository.SaveOrUpdate(productdb);

                    uow.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult AddConstituent(int pid)
        {
            var constituents = _constituentRepository.GetAll().ToList();
            ViewBag.Constituent = constituents.OrderBy(x => x.ConstituentName);
            var product = _productRepository.GetById(pid);
            var phasc = new ProductHasConstituent
            {
                Product = product
            };
            return View(phasc);
        }

        [HttpPost]
        public ActionResult AddConstituent(ProductHasConstituent phasc)
        {
            if (phasc.PartWeight > 0)
            {
                var constituent = _constituentRepository.GetById(phasc.Constituent.Id);
                phasc.Constituent = constituent;
                var product = _productRepository.GetById(phasc.Product.Id);
                phasc.Product = product;
                _productHasConstituentRepository.SaveOrUpdate(phasc);
                return RedirectToAction("Edit", new { id = phasc.Product.Id });
            }
            var constituents = _constituentRepository.GetAll().ToList();
            ViewBag.Constituent = constituents.OrderBy(x => x.ConstituentName);
            return View(phasc);
        }

        public ActionResult EditConstituent(int pHascId)
        {
            var pHasc = _productHasConstituentRepository.GetById(pHascId);

            return View(pHasc);
        }

        [HttpPost]
        public ActionResult EditConstituent(ProductHasConstituent phasc)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                var phascDb = _productHasConstituentRepository.GetById(phasc.Id);
                phascDb.PartWeight = phasc.PartWeight;
                _productHasConstituentRepository.SaveOrUpdate(phascDb);

                uow.SaveChanges();
                return RedirectToAction("Edit", new { id = phascDb.Product.Id });
            }
        }

        public ActionResult RemoveConstituent(int pHascId)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                var pHascDb = _productHasConstituentRepository.GetById(pHascId);
                if (pHascDb != null)
                {
                    _productHasConstituentRepository.Delete(pHascDb);
                }

                uow.SaveChanges();
                return RedirectToAction("Edit", new { id = pHascDb.Product.Id });
            }

        }

        public ActionResult Details(int id)
        {
            var productHasconstituents = _productHasConstituentRepository.Query.Where(x => x.Product.Id == id).ToList();
            var region = _regionRepository.Query.Where(x => x.RegionName == Session["regionName"].ToString()).FirstOrDefault();
            var locatedIn = new List<LocatedIn> { };


            foreach (var constituent in productHasconstituents)
            {
                var recyclability = _locatedInRepository.Query.Where(x => x.Constituent.Id == constituent.Id && x.Region.Id == region.Id).FirstOrDefault();
                locatedIn.Add(recyclability);
            }

            var productdto = new ProductDTO
            {
                pHasCs = productHasconstituents,
                recyclabilities = locatedIn,
                product = productHasconstituents.FirstOrDefault().Product
            };



            return View(productdto);
        }

        public ActionResult Delete(int id)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                var product = _productRepository.GetById(id);
                var phascs = _productHasConstituentRepository.Query.Where(x => x.Product == product);
                foreach (var phasc in phascs)
                {
                    _productHasConstituentRepository.Delete(phasc);
                }

                _productRepository.Delete(product);

                uow.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public JsonResult GetConstituents(int id)
        {
            var constituents = new List<ConstituentDTO> { };
            // instantiate a serializer
            foreach (var phasc in _productHasConstituentRepository.Query.Where(x => x.Product.Id == id))
            {
                var constituent = new ConstituentDTO
                {
                    CName = phasc.Constituent.ConstituentName,
                    PartWeight = phasc.PartWeight
                };
                constituents.Add(constituent);
            }

            return Json(constituents, JsonRequestBehavior.AllowGet);
        }

        public class ConstituentDTO
        {
            public string CName { get; set; }
            public double PartWeight { get; set; }
        }
    }
}
