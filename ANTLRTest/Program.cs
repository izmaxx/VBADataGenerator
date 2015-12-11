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
            // Generate Parse Tree from file
            FileStream fileStream = new FileStream(@"c:\example2.txt", FileMode.Open, FileAccess.Read);
            AntlrInputStream input = new AntlrInputStream(fileStream);
            VBGrammarLexer lexer = new VBGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            VBGrammarParser parser = new VBGrammarParser(tokens);
            IParseTree tree = parser.startRule();
            Console.WriteLine(tree.ToStringTree(parser));

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
            // TODO

        }


    }
}
