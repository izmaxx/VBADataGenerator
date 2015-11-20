using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

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

           // LabeledExprVisitor eval = new LabeledExprVisitor();
           // eval.Visit(tree);

            //parser.addSubExpr();
        }
    }
}
