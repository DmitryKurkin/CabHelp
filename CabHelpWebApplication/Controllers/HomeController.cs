namespace CabHelpWebApplication.Controllers
{
    using System;
    using System.IO;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using CabHelpWebApplication.Models;
    using Emerson.Common;

    public class HomeController : Controller
    {
        private static Project BuildDefaultProject()
        {
            var project = new Project
            {
                AppName = "App01",
                CompanyName = "Company01",
                InstallDirectory = "Folder01"
            };

            return project;
        }

        public HomeController()
        {
            if (ProjectRepository.Project == null)
            {
                ProjectRepository.Project = BuildDefaultProject();
            }
        }

        public ActionResult Index(string theothers)
        {
            return View(new ProjectViewModel {Project = ProjectRepository.Project});
        }

        public ActionResult CreateProject()
        {
            ProjectRepository.Project = BuildDefaultProject();

            return RedirectToAction("Index");
        }

        public ActionResult Output()
        {
            return View(new ProjectViewModel {Project = ProjectRepository.Project});
        }

        public async Task<ActionResult> SaveProject()
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream) {AutoFlush = true})
            {
                await streamWriter.WriteAsync(ProjectPersist.Save(ProjectRepository.Project));
                return File(memoryStream.ToArray(), MediaTypeNames.Application.Octet, "Project.json");
            }
        }

        [HttpPost]
        public ActionResult LoadProject(HttpPostedFileBase postedFile)
        {
            try
            {
                using (var streamReader = new StreamReader(postedFile.InputStream))
                {
                    ProjectRepository.Project = ProjectPersist.Load(streamReader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
            }

            return RedirectToAction("Index");
        }
    }
}