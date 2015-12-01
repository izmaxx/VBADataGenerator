using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ANTLRTest
{
    /// <summary>
    /// Store information found in branch
    /// </summary>
    class ConditionNode
    {
        public Expression Conditional { get; set; }
        public bool BranchType { get; set; }
        public int ParentLine { get; set; }
    }
}
