using ANTLRTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Z3Solver
{
    class Z3Solver : ANTLRTest.ISolver
    {
        public Dictionary<string, object> calculateTestData(Expression pathConstraints)
        {
            Z3ExprVisitor visitor = new Z3ExprVisitor();
            var test = visitor.Visit(pathConstraints);
            // TODO
            return null;
        }

        public bool checkSatisfiability(Expression pathConstraints)
        {
            throw new NotImplementedException();
        }
    }
}
