using System;

namespace Commerce.Application.Model.Resources
{
    public class FileResource
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public string Name { get; set; }
        public string RelativeFilePath { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
