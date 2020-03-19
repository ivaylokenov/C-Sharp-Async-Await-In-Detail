namespace StateMachine
{
    using System;
    using System.Linq;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Prints all state machines in the console.
            Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(d => d.Name.Contains("d_"))
                .ToList()
                .ForEach(Console.WriteLine);

            await Compiled.MainCompiled(args);
        }

        public static async Task StateMachineCompiled()
        {
            int input = 5;

            await Task.Run(() =>
            {
                Console.WriteLine(input);
            });

            var result = await Task.Run(() => true);

            Console.WriteLine(result);
        }

        public static async Task HardcoreStateMachineCompiled()
        {
            var input = 5;
            var secondInput = Console.ReadLine();

            var result = await Task
                .Run(() =>
                {
                    Console.WriteLine(input);
                })
                .ContinueWith(async task =>
                {
                    await Task
                        .Delay(3000)
                        .ContinueWith(_ =>
                        {
                            Console.WriteLine(secondInput);
                        });
                })
                .ContinueWith(t =>
                {
                    return Task.Run(() => true);
                });

            Console.WriteLine(await result);
        }
    }
}
