using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Commerce.Domain.Model;
using Pleiades.Commerce.Domain.NHibernateFluent;

namespace Pleiades.IntegrationTests.NHFluentTests
{
    [TestFixture]
    public class ManyToManyExperimentTests
    {
        [Test]
        public void Test1()
        {
            using (var session = NHManagerSingleton.Get.MakeOpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var left1 = new LeftTable() { Data1 = "Test Data 123", Data2 = 4, Data3 = 7.001f };
                var left2 = new LeftTable() { Data1 = "Test Data ABC", Data2 = 55, Data3 = 7.773f };
                var left3 = new LeftTable() { Data1 = "Test Data ZZZ", Data2 = 666, Data3 = 10.99f };

                session.SaveOrUpdate(left1);
                session.SaveOrUpdate(left2);
                session.SaveOrUpdate(left3);
                transaction.Commit();
            }
        }

        [Test]
        public void Test2()
        {
        }
    }
}

