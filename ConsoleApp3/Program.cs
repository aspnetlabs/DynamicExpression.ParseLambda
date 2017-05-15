using NCalc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using myAlias = System.Linq.Dynamic;
namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = System.Linq.Expressions.Expression.Parameter(typeof(Person), "Person");
            var p1 = System.Linq.Expressions.Expression.Parameter(typeof(int), "Age");
            var p2 = System.Linq.Expressions.Expression.Parameter(typeof(IEnumerable<string>), "Names");

            var k = myAlias.DynamicExpression.ParseLambda<Person ,string >("Test1(Age,Names)", new[] { p1, p2});

            var bob = new Person
            {
                Name = "Bob",
                Age = 30,
                Weight = 213,
                FavouriteDay = new DateTime(2000, 1, 1)
            };
            
            //var result = e.Compile().DynamicInvoke(bob);
            var result2 = k.Compile().Invoke(bob); 

            Console.ReadKey();
        }

        public class Person
        {
            public string Test1(int a, IEnumerable<string> names) { return a.ToString()+ names.First(); }
            public Func<Person, string> fur = p => p.Name;
            public Func<Person, string> fur2 = p => p.Age.ToString();
            public string Name { get; set; }
            public int Age { get; internal set; }
            public int Weight { get; internal set; }
            public DateTime FavouriteDay { get; internal set; } 
            public IEnumerable<string> Names { get { return new List<string> { "one","two"}; } }
        }
    }
}