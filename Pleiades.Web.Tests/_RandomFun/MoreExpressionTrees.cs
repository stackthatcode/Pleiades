using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gallio.Framework;
using MbUnit.Framework;

namespace Pleiades.Web.Tests.RandomFun
{
    [TestFixture]
    public class MoreExpressionTrees
    {
        [Test]
        public void Test1()
        {
            Expression<Func<int, int, int>> expr = (a, b) => a + b;
            TestLog.WriteLine("Param 1: {0}, Param 2: {1}", expr.Parameters[0], expr.Parameters[1]);

            var body = (BinaryExpression)expr.Body;
            var left = (ParameterExpression)body.Left;
            var right = (ParameterExpression)body.Right;

            TestLog.WriteLine(expr.Body);
            TestLog.WriteLine(" The left part of the expression: " +
              "{0}{4} The NodeType: {1}{4} The right part: {2}{4} The Type: {3}{4}",
              left.Name, body.NodeType, right.Name, body.Type, Environment.NewLine);

            int result = expr.Compile()(3, 5);
            TestLog.WriteLine(result);
        }

        [Test]
        public void Test2()
        {
            List<int> list = new List<int>() { 1, 2, 3 };

            var query = from number in list
                        where number < 3
                        select number;

            foreach (var value in query)
                TestLog.WriteLine(value);
        }

        [Test]
        public void FactorialTest()
        {
            ParameterExpression value =
                Expression.Parameter(typeof(int), "value");
            ParameterExpression result =
                Expression.Parameter(typeof(int), "result");
            LabelTarget label = Expression.Label(typeof(int));
            
            BlockExpression block = Expression.Block(
                new[] { result },
                Expression.Assign(result, Expression.Constant(1)),
                Expression.Loop(
                    Expression.IfThenElse(
                        Expression.GreaterThan(value,
                            Expression.Constant(1)),
                        Expression.MultiplyAssign(result,
                            Expression.PostDecrementAssign(value)),
                    Expression.Break(label, result)
                    ),
                    label
                )
            );

            var lambda = Expression.Lambda<Func<int, int>>(block, value);

            // See the lambda expression in the Text Visualizer
            Console.WriteLine(lambda.Compile()(5));
        }
    }
}
