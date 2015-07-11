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
    public class ConstituentController : Controller
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;
        private readonly IProductHasConstituentRepository _productHasConstituentRepository;
        private readonly ILocatedInRepository _locatedInRepository;
        private readonly IConstituentRepository _constituentRepository;
        private readonly IRegionRepository _regionRepository;

        public ConstituentController(IConstituentRepository constituentRepository
            , IProductHasConstituentRepository productHasConstituentRepository
            , ILocatedInRepository locatedInRepository
            , IUnitOfWorkFactory unitOfWorkFactory
            , IRegionRepository regionRepository)
        {
            _constituentRepository = constituentRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
            _locatedInRepository = locatedInRepository;
            _productHasConstituentRepository = productHasConstituentRepository;
            _regionRepository = regionRepository;
        }
        // GET: Constituent
        public ActionResult Index()
        {
            var constituents = _constituentRepository.GetAll().ToList();
            return View(constituents);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Constituent constituent)
        {
            if (ModelState.IsValid)
            {
                using (var uow = _unitOfWorkFactory.UnitOfWork)
                {
                    _constituentRepository.SaveOrUpdate(constituent);
                    uow.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            return View(constituent);
        }

        public ActionResult Edit(int id)
        {
            var constituent = _constituentRepository.GetById(id);
            var LocatedIns = _locatedInRepository.Query.Where(x => x.Constituent == constituent).ToList();
            var LocatedInsDTO = new LocatedInDTO
            {
                Constituent = constituent,
                LocatedIns = LocatedIns
            };
            return View(LocatedInsDTO);
        }

        [HttpPost]
        public ActionResult Edit(Constituent constituent)
        {
            if (ModelState.IsValid)
            {
                var constituentdb = _constituentRepository.GetById(constituent.Id);
                AutoMapper.Mapper.Map<Constituent, Constituent>(constituent, constituentdb);
                _constituentRepository.SaveOrUpdate(constituentdb);
                return RedirectToAction("Details", new { id = constituentdb.Id });
            }
            return View(constituent);
        }

        public ActionResult Details(int id)
        {
            var constituent = _constituentRepository.GetById(id);
            return View(constituent);
        }

        public ActionResult Delete(int id)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                var constituent = _constituentRepository.GetById(id);
                var phascs = _productHasConstituentRepository.Query.Where(x => x.Constituent == constituent).ToList();
                foreach (var phasc in phascs)
                {
                    _productHasConstituentRepository.Delete(phasc);
                }

                var locatedIns = _locatedInRepository.Query.Where(x => x.Constituent == constituent).ToList();
                foreach (var locatedIn in locatedIns)
                {
                    _locatedInRepository.Delete(locatedIn);
                }
                _constituentRepository.Delete(constituent);
                uow.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult AddRecyclability(int cid)
        {
            ViewBag.Regions = _regionRepository.GetAll().ToList();
            var locatedIn = new LocatedIn
            {
                Constituent = _constituentRepository.GetById(cid)
            };
            return View(locatedIn);
        }

        [HttpPost]
        public ActionResult AddRecyclability(LocatedIn locatedIn)
        {
            var constituent = _constituentRepository.GetById(locatedIn.Constituent.Id);
            var region = _regionRepository.GetById(locatedIn.Region.Id);
            locatedIn.Region = region;
            locatedIn.Constituent = constituent;
            _locatedInRepository.SaveOrUpdate(locatedIn);
            return RedirectToAction("Edit", new { id = locatedIn.Constituent.Id });

        }

        public ActionResult EditRecyclability(int locatedInId)
        {
            var locatedIn = _locatedInRepository.GetById(locatedInId);

            return View(locatedIn);
        }

        [HttpPost]
        public ActionResult EditRecyclability(LocatedIn locatedIn)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                _locatedInRepository.SaveOrUpdate(locatedIn);
                uow.SaveChanges();
                return RedirectToAction("Edit", new { id = locatedIn.Constituent.Id });
            }
        }

        public ActionResult RemoveRecyclability(int locatedInId)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                var locatedIn = _locatedInRepository.GetById(locatedInId);
                _locatedInRepository.Delete(locatedIn);
                uow.SaveChanges();
                return RedirectToAction("Edit", new { id = locatedIn.Constituent.Id });
            }

        }
    }
}