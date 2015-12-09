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
            int first;
            // need to do a parse that will split up the expressions. 
            // 2 < x
            //Z3Solver.Program.CheckLessThan(2,"x");
            //Testing
            //(p1 < 3 or p2 > 3) and (p2 <= 3)
            // p1 < 3
            //Expr lala = Z3Solver.Program.CheckLessThanValueFirst("p1", 3);
            //Console.WriteLine("lala: " + lala);
            ////p2 > 3
            //Expr haha = Z3Solver.Program.CheckGreaterThanValueFirst("p2", 3);
            //Console.WriteLine("hahah: " + haha);
            //// p2 < = 3
            //Expr boo = Z3Solver.Program.CheckLessThanOrEqualTo("p2", 3);
            //Console.WriteLine("boo: " + boo);

            //Z3Solver.Program.checkOr(lala, haha);

            //String test = lala.ToString();
            //int newval;
            //bool parsing = Int32.TryParse(test, out newval);

            //Console.Write("nuber:" + newval);
            //Expr Or = Z3Solver.Program.checkOr(Z3Solver.Program.CheckLessThanValueFirst("p1", 3), Z3Solver.Program.CheckGreaterThanValueFirst("p2", 3));
            //Console.Write("Or: " + Or);
            //Expr eh = Z3Solver.Program.CheckAll();
            //Console.Write("eh: " + eh);
            //Z3Solver.Program.parseAll();

            // Check this string
            // (p1 < 3) and(p2 <= 3)
            String equation = "(p1 < 3) and (p2 <= 3)";
            Expr w = Z3Solver.Program.parseAll2(equation);
            Console.WriteLine("the value is : " + w); 



        }
    }
}
