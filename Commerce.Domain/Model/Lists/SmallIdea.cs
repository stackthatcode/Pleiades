using System;
using System.Collections.Generic;

namespace Commerce.Application.Model.Lists
{
    public class SmallIdea
    {
        public SmallIdea()
        {
            this.MoreSmallIdeas = new List<SmallIdea>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<SmallIdea> MoreSmallIdeas { get; set; }
    }
}
