namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common.Entities;
    using System.Linq;
    using System.Web.Mvc;

    public class StringDefinitionsController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult Create()
        {
            return View("Edit", new StringDef());
        }

        [HttpGet]
        public ActionResult Edit(int defId)
        {
            var strDef = ProjectRepository.Project.StringDefs.Defs.Single(sd => sd.DefId == defId);

            return View(strDef);
        }

        [HttpPost]
        public ActionResult Edit(StringDef postedDefinition)
        {
            var strDef = ProjectRepository.Project.StringDefs.Defs.SingleOrDefault(sd => sd.DefId == postedDefinition.DefId);

            if (strDef != null)
            {
                strDef.Key = postedDefinition.Key;
                strDef.Value = postedDefinition.Value;
            }
            else
            {
                ProjectRepository.Project.StringDefs.AddStringDef(postedDefinition.Key, postedDefinition.Value);
            }

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int defId)
        {
            ProjectRepository.Project.StringDefs.RemoveStringDef(defId);

            return RedirectToAction("Index");
        }
    }
}