using System;
using System.Linq.Expressions;

namespace ANTLRTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate Parse Tree from file
            var constraints = ConstraintExtractor.getConstraints();
            foreach(Expression c in constraints)
            {
                // Call the solver to produce data for each constraint set in list
                
                Console.WriteLine(c.ToString());
            }
        }
    }
}
