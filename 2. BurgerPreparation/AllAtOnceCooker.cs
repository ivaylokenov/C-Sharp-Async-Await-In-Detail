namespace BurgerPreparation
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class AllAtOnceCooker
    {
        public static async Task Work()
        {
            var tasks = new List<Task>
            {
                HeatThePans(),
                UnfreezeMeat(),
                PeelPotatoes(),
                CookBurgers(),
                FryFries(),
                PourDrinks(),
                ServeAndEat()
            };

            await Task.WhenAll(tasks);
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
