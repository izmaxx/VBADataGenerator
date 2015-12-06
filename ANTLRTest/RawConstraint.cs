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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ExprType: " + ExprType);
            sb.Append(", Parent: " + ParentLineNumber.ToString());
            sb.Append(", Line: " + LineNumber.ToString());
            sb.Append("\n");
            return sb.ToString();
        }

        /// <summary>
        /// Finds IF-THEN-ELSE blocks that are at the same level as an IF block and
        /// moves children of IF-THEN-ELSE to become children of the IF block
        /// </summary>
        /// <param name="rawConstraints"></param>
        public static List<RawConstraint> updateParents(List<RawConstraint> rawConstraints)
        {
            var ibs = rawConstraints.Where<RawConstraint>(c => c.ExprType == "IB");
            foreach (var ib in ibs)
            {
                // Find any ITE with same parent line
                var parentLine = ib.ParentLineNumber;
                var ites = from rc in rawConstraints
                           where rc.ParentLineNumber == parentLine && rc.ExprType == "ITE"
                           select rc;

                // Get children of ITE
                foreach (var ite in ites)
                {
                    var iteLine = ite.LineNumber;
                    var children = from rc in rawConstraints
                                   where rc.ParentLineNumber == iteLine
                                   select rc;

                    foreach (var child in children)
                    {
                        child.ParentLineNumber = ib.LineNumber;
                    }
                }
            }

            return rawConstraints;
        }

        public static List<RawConstraint> getLeafNodes(List<RawConstraint> rawConstraints)
        {
            var leafNodes = new List<RawConstraint>();
            foreach (var rc in rawConstraints)
            {
                int currentLine = rc.LineNumber;

                var children = from rc1 in rawConstraints
                               where rc1.ParentLineNumber == currentLine
                               select rc1;

                int count = children.Count<RawConstraint>();
                if (count == 0)
                {
                    leafNodes.Add(rc);
                }
            }

            return leafNodes;
        }
    }
}
