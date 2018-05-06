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
        public enum AvailableDrink
        {
            Coffee, Tea
        }

        private readonly Dictionary<AvailableDrink, IHotDrinkFactory> _drinkFactories = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        public HotDrinkMachine()
        {
            foreach (var drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory =
                    (IHotDrinkFactory)Activator.CreateInstance(
                                        Type.GetType($"AbstractFactoryPattern.{Enum.GetName(typeof(AvailableDrink), drink)}Factory")
                                         ?? throw new InvalidOperationException());

                _drinkFactories.Add((AvailableDrink) drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
            return _drinkFactories[drink].Prepare(amount);
        }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var hotDrinkMachine = new HotDrinkMachine();
            var drink = hotDrinkMachine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea,100);
            drink.Consume();

            var otherDrink = hotDrinkMachine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 80);
            otherDrink.Consume();

            Console.ReadKey();
        }
    }
}
