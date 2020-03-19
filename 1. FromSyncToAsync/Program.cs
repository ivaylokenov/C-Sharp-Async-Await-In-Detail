namespace FromSyncToAsync
{
    using System;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main()
        {
            SimpleExample();
            ComplexExample();
        }

        public static void SimpleExample()
        {
            var task = Task.Run(() => Console.WriteLine("First!"));

            Console.WriteLine("Second");

            task.Wait();
        }

        public static void ComplexExample()
        {
            var task = Task
                .Run(() => Task
                    .Delay(2000)
                    .ContinueWith(t => "In a task"));

            Task.Delay(4000).Wait();
            Console.WriteLine("Outside of a task!");

            var completion = Task
                .WhenAll(task)
                .ContinueWith(async t =>
                {
                    Console.WriteLine((await t)[0]);
                });

            completion.Wait();
        }
    }
}
