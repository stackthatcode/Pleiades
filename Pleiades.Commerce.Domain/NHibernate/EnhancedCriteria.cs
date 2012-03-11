using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Criterion;

namespace Pleiades.Commerce.Domain.NHibernate
{
    public class Criterion
    {
        public string PropertyName;
        public object Value;
        public ExpressionType RestrictionType;
    }


    public class EnhancedCriteria
    {
        public static IEnumerable<Criterion> GetCriteria<T>(Expression<Func<T, bool>> expression)
        {
            return getCriteria<T>(expression.Body);
        }

        private static IEnumerable<Criterion> getCriteria<T>(System.Linq.Expressions.Expression expression)
        {
            if (expression is BinaryExpression)
            {
                var bin = (BinaryExpression)expression;

                // NOTE: Is this a good idea... appears to be obliterating the AND/OR differentiation...???

                if (bin.NodeType == ExpressionType.And || 
                    bin.NodeType == ExpressionType.AndAlso ||
                    bin.NodeType == ExpressionType.Or || 
                    bin.NodeType == ExpressionType.OrElse)
                {
                    return getCriteria<T>(bin.Left).Concat(getCriteria<T>(bin.Right));
                }

                if (bin.Left is MemberExpression)
                {
                    var me = (MemberExpression)bin.Left;
                    if (!(bin.Right is ConstantExpression))
                    {
                        throw new InvalidOperationException("Constant expected in criterion: " + bin.ToString());
                    }

                    return new[] 
                    {
                        new Criterion {
                            PropertyName = me.Member.Name,
                            Value = ((ConstantExpression) bin.Right).Value,
                            RestrictionType = bin.NodeType
                        } 
                    };
                }

                throw new InvalidOperationException("Unsupported binary operator: " + bin.NodeType);
            }

            throw new InvalidOperationException("Unsupported expression type: " + expression.GetType().Name);
        }
    }
}
