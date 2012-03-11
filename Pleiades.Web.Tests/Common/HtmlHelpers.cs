using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Common.MvcHelpers;

namespace Pleiades.Web.Tests.Common
{
    [TestFixture]
    public class HtmlHelpers
    {       
        [Test]
        public void EnumFun()
        {
            foreach (var g in EnumToSelectList.Make<AccountLevel>())
            {
                TestLog.WriteLine(g.Text + " " + g.Value);
            }
        }
    }
}
