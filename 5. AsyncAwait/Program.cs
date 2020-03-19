namespace AsyncAwait
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Program
    {
        public static async Task Main()
        {
            await WaitForTask();
            await TaskContinuation();
            await TaskExceptionsAndStatus();
            await MultipleTasksAtTheSameTime();
            await AtLeastOneTaskToFinish();
            await CompletedTaskAndFromResult();
            await DownloadContentAndSaveItToFile();

            // Bonus
            AsyncLambda();
        }

        public static async Task WaitForTask()
        {
            //var firstTask = Task.Run(() =>
            //{
            //    Console.WriteLine("First task");
            //});

            //var secondTask = Task.Run(() => "Second task");

            //Console.WriteLine("Sync write!");

            //firstTask.Wait();

            //var result = secondTask.Result;

            //Console.WriteLine(result);

            var firstTask = Task.Run(() =>
            {
                Console.WriteLine("First task");
            });

            var secondTask = Task.Run(() => "Second task");

            Console.WriteLine("Sync write!");

            await firstTask;

            var result = await secondTask;

            Console.WriteLine(result);
        }

        public static async Task TaskContinuation()
        {
            //var task = Task
            //    .Run(() => "Result")
            //    .ContinueWith(previousTask =>
            //    {
            //        Console.WriteLine(previousTask.Result);
            //    })
            //    .ContinueWith(previousTask => Task.Delay(2000).Wait())
            //    .ContinueWith(previousTask =>
            //    {
            //        Console.WriteLine("After delay!");
            //    });

            //task.Wait();

            var result = await Task.Run(() => "Result");

            Console.WriteLine(result);

            await Task.Delay(2000);

            Console.WriteLine("After delay!");
        }

        public static async Task TaskExceptionsAndStatus()
        {
            try
            {
                await Task.Run(() => throw new InvalidOperationException("Some exception"));
            }
            catch (InvalidOperationException exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Done");

            //var task = Task
            //    .Run(() => throw new InvalidOperationException("Some exception"))
            //    .ContinueWith(previousTask =>
            //    {
            //        if (previousTask.IsFaulted)
            //        {
            //            Console.WriteLine(previousTask.Exception.Message);
            //        }
            //    })
            //    .ContinueWith(previousTask =>
            //    {
            //        if (previousTask.IsCompletedSuccessfully)
            //        {
            //            Console.WriteLine("Done");
            //        }
            //    });

            //task.Wait();
        }

        public static async Task MultipleTasksAtTheSameTime()
        {
            var firstTask = Task.Run(async () =>
            {
                await Task.Delay(3000);
                Console.WriteLine("First");
            });

            var secondTask = Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Second");
            });

            var thirdTask = Task.Run(async () =>
            {
                await Task.Delay(2000);
                Console.WriteLine("Third");
            });

            await Task.WhenAll(firstTask, secondTask, thirdTask);

            //var firstTask = Task
            //    .Run(() => Task.Delay(3000).Wait())
            //    .ContinueWith(_ => Console.WriteLine("First"));

            //var secondTask = Task
            //    .Run(() => Task.Delay(1000).Wait())
            //    .ContinueWith(_ => Console.WriteLine("Second"));

            //var thirdTask = Task
            //    .Run(() => Task.Delay(2000).Wait())
            //    .ContinueWith(_ => Console.WriteLine("Third"));

            //Task.WaitAll(firstTask, secondTask, thirdTask);
        }

        public static async Task AtLeastOneTaskToFinish()
        {
            Console.WriteLine("You have 5 seconds to solve this: 111 * 111");

            var inputTask = Task.Run(() =>
            {
                while (true)
                {
                    var input = Console.ReadLine();

                    if (input == "12321")
                    {
                        Console.WriteLine("Correct!");
                        break;
                    }

                    Console.WriteLine("Wrong answer!");
                }
            });

            var timerTask = Task.Run(async () =>
            {
                for (var i = 5; i > 0; i--)
                {
                    Console.WriteLine(i);

                    await Task.Delay(1000);
                }
            });

            await Task.WhenAny(inputTask, timerTask);

            //Console.WriteLine("You have 5 seconds to solve this: 111 * 111");

            //var inputTask = Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        var input = Console.ReadLine();

            //        if (input == "12321")
            //        {
            //            Console.WriteLine("Correct!");
            //            break;
            //        }

            //        Console.WriteLine("Wrong answer!");
            //    }
            //});

            //var timerTask = Task.Run(() =>
            //{
            //    for (var i = 5; i > 0; i--)
            //    {
            //        Console.WriteLine(i);

            //        Task.Delay(1000).Wait();
            //    }
            //});

            //Task.WaitAny(inputTask, timerTask);
        }

        public static async Task CompletedTaskAndFromResult()
        {
            while (true)
            {
                var input = Console.ReadLine();

                if (input == "end")
                {
                    break;
                }

                var task = input switch
                {
                    "delay" => Task
                        .Delay(2000)
                        .ContinueWith(_ => Console.WriteLine("Delayed")),

                    "print" => Task
                        .Run(() => Console.WriteLine("Printed!")),

                    "throw" => Task
                        .FromException(new InvalidOperationException("Error"))
                        .ContinueWith(prev => Console.WriteLine(prev.Exception.Message)),

                    "42" => Task
                        .FromResult(42)
                        .ContinueWith(prev => Console.WriteLine(prev.Result)),

                    _ => Task
                        .CompletedTask
                        .ContinueWith(_ => Console.WriteLine("Invalid input!"))
                };

                await task;
            }

            //while (true)
            //{
            //    var input = Console.ReadLine();

            //    if (input == "end")
            //    {
            //        break;
            //    }

            //    var task = input switch
            //    {
            //        "delay" => Task
            //            .Delay(2000)
            //            .ContinueWith(_ => Console.WriteLine("Delayed")),

            //        "print" => Task
            //            .Run(() => Console.WriteLine("Printed!")),

            //        "throw" => Task
            //            .FromException(new InvalidOperationException("Error"))
            //            .ContinueWith(prev => Console.WriteLine(prev.Exception.Message)),

            //        "42" => Task
            //            .FromResult(42)
            //            .ContinueWith(prev => Console.WriteLine(prev.Result)),

            //        _ => Task
            //            .CompletedTask
            //            .ContinueWith(_ => Console.WriteLine("Invalid input!"))
            //    };

            //    task.Wait();
            //}
        }

        public static async Task DownloadContentAndSaveItToFile()
        {
            using var httpClient = new HttpClient();

            var googleTask = httpClient.GetStringAsync("https://google.com");
            var codeLessonsTask = httpClient.GetStringAsync("http://codelessons.online/");
            var myTestedTask = httpClient.GetStringAsync("https://mytestedasp.net");

            var getTasks = new List<Task<string>>
            {
                googleTask, codeLessonsTask, myTestedTask,
            };

            var (google, codeLessons, myTested) = await Task.WhenAll(getTasks);

            var writeFileTasks = new List<Task>
            {
                File.WriteAllTextAsync("google.txt", google),
                File.WriteAllTextAsync("codelessons.txt", codeLessons),
                File.WriteAllTextAsync("mytestedaspnet.txt", myTested)
            };

            await Task.WhenAll(writeFileTasks);

            var content = $"{google}{codeLessons}{myTested}";

            await File.AppendAllTextAsync("downloads.txt", content);

            //using var httpClient = new HttpClient();

            //var googleTask = httpClient
            //    .GetStringAsync("https://google.com")
            //    .ContinueWith(prevTask =>
            //    {
            //        File
            //            .WriteAllTextAsync("google.txt", prevTask.Result)
            //            .Wait();

            //        return prevTask.Result;
            //    });

            //var codeLessonsTask = httpClient
            //    .GetStringAsync("http://codelessons.online/")
            //    .ContinueWith(prevTask =>
            //    {
            //        File
            //            .WriteAllTextAsync("codelessons.txt", prevTask.Result)
            //            .Wait();

            //        return prevTask.Result;
            //    });

            //var myTestedTask = httpClient
            //    .GetStringAsync("https://mytestedasp.net")
            //    .ContinueWith(prevTask =>
            //    {
            //        File
            //            .WriteAllTextAsync("mytestedaspnet.txt", prevTask.Result)
            //            .Wait();

            //        return prevTask.Result;
            //    });

            //var tasks = new[]
            //{
            //    googleTask,
            //    codeLessonsTask,
            //    myTestedTask
            //};

            //Task
            //    .WhenAll(tasks)
            //    .ContinueWith(prevTask =>
            //    {
            //        var content = $"{prevTask.Result[0]}{prevTask.Result[1]}{prevTask.Result[2]}";

            //        File
            //            .AppendAllTextAsync("downloads.txt", content)
            //            .Wait();
            //    })
            //    .Wait();
        }

        public static void AsyncLambda()
        {
            var list = new List<int> { 1, 2, 3, 4, 5 };

            list.ForEach(async number =>
            {
                await Task.Run(() => Console.WriteLine(number));
            });
        }
    }
}
