namespace Gotchas
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            RunAsyncVoid();
            // AsyncVoidLambda(); // Remove the async and see.

            // await NestedTasks();
        }

        public static void RunAsyncVoid()
        {
            try
            {
                AsyncVoid();
            }
            catch
            {
                Console.WriteLine("Cannot be caught!");
            }
        }

        public static void AsyncVoidLambda()
        {
            try
            {
                var list = new List<int> { 1, 2, 3, 4, 5 };

                list.ForEach(async number =>
                {
                    await Task.Run(() => Console.WriteLine(number));

                    throw new InvalidOperationException("In a lambda!");
                });
            }
            catch
            {
                Console.WriteLine("Cannot be caught!");
            }
        }

        public static async void AsyncVoid()
        {
            await Task.Run(() => Console.WriteLine("Message"));

            throw new InvalidOperationException("Error");
        }

        public static async Task NestedTasks()
        {
            await Task.Run(async () =>
            {
                Console.WriteLine("Before Delay");

                await Task.Delay(1000);

                Console.WriteLine("After Delay");
            });

            Console.WriteLine("After Task");
        }
    }
}
