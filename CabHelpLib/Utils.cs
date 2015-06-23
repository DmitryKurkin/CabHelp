namespace Emerson.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;

    using Emerson.Common.Entities;

    public static class Utils
    {
        public static IEnumerable<SourceFile> LoadSourceFiles(string folder, out IEnumerable<SourceDir> sourceDirs)
        {
            var sourceDirectories = new Dictionary<string, SourceDir>();
            var sourceFiles = new List<SourceFile>();

            var currDirId = 1;
            foreach (var f in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
            {
                var directory = Path.GetDirectoryName(f);

                if (!sourceDirectories.ContainsKey(directory))
                {
                    var pathBuilder = new StringBuilder(directory, 260);
                    PathAddBackslashW(pathBuilder);

                    sourceDirectories.Add(
                        directory,
                        new SourceDir
                            {
                                Comment = Path.GetFileName(Path.GetDirectoryName(f)),
                                DirId = currDirId++,
                                Path = pathBuilder.ToString()
                            });
                }

                sourceFiles.Add(
                    new SourceFile { ParentDir = sourceDirectories[directory], FileName = Path.GetFileName(f) });
            }

            sourceDirs = sourceDirectories.Values;

            return sourceFiles;
        }

        [DllImport("shlwapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr PathAddBackslashW([MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszPath);
    }
}