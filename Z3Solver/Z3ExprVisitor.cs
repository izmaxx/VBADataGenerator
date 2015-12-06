using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ANTLRTest
{
    class Z3ExprVisitor
    {
        public Expression Visit(Expression exp)
        {
            using (Context ctx = new Context())
            {
                if (exp == null)
                    return exp;
                switch (exp.NodeType)
                {
                    case ExpressionType.Or:
                        // The VisitBinary method collects Z3 constraints by recursively walking the expression tree
                        var binExpr = VisitBinary((BinaryExpression)exp);
                        // When you get the results back, put them into a Z3 "or" expression
                        //ctx.MkOr(binExpr[0], binExpr[1]);
                        // TODO return the Z3 expression to be solved
                        return null;
                    case ExpressionType.LessThan:
                        VisitBinary((BinaryExpression)exp);
                        return null;
                    case ExpressionType.Parameter:
                        // parameter found
                        return null;
                }
            }
  
            return exp;
        }

        protected List<Microsoft.Z3.BoolExpr> VisitBinary(BinaryExpression b)
        {
            // Use recursion to generate Z3 constraints for each operand of the binary expression
            // left side -> create a Z3 expression representing the left side
            Expression left = Visit(b.Left);
            
            // right side -> creat a Z3 expression representing the right side
            Expression right = Visit(b.Right);

            // Put those into a Z3 format
            ExpressionType op = b.NodeType;

            using (Context ctx = new Context())
            {
                if (op == ExpressionType.LessThan)
                {
                    // ctx.MkLt(left, right);
                }
                
            }

            Console.WriteLine(b.ToString());
            
            // TODO - return a Z3 expression
            return null;
        }

    }
}
