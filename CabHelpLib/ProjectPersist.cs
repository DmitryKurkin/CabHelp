namespace Emerson.Common
{
    using System;
    using Newtonsoft.Json;

    public static class ProjectPersist
    {
        public static string Save(Project project)
        {
            if (project == null) throw new ArgumentNullException("project");

            return JsonConvert.SerializeObject(project);
        }

        public static Project Load(string persistedProject)
        {
            if (persistedProject == null) throw new ArgumentNullException("persistedProject");

            return JsonConvert.DeserializeObject<Project>(persistedProject);
        }
    }
}