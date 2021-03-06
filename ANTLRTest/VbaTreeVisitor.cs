﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using System.IO;
using Antlr4.Runtime;

namespace ANTLRTest
{
    class VbaTreeVisitor : VBGrammarBaseVisitor<Expression>
    {
        public List<RawConstraint> rawConstraints = new List<RawConstraint>();
        

        public override Expression VisitBlockIfThenElse([NotNull] VBGrammarParser.BlockIfThenElseContext context)
        {
            RawConstraint rc = new RawConstraint();
            rc.LineNumber = context.SourceInterval.a;
            rc.ExprType = "ITE";
            rc.Expr = null;
            rc.BranchType = null;
            // Get parent IfThenElse.  If not found before subStmt, then set to null
            findParent(context, rc);
            rawConstraints.Add(rc);


            return base.VisitBlockIfThenElse(context);
        }

        public override Expression VisitIfConditionStmt([NotNull] VBGrammarParser.IfConditionStmtContext context)
        {
            RawConstraint rc = new RawConstraint();
            rc.LineNumber = context.Start.StartIndex;
            rc.ExprType = "IB";
            rc.BranchType = "TRUE";
            findParent(context, rc);
            rc.Expr = getExpression(context);
            rawConstraints.Add(rc);

            return base.VisitIfConditionStmt(context);
        }

        private static Expression getExpression(VBGrammarParser.IfConditionStmtContext context)
        {
            String varname = null;
            String op = null;
            String value = null;

            var valueStatement = context.valueStmt();
            var count = valueStatement.ChildCount;

            // TODO - currently assume format of variable, operator, literal
            for (int i = 0; i < count; i++)
            {
                var child = valueStatement.GetChild(i);
                var type = child.GetType().ToString();

                if (child.GetText() == " ")
                {
                    continue;
                } else if (type == "VBGrammarParser+VsICSContext")
                {
                    varname = child.GetText();
                } else
                {
                    if (child.GetText() == "=")
                    {
                        op = "=";
                    } else if (child.GetText() == ">")
                    {
                        op = ">";
                    } else
                    {
                        value = child.GetText();
                    }
                    // Console.WriteLine(child.GetText());
                }

            }

            ParameterExpression varExpr = ParameterExpression.Parameter(typeof(int), varname);
            ParameterExpression literalExpr = ParameterExpression.Parameter(typeof(int), value);

            // TODO - add cases for more operations
            switch (op)
            {
                case "=":
                    BinaryExpression eq = BinaryExpression.Equal(varExpr, literalExpr);
                    return eq;
                case ">":
                    BinaryExpression gt = BinaryExpression.GreaterThan(varExpr, literalExpr);
                    return gt;
            }

            return null;
        }

        private void findParent(RuleContext context, RawConstraint rc)
        {
            var parent = context.Parent;
            
            while (parent != null)
            {
                String parentType = parent.GetType().ToString();
                if (parentType == "VBGrammarParser+SubStmtContext")
                {
                    // Console.WriteLine("Sub statement found before any branches, set parent null");
                    RuleContext token = (RuleContext)parent.Payload;
                    ParserRuleContext token1 = (ParserRuleContext)parent.Payload;
                    var test = token1.Start;
                    rc.ParentLineNumber = parent.SourceInterval.a;
                    
                    break;
                }
                else if (parentType == "VBGrammarParser+BlockIfThenElseContext")
                {
                    rc.ParentLineNumber = parent.SourceInterval.a;
                    break;
                }

                parent = parent.Parent;
                // Console.WriteLine("Parent type: " + parentType);
            }
        }

        public override Expression VisitIfElseBlockStmt([NotNull] VBGrammarParser.IfElseBlockStmtContext context)
        {
            RawConstraint rc = new RawConstraint();
            rc.LineNumber = context.Start.StartIndex;
            rc.ExprType = "IE";
            rc.BranchType = "FALSE";
            findParent(context, rc);
            rc.Expr = null;
            rawConstraints.Add(rc);

            return base.VisitIfElseBlockStmt(context);
        }


    }
}
