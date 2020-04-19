using System;
using System.Linq.Expressions;

namespace FORFarm.Application.Common
{
    public static class Extensions
    {
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExpr = one.Parameters[0];
            var body = Expression.Not(one.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
    }
}