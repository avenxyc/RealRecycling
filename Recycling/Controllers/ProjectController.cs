using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Recycling.Domain.Models;
using Recycling.Domain.Repository;
using Recycling.Domain.Services;

namespace Recycling.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserProjectRepository _userProjectRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IFileLogRepository _fileLogRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ProjectController(ICompanyRepository companyRepository
            , IUserRepository userRepository
            , IProjectRepository projectRepository
            , IUserProjectRepository userProjectRepository
            , IFileLogRepository fileLogRepository
            , IFileRepository fileRepository
            , IUnitOfWorkFactory unitOfWorkFactory)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _userProjectRepository = userProjectRepository;
            _projectRepository = projectRepository;
            _fileLogRepository = fileLogRepository;
            _fileRepository = fileRepository;
            _unitOfWorkFactory = unitOfWorkFactory;
        }
        //
        // GET: /Project/
        public ActionResult Index()
        {
            var projects = _projectRepository.GetAll().ToList();
            return View(projects);
        }

        //
        // GET: /Project/Details/5
        public ActionResult Details(int id)
        {
            var project = _projectRepository.GetById(id);
            ViewBag.UserList = _userProjectRepository.Query.Where(p => p.Project.Id == id).ToList();
            ViewBag.FileList = _fileRepository.Query.Where(p => p.Project.Id == id).ToList();
            return View(project);
        }

        //
        // GET: /Project/Create
        public ActionResult Create()
        {
            var Companies = _companyRepository.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CompanyName
            });
            ViewBag.Companies = Companies;

            return View();
        }

        //
        // POST: /Project/Create
        [HttpPost]
        public ActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Status = "Created";
                _projectRepository.SaveOrUpdate(project);
                return RedirectToAction("Details", new { id = project.Id });
            }

            return View();
        }

        //
        // GET: /Project/Edit/5
        public ActionResult Edit(int id)
        {
            var Companies = _companyRepository.GetAll().ToList().Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.CompanyName
            });
            ViewBag.Companies = Companies;
            var project = _projectRepository.GetById(id);
            return View(project);
        }

        //
        // POST: /Project/Edit/5
        [HttpPost]
        public ActionResult Edit(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Status = "Edited";
                var projectdb = _projectRepository.GetById(project.Id);
                AutoMapper.Mapper.Map(project, projectdb);
                _projectRepository.SaveOrUpdate(projectdb);
                return RedirectToAction("Details", new { id = project.Id });
            }

            return View();
        }

        //
        // GET: /Project/Delete/5
        public ActionResult Delete(int id)
        {
            var relations = _userProjectRepository.Query.Where(p => p.Id == id).ToList();
            foreach (var relation in relations)
            {
                _userProjectRepository.Delete(relation);
            };

            var files = _fileRepository.Query.Where(f => f.Project.Id == id).ToList();
            foreach (var file in files)
            {
                if (file != null)
                {
                    string fullPath = Request.MapPath("~/UploadedFiles/" + file.FileName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    var filelogs = _fileLogRepository.Query.Where(f => f.Files.Id == id).ToList();
                    foreach (var log in filelogs)
                    {
                        _fileLogRepository.Delete(log);
                    }

                    _fileRepository.Delete(file);
                }
            }

            var project = _projectRepository.GetById(id);
            _projectRepository.Delete(project);
            return View();
        }

        //
        // GET: /Project/DeleteFile/5
        public ActionResult DeleteFile(int id)
        {
            var file = _fileRepository.GetById(id);
            
            if (file != null)
            {
                string fullPath = Request.MapPath("~/UploadedFiles/" + file.FileName);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                var filelogs = _fileLogRepository.Query.Where(f => f.Files.Id == id).ToList();
                foreach (var log in filelogs)
                {
                    _fileLogRepository.Delete(log);
                }

                _fileRepository.Delete(file);
            }


            return RedirectToAction("Details", new { id = file.Project.Id });
        }

        [HttpPost]
        public ActionResult UploadFiles(Files newfile, FormCollection form, int pid = 0)
        {
            using (var uow = _unitOfWorkFactory.UnitOfWork)
            {
                // Save the uploaded file
                HttpPostedFileBase file = Request.Files["uploadFile"];
                newfile.FileName = file.FileName;
                if ((file != null) && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];
                    newfile.FilePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);
                    newfile.Description = form["description"];
                    file.SaveAs(newfile.FilePath);
                }

                var project = _projectRepository.GetById(pid);
                newfile.Project = project;
                _fileRepository.SaveOrUpdate(newfile);
                var newfilelog = new FileLog()
                {
                    User = _userRepository.GetById((int)Session["UserID"]),
                    Date = DateTime.Now,
                    Status = "Uploaded",
                    Project = project,
                    Files = newfile,
                    Note = newfile.FileName + " is uploaded to " + project.ProjectName
                };

                _fileLogRepository.SaveOrUpdate(newfilelog);

                uow.SaveChanges();
                return RedirectToAction("Details", new { id = pid });
            }
        }

        public ActionResult UserProject()
        {
            if (Session["UserID"] != null)
            {
                var userId = (int)Session["UserID"];
                var projects = _userProjectRepository.Query.Where(p => p.User.Id == userId).ToList();
                return View(projects);
            }

            else return RedirectToAction("Index", "Home", null);
           
        }
    }
}
