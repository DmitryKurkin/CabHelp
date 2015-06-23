namespace CabHelpWebApplication.Controllers
{
    using CabHelpWebApplication.Models;
    using Emerson.Common.Entities;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public class SourceDirectoriesController : Controller
    {
        public ActionResult Index()
        {
            return View(new ProjectViewModel { Project = ProjectRepository.Project });
        }

        public ActionResult CreateDirectory()
        {
            return View("EditDirectory", new SourceDir());
        }

        [HttpGet]
        public ActionResult EditDirectory(int dirId)
        {
            var dir = ProjectRepository.Project.SourceDirs.Directories.Single(sd => sd.DirId == dirId);

            return View(dir);
        }

        [HttpPost]
        public ActionResult EditDirectory(SourceDir directory)
        {
            var existingDirectory = ProjectRepository.Project.SourceDirs.Directories.SingleOrDefault(sd => sd.DirId == directory.DirId);

            if (existingDirectory != null)
            {
                existingDirectory.Path = directory.Path;
                existingDirectory.Comment = directory.Comment;
            }
            else
            {
                ProjectRepository.Project.SourceDirs.AddDirectory(directory.Path, directory.Comment);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteDirectory(int dirId)
        {
            ProjectRepository.Project.SourceDirs.RemoveDirectory(dirId);

            return RedirectToAction("Index");
        }

        public ActionResult LoadFiles(int dirId, string fileList)
        {
            var files = fileList.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            var directory = ProjectRepository.Project.SourceDirs.Directories.Single(sd => sd.DirId == dirId);
            directory.LoadFiles(files);

            return RedirectToAction("Index");
        }

        public ActionResult CreateFile(int dirId)
        {
            var viewModel = new SourceFileViewModel
            {
                SourceFile = new SourceFile(),
                ParentDirId = dirId
            };

            return View("EditFile", viewModel);
        }

        [HttpGet]
        public ActionResult EditFile(int dirId, int fileId)
        {
            var directory = ProjectRepository.Project.SourceDirs.Directories.Single(sd => sd.DirId == dirId);

            var file = directory.Files.Single(sf => sf.FileId == fileId);

            var viewModel = new SourceFileViewModel
            {
                SourceFile = file,
                ParentDirId = dirId
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditFile(SourceFile sourceFile, int parentDirId)
        {
            var parentDir = ProjectRepository.Project.SourceDirs.Directories.Single(sd => sd.DirId == parentDirId);

            var existingFile = parentDir.Files.SingleOrDefault(sf => sf.FileId == sourceFile.FileId);

            if (existingFile != null)
            {
                existingFile.FileName = sourceFile.FileName;
            }
            else
            {
                parentDir.AddFile(sourceFile.FileName);
            }

            return RedirectToAction("Index");
        }

        public ActionResult DeleteFile(int dirId, int fileId)
        {
            var directory = ProjectRepository.Project.SourceDirs.Directories.Single(sd => sd.DirId == dirId);

            directory.RemoveFile(fileId);

            return RedirectToAction("Index");
        }
    }
}