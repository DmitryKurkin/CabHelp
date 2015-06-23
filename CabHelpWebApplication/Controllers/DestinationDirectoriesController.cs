namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common.Entities;
    using Emerson.Common.Sections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class DestinationDirectoriesController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult CreateDirectory()
        {
            return View("EditDirectory", new DestinationDir());
        }

        [HttpGet]
        public ActionResult EditDirectory(int dirId)
        {
            var dir = ProjectRepository.Project.DestinationDirs.Directories.Single(dd => dd.DirId == dirId);

            return View(dir);
        }

        [HttpPost]
        public ActionResult EditDirectory(DestinationDir directory)
        {
            var existingDirectory = ProjectRepository.Project.DestinationDirs.Directories.SingleOrDefault(dd => dd.DirId == directory.DirId);

            if (existingDirectory != null)
            {
                existingDirectory.DirectoryName = directory.DirectoryName;
                existingDirectory.IsDefault = directory.IsDefault;
                existingDirectory.SectionName = directory.SectionName;
            }
            else
            {
                ProjectRepository.Project.DestinationDirs.AddDirectory(
                    directory.DirectoryName,
                    directory.IsDefault,
                    directory.SectionName);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDirectory(int dirId)
        {
            ProjectRepository.Project.DestinationDirs.RemoveDirectory(dirId);

            return RedirectToAction("Index");
        }

        public ActionResult CreateFile(int dirId)
        {
            var sourceFiles = new List<string>();
            foreach (var sd in ProjectRepository.Project.SourceDirs.Directories)
            {
                sourceFiles.AddRange(sd.Files.Select(sf => sf.FileName));
            }

            var viewModel = new DestinationFileViewModel
            {
                DestinationFile = new DestinationFile(),
                ParentDirId = dirId,
                SourceFiles = sourceFiles
            };

            return View("EditFile", viewModel);
        }

        [HttpGet]
        public ActionResult EditFile(int dirId, int fileId)
        {
            var section = ProjectRepository.Project.DestinationDirs.Directories.Single(dd => dd.DirId == dirId);

            var file = section.Files.Single(df => df.FileId == fileId);

            var sourceFiles = new List<string>();
            foreach (var sd in ProjectRepository.Project.SourceDirs.Directories)
            {
                sourceFiles.AddRange(sd.Files.Select(sf => sf.FileName));
            }

            var viewModel = new DestinationFileViewModel
            {
                DestinationFile = file,
                ParentDirId = dirId,
                SourceFiles = sourceFiles
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditFile(DestinationFile destinationFile, int parentDirId)
        {
            if (string.IsNullOrWhiteSpace(destinationFile.FileName))
            {
                destinationFile.FileName = destinationFile.SourceFileName;
            }

            var section = ProjectRepository.Project.DestinationDirs.Directories.Single(dd => dd.DirId == parentDirId);

            var existingFile = section.Files.SingleOrDefault(df => df.FileId == destinationFile.FileId);

            if (existingFile != null)
            {
                existingFile.FileName = destinationFile.FileName;
                existingFile.SourceFileName = destinationFile.SourceFileName;
                existingFile.Flags = destinationFile.Flags;
            }
            else
            {
                section.AddFile(destinationFile.FileName, destinationFile.SourceFileName, destinationFile.Flags);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile(int dirId, int fileId)
        {
            var section = ProjectRepository.Project.DestinationDirs.Directories.Single(dd => dd.DirId == dirId);

            section.RemoveFile(fileId);

            return RedirectToAction("Index");
        }
    }
}