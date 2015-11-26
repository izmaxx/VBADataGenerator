using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Z3Solver
{
    public class Program
    {
        static void Main(string[] args)
        {


            using (Context ctx = new Context())
            {
                // Simplified case: (p1 < 3) or (p2 > 3)
                ParameterExpression p1 = Expression.Parameter(typeof(int), "p1");
                ParameterExpression p2 = Expression.Parameter(typeof(int), "p2");
                ConstantExpression three = Expression.Constant(3);
                BinaryExpression b1 = Expression.LessThan(p1, three);
                BinaryExpression b2 = Expression.GreaterThan(p2, three);
                BinaryExpression final = Expression.Or(b1, b2);

                Z3Solver solver = new Z3Solver();
                var result = solver.calculateTestData(final);


                //IntExpr x = ctx.MkIntConst("a");
                //IntExpr one = ctx.MkInt(3);
                //BoolExpr test = ctx.MkLt(x, one); // x< 3
                //Model model = Check(ctx, test, Status.SATISFIABLE);
            }
        }

        static Model Check(Context ctx, BoolExpr f, Status sat)
        {
            Solver s = ctx.MkSolver();
            s.Assert(f);
            if (s.Check() != sat)
            {
                return null;
            };
            if (sat == Status.SATISFIABLE)
            {
                Console.WriteLine("Data:" + s.Model);
                return s.Model;
            }
            else
                return null;
        }

        public static void CheckLessThan(int a, String b)
        {
            // Console.WriteLine("This test worked" + a + " " + b);

            using (Context ctx = new Context())
            {
                // 3 < x
                Expr x = ctx.MkConst(b, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(a, ctx.MkIntSort());
                BoolExpr test = ctx.MkLt((ArithExpr)value, (ArithExpr)x);
                Model model = Check(ctx, test, Status.SATISFIABLE);
            }
        }


    }
}
