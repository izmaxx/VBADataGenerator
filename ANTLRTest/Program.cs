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



            System.Console.WriteLine(final);

            VbaTreeVisitor eval = new VbaTreeVisitor();
            Expression exp = eval.Visit(tree);
            var test = eval.rawConstraints;


            //parser.addSubExpr();


        }
    }
}
