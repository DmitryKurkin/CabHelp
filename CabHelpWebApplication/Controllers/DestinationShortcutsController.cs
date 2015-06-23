namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common.Entities;
    using Emerson.Common.Sections;
    using System.Linq;
    using System.Web.Mvc;

    public class DestinationShortcutsController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult CreateSection()
        {
            return View("EditSection", new ShortcutListSection());
        }

        [HttpGet]
        public ActionResult EditSection(int sectionId)
        {
            var section = ProjectRepository.Project.ShortcutListSections.Single(scls => scls.SectionId == sectionId);

            return View(section);
        }

        [HttpPost]
        public ActionResult EditSection(ShortcutListSection section)
        {
            var existingSection = ProjectRepository.Project.ShortcutListSections.SingleOrDefault(scls => scls.SectionId == section.SectionId);

            if (existingSection != null)
            {
                existingSection.Name = section.Name;
            }
            else
            {
                ProjectRepository.Project.AddShortcutListSection(section.Name);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteSection(int sectionId)
        {
            ProjectRepository.Project.RemoveShortcutListSection(sectionId);

            return RedirectToAction("Index");
        }

        public ActionResult CreateShortcut(int sectionId)
        {
            var section = ProjectRepository.Project.ShortcutListSections.Single(scls => scls.SectionId == sectionId);

            var viewModel = new ShortcutViewModel
            {
                Shortcut = new Shortcut(),
                ParentSectionId = sectionId
            };

            return View("EditShortcut", viewModel);
        }

        [HttpGet]
        public ActionResult EditShortcut(int sectionId, int shortcutId)
        {
            var section = ProjectRepository.Project.ShortcutListSections.Single(scls => scls.SectionId == sectionId);

            var shortcut = section.Shortcuts.Single(sc => sc.ShortcutId == shortcutId);

            var viewModel = new ShortcutViewModel
            {
                Shortcut = shortcut,
                ParentSectionId = sectionId
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditShortcut(Shortcut shortcut, int parentSectionId)
        {
            var section = ProjectRepository.Project.ShortcutListSections.Single(scls => scls.SectionId == parentSectionId);

            var existingShortcut = section.Shortcuts.SingleOrDefault(sc => sc.ShortcutId == shortcut.ShortcutId);

            if (existingShortcut != null)
            {
                existingShortcut.Name = shortcut.Name;
                existingShortcut.Type = shortcut.Type;
                existingShortcut.TargetPath = shortcut.TargetPath;
                existingShortcut.StandardDestinationPath = shortcut.StandardDestinationPath;
            }
            else
            {
                section.AddShortcut(shortcut.Name, shortcut.Type, shortcut.TargetPath, shortcut.StandardDestinationPath);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteShortcut(int sectionId, int shortcutId)
        {
            var section = ProjectRepository.Project.ShortcutListSections.Single(scls => scls.SectionId == sectionId);

            section.RemoveShortcut(shortcutId);

            return RedirectToAction("Index");
        }
    }
}