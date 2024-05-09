namespace CodeAnalysis.Domain.Analyzers.Tests.TestClasses
{
    public class CSharpIfElseClass
    {
        /// <summary>
        ///     BasicMethod
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 0
        /// </remarks>
        public static void BasicMethod()
        {
            Console.WriteLine("This method does nothing");
        }


        /// <summary>
        ///     Method_IfStatement
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 1
        /// </remarks>
        public void Method_IfStatement()
        {
            var rand = new Random(100);
            if (0 > rand.Next()) //Adds 1
                Console.WriteLine("Do nothing");
        }
        
        /// <summary>
        ///     Method_IfElseStatement
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 2
        /// </remarks>
        public void Method_IfElseStatement()
        {
            var rand = new Random(100);
            if (0 > rand.Next()) //Adds 1
                Console.WriteLine("Do nothing");

            else // Adds 1
                Console.WriteLine("Do this instead");
        }
        
        /// <summary>
        ///     Method_IfElseStatement
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 3
        /// </remarks>
        public void Method_IfElseIfStatement()
        {
            var rand = new Random(100);
            if (0 > rand.Next()) //Adds 1
                Console.WriteLine("Do nothing");
            
            else if (2 > rand.Next())
            {
                Console.WriteLine("Do nothing here too");
            }

            else // Adds 1
                Console.WriteLine("Do this instead");
        }

        /// <summary>
        ///     Method_NestedIfElseStatement
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 5
        /// </remarks>
        public void Method_NestedIfElseStatement()
        {
            var rand = new Random(100);
            if (10 > rand.Next()) // Adds 1
            {
                if (10 > rand.Next()) // Adds 2
                    Console.WriteLine("Do nothing");

                else // Adds 1
                    Console.WriteLine("Do this instead");
            }

            else // Adds 1
            {
                Console.WriteLine("Do this instead");
            }
        }

        /// <summary>
        ///     Method_DoublyNestedIfElseStatement
        /// </summary>
        /// <remarks>
        ///     Cognitive complexity: 8
        /// </remarks>
        public void Method_DoublyNestedIfElseStatement()
        {
            var rand = new Random(100);
            if (10 > rand.Next()) // Adds 1
            {
                if (10 > rand.Next()) //Adds 2
                    Console.WriteLine("Do nothing");

                else // Adds 1
                    Console.WriteLine("Do this instead");
            }

            else // Adds 1
            {
                if (0 > rand.Next())// Adds 2
                    Console.WriteLine("Do nothing");

                else // Adds 1
                    Console.WriteLine("Do this instead");
            }
        }

        public void Method_DeeplyNestedIfElseStatement()
        {
            var rand = new Random(100);
            if (10 > rand.Next()) // Adds 1
            {
                if (10 > rand.Next()) // Adds 2
                {
                    if (10 > rand.Next()) // Adds 3
                        Console.WriteLine("Do nothing");

                    else // Adds 1
                        Console.WriteLine("Do this instead");
                }

                else // Adds 1
                {
                    if (0 > rand.Next()) // Adds 1
                        Console.WriteLine("Do nothing");

                    else // Adds 1
                        Console.WriteLine("Do this instead");
                }
            }
        }

        public void Method_CoalescedIfElse()
        {
            var rand = new Random(100);
            Console.WriteLine(0 > rand.Next() ? "Do nothing" : "Do this instead");
        }
    }
}