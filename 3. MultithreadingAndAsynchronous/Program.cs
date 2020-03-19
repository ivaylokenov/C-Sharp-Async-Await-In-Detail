namespace MultithreadingAndAsynchronous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public class Program
    {
        private static readonly List<int> Data = Enumerable.Range(0, 100).ToList();

        public static void Main()
        {
            for (int i = 0; i < 4; i++)
            {
                var thread = new Thread(Work);
                thread.Start();

                //if (i % 2 == 0)
                //{
                //    thread.Join();
                //}
            }

            Console.WriteLine(Data.Count);
        }

        public static void Work()
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(i);

                Thread.Sleep(500);

                //if (Data.Count > 90)
                //{
                //    Data.RemoveAt(Data.Count - 1);
                //}
            }
        }
    }
}
