namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common.Entities;
    using Emerson.Common.Sections;
    using System.Linq;
    using System.Web.Mvc;

    public class DestinationRegistryValuesController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult CreateSection()
        {
            return View("EditSection", new DestinationRegValuesSection());
        }

        [HttpGet]
        public ActionResult EditSection(int sectionId)
        {
            var section = ProjectRepository.Project.DestinationRegValuesSections.Single(drvs => drvs.SectionId == sectionId);

            return View(section);
        }

        [HttpPost]
        public ActionResult EditSection(DestinationRegValuesSection section)
        {
            var existingSection = ProjectRepository.Project.DestinationRegValuesSections.SingleOrDefault(drvs => drvs.SectionId == section.SectionId);

            if (existingSection != null)
            {
                existingSection.Name = section.Name;
                existingSection.IsDefault = section.IsDefault;
            }
            else
            {
                ProjectRepository.Project.AddRegValuesSection(section.Name, section.IsDefault);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteSection(int sectionId)
        {
            ProjectRepository.Project.RemoveRegValuesSection(sectionId);

            return RedirectToAction("Index");
        }

        public ActionResult CreateValue(int sectionId)
        {
            var viewModel = new DestinationRegValueViewModel
            {
                DestinationRegValue = new DestinationRegValue(),
                ParentSectionId = sectionId
            };

            return View("EditValue", viewModel);
        }

        [HttpGet]
        public ActionResult EditValue(int sectionId, int valueId)
        {
            var section = ProjectRepository.Project.DestinationRegValuesSections.Single(drvs => drvs.SectionId == sectionId);

            var value = section.RegValues.Single(drv => drv.ValueId == valueId);

            var viewModel = new DestinationRegValueViewModel
            {
                DestinationRegValue = value,
                ParentSectionId = sectionId
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditValue(DestinationRegValue destinationRegValue, int parentSectionId)
        {
            var section = ProjectRepository.Project.DestinationRegValuesSections.Single(drvs => drvs.SectionId == parentSectionId);

            var existingValue = section.RegValues.SingleOrDefault(drv => drv.ValueId == destinationRegValue.ValueId);

            if (existingValue != null)
            {
                existingValue.RegistryRoot = destinationRegValue.RegistryRoot;
                existingValue.Subkey = destinationRegValue.Subkey;
                existingValue.ValueName = destinationRegValue.ValueName;
                existingValue.Value = destinationRegValue.Value;
                existingValue.Flags = destinationRegValue.Flags;
            }
            else
            {
                section.AddRegValue(destinationRegValue.RegistryRoot, destinationRegValue.Subkey, destinationRegValue.ValueName, destinationRegValue.Value, destinationRegValue.Flags);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteValue(int sectionId, int valueId)
        {
            var section = ProjectRepository.Project.DestinationRegValuesSections.Single(drvs => drvs.SectionId == sectionId);

            section.RemoveRegValue(valueId);

            return RedirectToAction("Index");
        }
    }
}