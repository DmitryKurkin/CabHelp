namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        public HomeController()
        {
            if (ProjectRepository.Project == null)
            {
                ProjectRepository.Project = BuildDefaultProject();
            }
        }

        public ActionResult Index(string theothers)
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult CreateProject()
        {
            ProjectRepository.Project = BuildDefaultProject();

            return RedirectToAction("Index");
        }

        public ActionResult Output()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        private Project BuildDefaultProject()
        {
            var project = new Project
            {
                AppName = "App01",
                CompanyName = "Company01",
                InstallDirectory = "Folder01"
            };

            return project;
        }
    }
}