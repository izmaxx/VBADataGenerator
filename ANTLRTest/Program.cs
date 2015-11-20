using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System.Linq.Expressions;

namespace ANTLRTest
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream fileStream = new FileStream(@"c:\example1.txt", FileMode.Open, FileAccess.Read);

            Stream inputStream = Console.OpenStandardInput();
            AntlrInputStream input = new AntlrInputStream(fileStream);
            //AntlrInputStream input = new AntlrInputStream("{1,2}");
            VBGrammarLexer lexer = new VBGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            VBGrammarParser parser = new VBGrammarParser(tokens);

            IParseTree tree = parser.startRule(); // begin parsing at init rule
            
            Console.WriteLine(tree.ToStringTree(parser));

            ParameterExpression p1 = Expression.Parameter(typeof(int), "p1");
            ParameterExpression p2 = Expression.Parameter(typeof(int), "p2");
            ConstantExpression c1 = Expression.Constant(3);
            BinaryExpression b1 = Expression.LessThan(p1, c1);
            BinaryExpression b2 = Expression.GreaterThan(p2, c1);
            BinaryExpression b3 = Expression.LessThanOrEqual(p2, c1);
            BinaryExpression final = Expression.And(Expression.Or(b1, b2), b3);

            //var test = b1.Left.GetType();
            System.Console.WriteLine(final);

            List<Expression> exp = new List<Expression>();
           // LabeledExprVisitor eval = new LabeledExprVisitor();
           // eval.Visit(tree);

            //parser.addSubExpr();


        }
    }
}
