using System;
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
        List<ConditionNode> conditions = new List<ConditionNode>();

        public override Expression VisitBlock([NotNull] VBGrammarParser.BlockContext context)
        {
            int lineNumber = context.Start.Line;

            var source = context.Parent.SourceInterval;

            return base.VisitBlock(context);
        }

        public override Expression VisitIfConditionStmt([NotNull] VBGrammarParser.IfConditionStmtContext context)
        {
            //Expression left = null;
            //String op = "";
            //Expression right = null;
            var valueStatement = context.valueStmt();
            var currentLineNumber = valueStatement.Start.Line;

            var parent = context.Parent;
            var source = context.Parent.SourceInterval;
            while (parent != null)
            {
                String parentType = parent.Parent.GetType().ToString();
                if (parentType == "VBGrammarParser+BlockContext")
                {
                    var test = parent.SourceInterval;
                    
                    //Token firstToken = tokenStream.get(test.a);
                    //Token firstToken = tokenStream.get(sourceInterval.a);
                    //int line = firstToken.getLine();
                    Console.WriteLine("Found block");
                    break;
                } else
                {
                    parent = parent.Parent;
                    Console.WriteLine("Parent type: " + parentType);
                }
            }

           // var parentType = parent.GetType();

            var count = valueStatement.ChildCount;

            for (int i = 0; i < count; i++)
            {
                var child = valueStatement.GetChild(i);
                Console.WriteLine(child.GetType());
                Console.WriteLine(child.GetText());
            }

            //for (int i = 0; i < count; i++)
            //{

            //    var child = a.GetChild(i);
            //    var childText = child.GetText();

            //    if (child.GetType().ToString() == "VBGrammarParser+VsICSContext")
            //    {
            //        System.Console.WriteLine("Found VsICSContext");
            //        ParameterExpression param = Expression.Parameter(typeof(int), child.GetText());
            //        left = param;
            //    }

            //    if (child.GetText() == "=")
            //    {
            //        System.Console.WriteLine("Equal found");
            //        op = "=";
            //    }

            //    Console.WriteLine(child.GetType());
            //    if (child.GetType().ToString() == "VBGrammarParser+VsLiteralContext")
            //    {
            //        right = Visit(child);
            //    }
            //}

            //if (op == "=") {
            //    BinaryExpression b1 = BinaryExpression.Equal(left, right); 
            //}



            return VisitChildren(context);
        }

        public override Expression VisitLiteral([NotNull] VBGrammarParser.LiteralContext context)
        {
            var line = context.Start.Line;
            var value = context.GetText();
            var intContext = context.INTEGERLITERAL();
            var strContext = context.STRINGLITERAL();

            if (intContext != null)
            {
                ConstantExpression intConst = Expression.Constant(Convert.ToInt32(intContext.GetText()), typeof(int));
                return intConst;
            }

            System.Console.WriteLine("Value visitor: " + value );
            return null;
        }


    }
}
