using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Commerce.Persist.Concrete;
using Commerce.Persist.Model.Lists;

namespace Commerce.IntegrationTests
{
    [TestFixture]
    public class SmallIdeaTests : FixtureBase
    {
        [Test]
        public void Test1()
        {
            var context = new PleiadesContext();
            context.SmallIdeas.ToList().ForEach(x => context.SmallIdeas.Remove(x));
            context.SaveChanges();

            var idea1 = new SmallIdea { Name = "Idea1" };
            var idea2 = new SmallIdea { Name = "Idea2" };
            var idea3 = new SmallIdea { Name = "Idea3" };
            var idea4 = new SmallIdea { Name = "Idea4" };

            idea1.MoreSmallIdeas.Add(idea2);
            idea1.MoreSmallIdeas.Add(idea3);
            idea3.MoreSmallIdeas.Add(idea4);

            context.SmallIdeas.Add(idea1);
            context.SaveChanges();

            var retrieveIdea1 =
                context
                    .SmallIdeas
                    .AsQueryable()
                    .Include(x => x.MoreSmallIdeas)
                    .First(x => x.Name == "Idea1");

            Console.WriteLine(retrieveIdea1.Name);
        }
    }
}
