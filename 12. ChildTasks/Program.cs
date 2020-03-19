namespace ChildTasks
{
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            Detached();
            // Attached();

            // await DetachedAsync();
        }

        public static void Detached()
        {
            Task
                .Factory
                .StartNew(() =>
                {
                    Console.WriteLine("First task detached");

                    Task.Factory.StartNew(() =>
                    {
                        Task.Delay(10000).Wait();
                        Console.WriteLine("Second task detached");
                    });
                })
                .Wait();
        }

        public static void Attached()
        {
            Task
                .Factory
                .StartNew(() =>
                {
                    Console.WriteLine("First task attached");

                    Task.Factory.StartNew(() =>
                    {
                        Task.Delay(2000).Wait();
                        Console.WriteLine("Second task attached");
                    }, TaskCreationOptions.AttachedToParent);
                })
                .Wait();
        }

        private static async Task DetachedAsync()
        {
            await Task.Run(async () =>
            {
                Console.WriteLine("First task async");

                await Task.Run(async () =>
                {
                    await Task.Delay(4000);
                    Console.WriteLine("Second task async");
                });
            });
        }
    }
}
