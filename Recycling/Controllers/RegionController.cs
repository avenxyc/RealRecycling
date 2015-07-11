using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Recycling.Controllers
{
    public class RegionController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IConstituentRepository _constituentRepository;
        private readonly IProductHasConstituentRepository _productHasConstituentRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly ILocatedInRepository _locatedInRepository;
        private readonly ICatetoryRepository _categoryRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public RegionController(IProductRepository productRepository
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

        // GET: Region
        public ActionResult Index()
        {
            var regions = _regionRepository.GetAll();
            return View(regions);
        }


        // GET: Region/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Region/Create
        [HttpPost]
        public ActionResult Create(Region region)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _regionRepository.SaveOrUpdate(region);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View(region);
            }
        }

        // GET: Region/Edit/5
        public ActionResult Edit(int id)
        {
            var region = _regionRepository.GetById(id);
            return View(region);
        }

        // POST: Region/Edit/5
        [HttpPost]
        public ActionResult Edit(Region region)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var regiondb = _regionRepository.GetById(region.Id);
                    AutoMapper.Mapper.Map<Region, Region>(region, regiondb);
                    _regionRepository.SaveOrUpdate(regiondb);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Region/Delete/5
        public ActionResult Delete(int id)
        {
            var region = _regionRepository.GetById(id);
            var locatedIns = _locatedInRepository.Query.Where(x => x.Region == region).ToList();
            foreach(var locatedIn in locatedIns)
            {
                _locatedInRepository.Delete(locatedIn);
            }
            _regionRepository.Delete(region);
            return RedirectToAction("Index");
        }

    }
}
