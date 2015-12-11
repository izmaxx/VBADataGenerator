using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z3Solver;
using Microsoft.Z3;

namespace Z3Test
{
    class Z3
    {
        static void Main(string[] args)
        {
            // Check this string
            // (p1 < 3) and(p2 <= 3)
            //String equation = "(pppp < 39999) and (p < 3100)";
            //String equation = "(p1 >= 3) and (p2 <= 3)";
            //String equation = "(p1 >= 3) or (p2 > 300)";
            String equation = "{((p1 >= 3) or (p2 > 300))}"; 
            Expr w = Z3Solver.Program.parseAll2(equation);
            Console.WriteLine("the value is : " + w); 
        }
    }
}
