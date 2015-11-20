using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ANTLRTest
{
    interface ISolver
    {
        /// <summary>
        /// Takes in a path constraint (one or more conditions) and returns inputs that will satisfy the path constraint.
        /// </summary>
        /// <param name="pathConstraints">Expression object that contains path constraint</param>
        /// <returns>Assignment of variables to inputs that satisfy constraints </returns>
        Dictionary<String, Object> calculateTestData(Expression pathConstraints);

        Boolean checkSatisfiability(Expression pathConstraints);

        /* This is an example Expression */
        // (p1 < 3 or p2 > 3) and (p2 <= 3)

        //ParameterExpression p1 = Expression.Parameter(typeof(int), "p1");
        //ParameterExpression p2 = Expression.Parameter(typeof(int), "p2");
        //ConstantExpression c1 = Expression.Constant(3);
        //BinaryExpression b1 = Expression.LessThan(p1, c1);
        //BinaryExpression b2 = Expression.GreaterThan(p2, c1);
        //BinaryExpression b3 = Expression.LessThanOrEqual(p2, c1);
        //BinaryExpression exp = Expression.And(Expression.Or(b1, b2), b3);
        
    }
}
