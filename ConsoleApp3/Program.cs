    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks; 
    using System.Linq.Dynamic;
    namespace ConsoleApp3
    {
        class Program
        {
            static void Main(string[] args)
        {
            var bob = new Person
            {
                Name = "Bob",
                Age = 30,
                Weight = 30,
                FavouriteDay = new DateTime(2000, 1, 1)
            }; 

            Dictionary< string, Type> inputs = new Dictionary< string, Type>();
            inputs.Add( "Person", typeof(Person));
            inputs.Add("CalculateDiscountForAgeAndWeight", typeof(Func<int, int, string>));
            inputs.Add("DiscountCalculator", typeof(DiscountCalculator));

            var method1 = DynamicObject(typeof(Person), typeof(string), inputs, "Test1(Age,Names)");//Method, with in object
            var proptery1 = DynamicObject(typeof(Person), typeof(Func<Person, string>), inputs, "fur2"); //Property, with in object
            var staticMethod1 = DynamicObject2(typeof(string), inputs, "CalculateDiscountForAgeAndWeight.Invoke(Person.Age, Person.Weight)");//static method from the parameter
            var method2 = DynamicObject2(typeof(string), inputs, "DiscountCalculator.RideHasAgeLimit(Person.Age+1, \"Ride1\")");//method from the parameters

            var result1 = (proptery1.DynamicInvoke(bob) as Func<Person, string>)(bob);
            var result2 = method1.DynamicInvoke(bob);
            Func<int, int, string> t = DiscountCalculator.CalculateDiscountForAgeAndWeight;
            var discount = staticMethod1.DynamicInvoke(bob,t, null);
            var canride = method2.DynamicInvoke(new object[] {bob,null,new DiscountCalculator() });

            Console.ReadKey();
        }
    
            private static Delegate DynamicObject(Type InputType, Type OutPutType, IDictionary<string,Type> OtherInputs, string methodToCall) 
        {
            List<ParameterExpression> parameterExpressions = new List<ParameterExpression>();
            OtherInputs.ToList().ForEach( x => { parameterExpressions.Add(System.Linq.Expressions.Expression.Parameter( x.Value, x.Key)); });
            return System.Linq.Dynamic.DynamicExpression.ParseLambda(InputType, OutPutType, methodToCall, parameterExpressions).Compile();
        }
    
            private static Delegate DynamicObject2( Type OutPutType, IDictionary<string,Type> OtherInputs, string methodToCall)
        {
            var parameterExpressions = new List<ParameterExpression>();
            foreach (var item in OtherInputs)
            {
                parameterExpressions.Add(System.Linq.Expressions.Expression.Parameter(item.Value, item.Key));
            }
            return System.Linq.Dynamic.DynamicExpression.ParseLambda(parameterExpressions.ToArray(), OutPutType, methodToCall,null).Compile();
        }
    
    
            public class Person
            {
                public string Test1(int a, IEnumerable<string> names) { return a.ToString()+ names.First(); }
                public Func<Person, string> fur = p => p.Name;
                public Func<Person, string> fur2 = p => p.Age.ToString();
                public string fun3 => this.Name;
                public string Name { get; set; }
                public int Age { get;  set; }
                public int Weight { get; internal set; }
                public DateTime FavouriteDay { get; internal set; } 
                public IEnumerable<string> Names { get { return new List<string> { "one","two"}; } }
            }
    
            public  class DiscountCalculator
            {
                public static string CalculateDiscountForAgeAndWeight(int Age, int Weight)
                {
                    if (Age > 30 && Weight < 200)
                        return 10.ToString();
                    else
                        return 5.ToString();
                }
    
                public  string RideHasAgeLimit(int Age, string RideName)
                {
                    if (Age > 30 && RideName == "Ride1")
                        return true.ToString();
                    else
                        return false.ToString();
                }
                 
            }
        }
    }