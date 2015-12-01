using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z3Solver
{
    public class Program
    {
        static void Main(string[] args)
        {
            //using (Context ctx = new Context())
            //{
            //    IntExpr x = ctx.MkIntConst("a");
            //    IntExpr one = ctx.MkInt(3);
            //    BoolExpr test = ctx.MkLt(x, one); // x< 3
            //    Model model = Check(ctx, test, Status.SATISFIABLE);
            //}
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
                //Console.WriteLine("Data:" + s.Model);
                return s.Model;
            }
            else
                return null;
        }

        public static int CheckLessThan(int a, String b)
        {
            using (Context ctx = new Context())
            {
                // p1 < 3
                Expr number = ctx.MkConst(b, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(a, ctx.MkIntSort());
                BoolExpr test = ctx.MkLt((ArithExpr)value, (ArithExpr)number);
                Model model = Check(ctx, test, Status.SATISFIABLE);
                Console.WriteLine("testing: " + model);
                return 0; 
            }
        }

        public static Expr CheckLessThanValueFirst(String b , int a)
        {
            using (Context ctx = new Context())
            {
                // p1 < 3
                Expr number = ctx.MkConst(b, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(a, ctx.MkIntSort());
                BoolExpr test = ctx.MkLt((ArithExpr)number, (ArithExpr)value);
                Expr testing = ctx.MkLt((ArithExpr)number, (ArithExpr)value);
                Model model = Check(ctx, test, Status.SATISFIABLE);
                Console.WriteLine("ugh: " + testing); 

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkLt((ArithExpr)number, (ArithExpr)value));
                s.Check();
                Expr result = null; 
                Model m = s.Model; 
                //foreach(FuncDecl d in m.Decls)
                //{
                //    result = m.ConstInterp(d);
                //    //Console.WriteLine(m.ConstInterp(d));
                //    return result;
                //}
                return testing; 
            }
        }

        public static Expr CheckGreaterThanValueFirst(String b, int a)
        {
            using (Context ctx = new Context())
            {
                // p1 < 3
                Expr number = ctx.MkConst(b, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(a, ctx.MkIntSort());
                BoolExpr test = ctx.MkGt((ArithExpr)number, (ArithExpr)value);
                Expr testing = ctx.MkGt((ArithExpr)number, (ArithExpr)value);
                Model model = Check(ctx, test, Status.SATISFIABLE);

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkGt((ArithExpr)number, (ArithExpr)value));
                s.Check();
                Expr result = null;
                Model m = s.Model;
                //foreach (FuncDecl d in m.Decls)
                //{
                //    result = m.ConstInterp(d);
                //   // Console.WriteLine(m.ConstInterp(d));
                //    return result;
                //}
                return testing;
            }
        }

        public static Expr CheckLessThanOrEqualTo(String b, int a)
        {
            using (Context ctx = new Context())
            {
                // p1 < 3
                Expr number = ctx.MkConst(b, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(a, ctx.MkIntSort());
                BoolExpr test = ctx.MkLe((ArithExpr)number, (ArithExpr)value);
                Expr testing = ctx.MkLe((ArithExpr)number, (ArithExpr)value);

                Model model = Check(ctx, test, Status.SATISFIABLE);
                //Console.WriteLine("testing less than or equal: " + model);
                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkLe((ArithExpr)number, (ArithExpr)value));
                s.Check();
                Expr result;
                Model m = s.Model;
                //foreach (FuncDecl d in m.Decls)
                //{
                //    result = m.ConstInterp(d);
                //    //Console.WriteLine(m.ConstInterp(d));
                //    return result;
                //}
                return testing;
            }
        }

        public static Expr checkOr(Expr a, Expr b)
        {
            using (Context ctx = new Context())
            {
                Expr number = ctx.MkNumeral(2, ctx.MkIntSort());
                Expr value = ctx.MkNumeral(3, ctx.MkIntSort());
                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr((BoolExpr)number, (BoolExpr)value));
               
                s.Check();
                Expr result;
                Model m = s.Model;
                foreach (FuncDecl d in m.Decls)
                {
                    result = m.ConstInterp(d);
                    //Console.WriteLine(m.ConstInterp(d));
                    return result;
                }
                return null;
            }
        }

        public static void CheckGreaterThan(int a, String b)
        {
            using (Context ctx2 = new Context())
            {
                // p2 > 3
                Expr x = ctx2.MkConst(b, ctx2.MkIntSort());
                Expr value = ctx2.MkNumeral(a, ctx2.MkIntSort());
                BoolExpr test = ctx2.MkGt((ArithExpr)value, (ArithExpr)x);
                Model model = Check(ctx2, test, Status.SATISFIABLE);

            }
        }


        public static Expr CheckAll()
        {
            using (Context ctx = new Context())
            {
                Expr p1 = ctx.MkConst("p1", ctx.MkIntSort());
                Expr three = ctx.MkNumeral(3, ctx.MkIntSort());
                Expr p2 = ctx.MkConst("p2", ctx.MkIntSort());
                Expr two = ctx.MkNumeral(2, ctx.MkIntSort());
             

                Solver s = ctx.MkSolver();

                //s.Assert(ctx.MkAnd(ctx.MkOr(ctx.MkLt((ArithExpr)p1, (ArithExpr)three), ctx.MkGt((ArithExpr)p2, (ArithExpr)three)), ctx.MkLe((ArithExpr)p2, (ArithExpr)three)));

                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)p1, (ArithExpr)three), ctx.MkGt((ArithExpr)p2, (ArithExpr)three)), ctx.MkLe((ArithExpr)p2, (ArithExpr)three));

                s.Check();
                Expr result; 
                Model m = s.Model;
                foreach (FuncDecl d in m.Decls)
                {
                    result = m.ConstInterp(d);
                    return result;
                }
            }
            return null; 
        }

        public static void parseAll()
        {
            // (p1 < 3 or p2 > 3) and (p2 <= 3)
            String iS = "(p1 < 3 or p2 > 3) AND (p2 <= 3)";
            iS = iS.Replace(" ", "");
            iS = iS.ToLower(); 
            Console.WriteLine("inputString : " + iS);
            int stringlength = iS.Length; 
            Console.WriteLine("length: " + stringlength);

            String sample =""; 

            //Check if its an AND or OR. 
            if (iS.Contains(")and("))
            {
                Console.WriteLine("Make And expression ");
                sample = sample + "MKAND(";
                int index = iS.IndexOf(")and(");
                Console.WriteLine("index:" + index);
                String before = iS.Substring(0, index + 1);
                Console.WriteLine("before: " + before);
                String after = iS.Substring(index+4, stringlength-index-3-1);
                Console.WriteLine("after: " + after);
                if (before.Contains("and"))
                {
                    // separate the before and after. 
                    Console.WriteLine("and");
                }
                if (before.Contains("or"))
                {
                    //separate the before and after.
                    Console.WriteLine("or");
                    sample = sample + "MKOR(";
                    int beforeIndex = before.IndexOf("or");
                  
                    String beforeBefore = before.Substring(0, beforeIndex);
                    Console.WriteLine("beforeBefore: " + beforeBefore);
                    String beforeAfter = before.Substring(beforeIndex + 2, before.Length - beforeIndex -2);
                    Console.WriteLine("beforeAfter: " + beforeAfter);
                }

            }
            if (iS.Contains(")or("))
            {
                Console.WriteLine("Make OR expression ");
                sample = sample + "MKOR(";
                int index = iS.IndexOf(")and(");
                int index2 = stringlength - index - 3;
                Console.WriteLine("index2:" + index2);
                Console.WriteLine("index:" + index);
                String before = iS.Substring(0, index + 1);
                Console.WriteLine("before: " + before);
                if(before.Contains("and"))
                {
                    // separate the before and after. 
                    Console.WriteLine("and");
                }
                if(before.Contains("or"))
                {
                    //separate the before and after.
                    Console.WriteLine("or");
                }



                String after = iS.Substring(index + 3, stringlength - index - 2 - 1);
                Console.WriteLine("after: " + after);
            }

            if (iS.Contains(")<("))
            {
            }
            if (iS.Contains(")>("))
            {
            }




            Console.WriteLine("Z3: " + sample);

        }


    }
}
