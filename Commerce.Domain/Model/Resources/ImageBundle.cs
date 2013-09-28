﻿using System;

namespace Commerce.Application.Model.Resources
{
    public class ImageBundle
    {
        public int Id { get; set; }
        public Guid ExternalId { get; set; }
        public FileResource Original { get; set; }
        public FileResource Thumbnail { get; set; }
        public FileResource Large { get; set; }
        public FileResource Small { get; set; }
        public bool Deleted { get; set; }
        public DateTime DateInserted { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}
