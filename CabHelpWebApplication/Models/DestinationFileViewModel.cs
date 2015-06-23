namespace CabHelpWebApplication.Models
{
    using Emerson.Common.Entities;
    using System.Collections.Generic;

    public class DestinationFileViewModel
    {
        public DestinationFile DestinationFile { get; set; }

        public int ParentDirId { get; set; }

        public IEnumerable<string> SourceFiles { get; set; }
    }
}