namespace BurgerPreparation
{
    using System;
    using System.Threading;

    public static class SyncCooker
    {
        public static void Work()
        {
            HeatThePans();
            UnfreezeMeat();
            PeelPotatoes();
            CookBurgers();
            FryFries();
            PourDrinks();
            ServeAndEat();
        }

        public static void HeatThePans()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Pan heated!");
        }

        public static void UnfreezeMeat()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Meat ready!");
        }

        public static void PeelPotatoes()
        {
            Thread.Sleep(2000);
            Console.WriteLine("Potatoes peeled!");
        }

        public static void CookBurgers()
        {
            Thread.Sleep(5000);
            Console.WriteLine("Burgers cooked!");
        }

        public static void FryFries()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Fries fried!");
        }

        public static void PourDrinks()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Drinks poured!");
        }

        public static void ServeAndEat()
        {
            Thread.Sleep(5000);
            Console.WriteLine("Delicious!");
        }
    }
}
