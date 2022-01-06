using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Enumerables
{
    // IEnumerable Documentation: https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=net-6.0
    public class Driver
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Driver(ITestOutputHelper testOutputHelper){
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TheBasics()
        {
            // We create two lists and store one in a list and one in an IEnumerable.
            
            List<string> myList = new List<string>
            {
                "List.A",
                "List.B",
                "List.C",
            };

            IEnumerable<string> myEnumerable = new List<string>
            {
                "IEnumerable.A",
                "IEnumerable.B",
                "IEnumerable.C",
            };

            // We can see that both act the same way in this case!
            foreach (string value in myList)
                Console.WriteLine(value);

            // Notice that the foreach loop acts on an IEnumerable.
            foreach (string value in myEnumerable)
                Console.WriteLine(value);
        }

        [Fact]
        public void DifferentEnumerableConcreteTypes()
        {
            
            // Our good old list.
            List<string> myList = new List<string>
            {
                "List.A",
                "List.B",
                "List.C",
            };

            // This time we get a Stack instead of another list.
            Stack<string> myStack = new Stack<string>();
            myStack.Push("Stack.A");
            myStack.Push("Stack.B");
            myStack.Push("Stack.C");

            // This prints First in First Out (FIFO)
            foreach (string value in myList)
                Console.WriteLine(value);

            // This prints First In Last Out (FILO)
            foreach (string value in myStack)
                Console.WriteLine(value);
        }



        [Fact]
        public void FunWithGenerators()
        {
            int x = 0;
            // Another list as example with an incrementing integer as salt.
            List<string> myList = new List<string>
            {
                $"List.{x++}",
                $"List.{x++}",
                $"List.{x++}",
            };

            int y = 0;
            // A generator which creates an IEnumerable with an incrementing integer as salt.
            IEnumerable<string> Generate()
            {
                yield return $"IEnumerable.{y++}";
                yield return $"IEnumerable.{y++}";
                yield return $"IEnumerable.{y++}";
            }

            foreach (string value in myList)
            {
                _testOutputHelper.WriteLine(value);
                _testOutputHelper.WriteLine(x.ToString());
            }

            foreach (string value in Generate())
            {
                _testOutputHelper.WriteLine(value);
                _testOutputHelper.WriteLine(y.ToString());
            }
        }

        [Fact]
        public void TheWondersOfToList()
        {
            int y = 0;
            IEnumerable<string> Generate()
            {
                yield return $"IEnumerable.{y++}";
                yield return $"IEnumerable.{y++}";
                yield return $"IEnumerable.{y++}";
            }

            IEnumerable<string> myEnumerable = Generate().ToList();

            foreach (string value in myEnumerable)
            {
                _testOutputHelper.WriteLine(value);
                _testOutputHelper.WriteLine(y.ToString());
            }
        }

        [Fact]
        public void WhyIEnumerablesAreMorePerformant()
        {
            int y = 0;
            IEnumerable<string> Generate()
            {
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
            }

            // We're all about optimization, we don't convert the IEnumerable to a list.
            IEnumerable<string> myEnumerable = Generate();

            foreach (string value in myEnumerable)
            {
                _testOutputHelper.WriteLine(value);
                _testOutputHelper.WriteLine(y.ToString());
                if (value.Contains("0"))
                    return;
            }
        }

        [Fact]
        public void WhyCallingToListAllTheTimeIsBad()
        {
            int y = 0;
            IEnumerable<string> Generate()
            {
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
            }

            // We convert our IEnumerable to a list because lists are pretty.
            IEnumerable<string> myEnumerable = Generate().ToList();

            foreach (string value in myEnumerable)
            {
                _testOutputHelper.WriteLine(value);
                _testOutputHelper.WriteLine(y.ToString());
                if (value.Contains("0"))
                    return;
            }
        }

        [Fact]
        public void WhenGeneratorsAreSuboptimal()
        {
            int y = 0;
            IEnumerable<string> Generate()
            {
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
            }

            // We convert our IEnumerable to a list because lists are pretty.
            IEnumerable<string> myEnumerable = Generate();

            _testOutputHelper.WriteLine($"My count: {myEnumerable.Count()}");

            // Print all values since we want to know what's inside.
            foreach (string value in myEnumerable)
            {
                _testOutputHelper.WriteLine(value);
            }
        }

        [Fact]
        public void CachingYourResults()
        {
            int y = 0;
            IEnumerable<string> Generate()
            {
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
                Thread.Sleep(500);
                yield return $"IEnumerable.{y++}";
            }

            // We convert our IEnumerable to a list because lists are pretty.
            IEnumerable<string> myEnumerable = Generate().ToList();

            _testOutputHelper.WriteLine($"My count: {myEnumerable.Count()}");

            // Print all values since we want to know what's inside.
            foreach (string value in myEnumerable)
            {
                _testOutputHelper.WriteLine(value);
            }
        }
    }
}
