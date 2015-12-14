using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ANTLRTest
{
    public class ConstraintExtractor
    {
        public static List<Expression> getConstraints()
        {
            List<Expression> pathConstraints = new List<Expression>();

            FileStream fileStream = new FileStream(@"c:\example2.txt", FileMode.Open, FileAccess.Read);
            AntlrInputStream input = new AntlrInputStream(fileStream);
            VBGrammarLexer lexer = new VBGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            VBGrammarParser parser = new VBGrammarParser(tokens);
            IParseTree tree = parser.startRule();
            //Console.WriteLine(tree.ToStringTree(parser));

            // Use visitor pattern to get "raw" constraints
            VbaTreeVisitor eval = new VbaTreeVisitor();
            Expression exp = eval.Visit(tree);
            List<RawConstraint> rawConstraints = eval.rawConstraints;

            // Create negated expressions for ELSE blocks
            RawConstraint.updateElseBlocks(rawConstraints);

            // Update constraints to transform into binary tree and get leaf nodes
            RawConstraint.updateParents(rawConstraints);
            List<RawConstraint> leafNodes = RawConstraint.getLeafNodes(rawConstraints);

            // For each leaf node, work the way up to generate PathConstraints
            foreach (RawConstraint leaf in leafNodes)
            {
                var parentLine = leaf.ParentLineNumber;
                if (leaf.Expr == null) { continue; }

                // find parent
                RawConstraint parent = findParent(rawConstraints, parentLine);

                // AND with parent
                BinaryExpression binExp = (BinaryExpression)leaf.Expr;
                while (parent != null)
                {
                    if (parent.Expr == null)
                    {
                        break;
                    }
                    else
                    {
                        binExp = BinaryExpression.And(binExp, parent.Expr);
                    }

                    parent = findParent(rawConstraints, parent.ParentLineNumber);
                }

                pathConstraints.Add(binExp);
                // Console.WriteLine("Leaf: " + leaf.Expr);
                // Console.WriteLine("AND: " + binExp);
            }

            return pathConstraints;
        }

        private static RawConstraint findParent(List<RawConstraint> rawConstraints, int parentLine)
        {
            var parents = from rc in rawConstraints
                          where rc.LineNumber == parentLine
                          select rc;

            var parent = parents.Single<RawConstraint>();
            return parent;
        }
    }
}

