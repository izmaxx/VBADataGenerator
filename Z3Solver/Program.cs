﻿using ANTLRTest;
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
            // Generate Parse Tree from file
            var constraints = ConstraintExtractor.getConstraints();
            foreach (Expression c in constraints)
            {
                // Call the solver to produce data for each constraint set in list
                String equation = c.ToString();
                Expr testData = parseAll2(equation);
                Console.WriteLine(c.ToString());
                
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
                Console.WriteLine("Satisfiable"); 
                return s.Model;
            }
            else
                return null;
        }

     
        public static Expr parseAll2(String equation)
        {
            using (Context ctx = new Context())
            {
                String formula = equation;

                formula = formula.Replace(" ", "");
                formula = formula.ToLower();
                formula = formula.Remove(0, 1);               
                formula = formula.Remove(formula.Length - 2, 1);
                int formulaLength = formula.Length;
                int index;
                String leftEquation;
                int leftSymbolIndex;
                String leftValue1;
                String leftValue2;
                String rightEquation;
                int rightSymbolIndex;
                String rightValue1;
                String rightValue2;
                Expr resultQ;

                Console.WriteLine("formula: " + formula);

                if (formula.Contains(")and("))
                {
                    //Console.WriteLine("And Equation");
                    //Get the values of the left equation  
                    index = formula.IndexOf(")and(");
                    leftEquation = formula.Substring(0, index + 1);
                    //Console.WriteLine("left equation: " + leftEquation);
                    //Get the value of the right equation
                    rightEquation = formula.Substring(index + 4, formulaLength - (index + 4));
                    //Console.WriteLine("right equation: " + rightEquation);
                    //Check the sign of the left equation
                    if (leftEquation.Contains("<="))
                    {
                      //  //Console.WriteLine("left symbol: <=");
                        leftSymbolIndex = leftEquation.IndexOf("<=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                           // //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                           // //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                           // //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            ////Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            ////Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            ////Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            ////Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            ////Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            ////Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            ////Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            ////Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            ////Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            ////Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            ////Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            ////Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            ////Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            ////Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            ////Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                       
                    }
                    else if (leftEquation.Contains(">="))
                    {
                        //Console.WriteLine("left symbol: >=");
                        leftSymbolIndex = leftEquation.IndexOf(">=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if (leftEquation.Contains("<"))
                    {
                        //Get the values for the left Equation. 
                        //Console.WriteLine("left symbol: <");
                        leftSymbolIndex = leftEquation.IndexOf("<");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 1, leftEquation.Length - leftSymbolIndex - 2);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ; 
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex -2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if(leftEquation.Contains(">"))
                    {
                        //Console.WriteLine("left symbol: >");
                        leftSymbolIndex = leftEquation.IndexOf(">");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 1, leftEquation.Length - leftSymbolIndex - 2);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if(leftEquation.Contains("!="))
                    {
                        //Console.WriteLine("left symbol: !=");
                        leftSymbolIndex = leftEquation.IndexOf("!=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);
                        
                            //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }

                    }
                    else
                    {
                        Console.WriteLine("left symbol: ==");
                        leftSymbolIndex = leftEquation.IndexOf("==");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);
                        
                            //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeAndEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                }
                //fix here and below
                if (formula.Contains(")or("))
                {
                    //Console.WriteLine("OR Equation");
                    //Get the values of the left equation  
                    index = formula.IndexOf(")or(");
                    leftEquation = formula.Substring(0, index + 1);
                    //Console.WriteLine("left equation: " + leftEquation);
                    //Get the value of the right equation
                    rightEquation = formula.Substring(index + 3, formulaLength - (index + 3));
                    //Console.WriteLine("right equation: " + rightEquation);
                    //Check the sign of the left equation
                    if (leftEquation.Contains("<="))
                    {
                        //Console.WriteLine("left symbol: <=");
                        leftSymbolIndex = leftEquation.IndexOf("<=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LeOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if (leftEquation.Contains(">="))
                    {
                        //Console.WriteLine("left symbol: >=");
                        leftSymbolIndex = leftEquation.IndexOf(">=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GeOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if (leftEquation.Contains("<"))
                    {
                        //Get the values for the left Equation. 
                        //Console.WriteLine("left symbol: <");
                        leftSymbolIndex = leftEquation.IndexOf("<");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 1, leftEquation.Length - leftSymbolIndex - 2);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else 
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = LtOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if (leftEquation.Contains(">"))
                    {
                       // Console.WriteLine("left symbol: >");
                        leftSymbolIndex = leftEquation.IndexOf(">");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 1, leftEquation.Length - leftSymbolIndex - 2);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if(rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = GtOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else if (leftEquation.Contains("!="))
                    {
                       // Console.WriteLine("left symbol: !=");
                        leftSymbolIndex = leftEquation.IndexOf("!=");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">"))
                        {
                            //Console.WriteLine("right symbol: >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = NeOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("left symbol: ==");
                        leftSymbolIndex = leftEquation.IndexOf("==");
                        leftValue1 = leftEquation.Substring(1, leftSymbolIndex - 1);
                        //Console.WriteLine("left value1: " + leftValue1);
                        leftValue2 = leftEquation.Substring(leftSymbolIndex + 2, leftEquation.Length - leftSymbolIndex - 3);
                        //Console.WriteLine("left value2: " + leftValue2);

                        //Get the values for the right Equation
                        if (rightEquation.Contains("<="))
                        {
                            //Console.WriteLine("right symbol: <=");
                            rightSymbolIndex = rightEquation.IndexOf("<=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrLe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">="))
                        {
                            //Console.WriteLine("right symbol: >=");
                            rightSymbolIndex = rightEquation.IndexOf(">=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrGe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("<"))
                        {
                            //Console.WriteLine("right symbol: <");
                            rightSymbolIndex = rightEquation.IndexOf("<");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrLt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains(">"))
                        {
                           // Console.WriteLine("right symbol:  >");
                            rightSymbolIndex = rightEquation.IndexOf(">");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 1, rightEquation.Length - rightSymbolIndex - 2);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrGt(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else if (rightEquation.Contains("!="))
                        {
                            //Console.WriteLine("right symbol: !=");
                            rightSymbolIndex = rightEquation.IndexOf("!=");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrNe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                        else
                        {
                            //Console.WriteLine("right symbol: ==");
                            rightSymbolIndex = rightEquation.IndexOf("==");
                            rightValue1 = rightEquation.Substring(1, rightSymbolIndex - 1);
                            //Console.WriteLine("right value1: " + rightValue1);
                            rightValue2 = rightEquation.Substring(rightSymbolIndex + 2, rightEquation.Length - rightSymbolIndex - 3);
                            //Console.WriteLine("right value2: " + rightValue2);
                            resultQ = EeOrEe(leftValue1, Int32.Parse(leftValue2), rightValue1, Int32.Parse(rightValue2));
                            return resultQ;
                        }
                    }

                }
                return null; 
            }
        }
        //------------LT AND -------------------------------------------------------------------

        public static Expr LtAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));      
                Model model = Check(ctx, testing, Status.SATISFIABLE);             

                Expr result2;
                Model m2 = s.Model; 
                foreach(FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2; 
                }
            }
            return null; 
        }

        public static Expr LtAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }


        //-----------GT AND --------------------------------------------------------------------

        public static Expr GtAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //----------LE AND ---------------------------------------------------------------------

        public static Expr LeAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //----------GE AND ---------------------------------------------------------------------
        public static Expr GeAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //----------LT OR ---------------------------------------------------------------------
        public static Expr LtOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }


        //----------GT OR ---------------------------------------------------------------------
        public static Expr GtOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //----------LE OR ---------------------------------------------------------------------
        public static Expr LeOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //----------GE OR ---------------------------------------------------------------------
        public static Expr GeOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }





        
        public static Expr GtAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b),ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLt((ArithExpr)a, (ArithExpr)b),ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LtOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GtOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGt((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr LeOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkLe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr GeOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkGe((ArithExpr)a, (ArithExpr)b), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }


        //-----------Ne And-------------------------------------------------------------
        public static Expr NeAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))),ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        //-----------Ne Or-------------------------------------------------------------
        public static Expr NeOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr NeOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b))), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkNot(ctx.MkEq((ArithExpr)a, (ArithExpr)b)), ctx.MkEq((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }


        //-----------Ee And-------------------------------------------------------------

        public static Expr EeAndLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeAndGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeAndLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeAndGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeAndNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeAndEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkEq((ArithExpr)a, (ArithExpr)b))));
                s.Check();

                BoolExpr testing = ctx.MkAnd(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkEq((ArithExpr)a, (ArithExpr)b)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }




        //-----------Ee Or-------------------------------------------------------------

        public static Expr EeOrLe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeOrGe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGe((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeOrLt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkLt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeOrGt(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d)));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), ctx.MkGt((ArithExpr)c, (ArithExpr)d));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeOrNe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d)))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkNot(ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }

        public static Expr EeOrEe(String left1, int left2, String right1, int right2)
        {
            using (Context ctx = new Context())
            {
                Expr a = ctx.MkConst(left1, ctx.MkIntSort());
                Expr b = ctx.MkNumeral(left2, ctx.MkIntSort());
                Expr c = ctx.MkConst(right1, ctx.MkIntSort());
                Expr d = ctx.MkNumeral(right2, ctx.MkIntSort());

                Solver s = ctx.MkSolver();
                s.Assert(ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkEq((ArithExpr)c, (ArithExpr)d))));
                s.Check();

                BoolExpr testing = ctx.MkOr(ctx.MkEq((ArithExpr)a, (ArithExpr)b), (ctx.MkEq((ArithExpr)c, (ArithExpr)d)));
                Model model = Check(ctx, testing, Status.SATISFIABLE);
                

                Expr result2;
                Model m2 = s.Model;
                foreach (FuncDecl d2 in m2.Decls)
                {
                    result2 = m2.ConstInterp(d2);
                    return result2;
                }
            }
            return null;
        }







    }

}


