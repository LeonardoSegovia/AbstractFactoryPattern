using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractFactoryPattern
{
    public interface IHotDrink
    {
        void Consume();
    }

    internal class Tea : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This tea is nice but I'd preder it with milk.!");
        }
    }

    internal class Coffee : IHotDrink
    {
        public void Consume()
        {
            Console.WriteLine("This coffee is sensational!");
        }
    }

    public interface IHotDrinkFactory
    {
        IHotDrink Prepare(int amount);
    }


    internal class TeaFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Put in a tea bag, boild water, pour {amount} ml.");
            return new Tea();
        }
    }

    internal class CoffeeFactory : IHotDrinkFactory
    {
        public IHotDrink Prepare(int amount)
        {
            Console.WriteLine($"Grind some beans, boil water, pour {amount} ml, add sugar.");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {

        private List<Tuple<string, IHotDrinkFactory>> _factories
            = new List<Tuple<string, IHotDrinkFactory>>();

        public HotDrinkMachine()
        {
            foreach (var t in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IHotDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    _factories.Add(Tuple.Create(t.Name.Replace("Factory", String.Empty),
                        (IHotDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }

        public IHotDrink MakeDrink()
        {
            Console.WriteLine("Available drinks:");
            for (var i = 0; i < _factories.Count; i++)
            {
                var tuple = _factories[i];

                Console.WriteLine($"{i}: {tuple.Item1}");
            }

            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int index)
                    && index >= 0
                    && index < _factories.Count)

                {
                    Console.WriteLine("Specify amount: ");
                    s = Console.ReadLine();
                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                        return _factories[index].Item2.Prepare(amount);

                }

                Console.WriteLine("Incorrect input, try again!");

            }
        }

    }



    class Program
    {
        static void Main(string[] args)
        {
            var hotDrinkMachine = new HotDrinkMachine();

            hotDrinkMachine.MakeDrink();

            Console.ReadKey();
        }
    }
}
