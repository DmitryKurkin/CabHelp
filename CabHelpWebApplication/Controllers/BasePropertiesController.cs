namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common;
    using System.Web.Mvc;

    public class BasePropertiesController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        [HttpGet]
        public ActionResult Edit()
        {
            return View(ProjectRepository.Project);
        }

        [HttpPost]
        public ActionResult Edit(Project postedProject)
        {
            ProjectRepository.Project.AppName = postedProject.AppName;
            ProjectRepository.Project.CompanyName = postedProject.CompanyName;
            ProjectRepository.Project.InstallDirectory = postedProject.InstallDirectory;

            return RedirectToAction("Index");
        }

        public ActionResult Discard()
        {
            return RedirectToAction("Index");
        }
    }
}