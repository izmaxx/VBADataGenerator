using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ANTLRTest
{
    class RawConstraint
    {
        public int LineNumber { get; set; }
        public int ParentLineNumber { get; set; }
        public Expression Expr { get; set; }
        public string ExprType { get; set; }
        public string BranchType { get; set; }
       
    }
}
