namespace BurgerPreparation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AsyncCooker
    {
        public static async Task Work()
        {
            await Task.WhenAll(
                HeatThePans(),
                UnfreezeMeat(),
                PeelPotatoes());

            await Task.WhenAll(
                CookBurgers(),
                FryFries(),
                PourDrinks());

            await ServeAndEat();
        }

        public static async Task HeatThePans()
        {
            await Task.Delay(2000);
            Console.WriteLine("Pan heated!");
        }

        public static async Task UnfreezeMeat()
        {
            await Task.Delay(3000);
            Console.WriteLine("Meat ready!");
        }

        public static async Task PeelPotatoes()
        {
            await Task.Delay(2000);
            Console.WriteLine("Potatoes peeled!");
        }

        public static async Task CookBurgers()
        {
            await Task.Delay(5000);
            Console.WriteLine("Burgers cooked!");
        }

        public static async Task FryFries()
        {
            await Task.Delay(3000);
            Console.WriteLine("Fries fried!");
        }

        public static async Task PourDrinks()
        {
            await Task.Delay(1000);
            Console.WriteLine("Drinks poured!");
        }

        public static async Task ServeAndEat()
        {
            await Task.Delay(5000);
            Console.WriteLine("Delicious!");
        }
    }
}
