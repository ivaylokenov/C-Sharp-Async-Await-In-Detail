namespace CustomAwait
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            var content = await new Uri("https://mytestedasp.net");
            Console.WriteLine(content);

            await TimeSpan.FromSeconds(3);

            await 1000; // In milliseconds.

            await new List<Task>
            {
                Task.Delay(1000),
                Task.Delay(2000)
            };
        }
    }
}
