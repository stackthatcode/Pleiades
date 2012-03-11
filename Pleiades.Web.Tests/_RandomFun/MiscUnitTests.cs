using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using Gallio.Framework;
using MbUnit.Framework;
using Pleiades.Web.Security.Model;
using Pleiades.Web.Common.MvcHelpers;

namespace Pleiades.Web.Tests.RandomFun
{
    public class TestClass
    {
        public string Name { get; set; }
        public int Balance { get; set; }

        public object GetPropertyViaExpression(Expression<Func<TestClass, object>> expr)
        {
            Func<TestClass, object> del = expr.Compile();
            return del(this);
        }
    }

    [TestFixture]
    public class MiscUnitTests
    {
        [Test]
        public void ExpressionTreesPart3()
        {
            var c1 = new TestClass { Name = "Jimmy", Balance = 333 };
            TestLog.WriteLine(c1.GetPropertyViaExpression(x => x.Balance));
        }

        [Test]
        public void ExpressionTreesPart2()
        {
            var myobject = new TestClass() { Name = "Barbara", Balance = 789 };

            // Now, at some point, I'll want to extract Balance, then Name...
            var listOfExpr = new List<Expression<Func<TestClass, object>>>();
            listOfExpr.Add(x => x.Balance);
            listOfExpr.Add(x => x.Name);

            foreach (var expr in listOfExpr)
            {
                var member = expr.Body;
                //var memberName = member.m Member.Name;

                Func<TestClass, object> del = expr.Compile();
                var value = del(myobject).ToString();
                TestLog.WriteLine("Value : " + value);
            }
        }

        [Test]
        public void ExpressionTreesPart1()
        {
            var myobject = new TestClass() { Name = "Barbara", Balance = 789 };
            TestLog.WriteLine(GetName(() => myobject.Name));
            TestLog.WriteLine(GetName(() => myobject.Balance));
        }

        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            var memberName = member.Member.Name;

            Func<T> del = e.Compile();
            var memberValue = del();

            return memberName + " " + memberValue;
        }
    }
}
