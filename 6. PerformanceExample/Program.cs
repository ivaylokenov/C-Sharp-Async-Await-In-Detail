namespace PerformanceExample
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Running;
    using SixLabors.ImageSharp;
    using SixLabors.ImageSharp.Processing;

    public class Program
    {
        private const int NumberOfCpus = 12;

        private const string ImagesDirectory = @"C:\Data\Projects\Code It Up\Source\1. C# Async-Await In Detail\6. PerformanceExample\Images";
        private const string CpuSyncDirectory = ImagesDirectory + @"\CpuSync\";
        private const string CpuAsyncDirectory = ImagesDirectory + @"\CpuAsync\";
        private const string CpuAsyncMultipleDirectory = ImagesDirectory + @"\CpuAsyncMultiple\";
        private const string MoreThanCpuSyncDirectory = ImagesDirectory + @"\MoreThanCpuSync\";
        private const string MoreThanCpuAsyncMultipleDirectory = ImagesDirectory + @"\MoreThanCpuAsyncMultiple\";

        public static async Task Main()
        {
            //var benchmark = BenchmarkRunner.Run<Program>();

            //Console.WriteLine(benchmark);

            var program = new Program();

            var stopWatch = Stopwatch.StartNew();

            program.ResizeCpuSynchronously();

            Console.WriteLine($"CPU Sync - {stopWatch.Elapsed}");

            stopWatch = Stopwatch.StartNew();

            await program.ResizeCpuAsynchronously();

            Console.WriteLine($"CPU Async - {stopWatch.Elapsed}");

            stopWatch = Stopwatch.StartNew();

            await program.ResizeCpuAsynchronouslyMultipleTasks();

            Console.WriteLine($"CPU Async Multiple Tasks - {stopWatch.Elapsed}");

            stopWatch = Stopwatch.StartNew();

            program.ResizeMoreThanCpuSynchronously();

            Console.WriteLine($"More Than CPU Sync - {stopWatch.Elapsed}");

            stopWatch = Stopwatch.StartNew();

            await program.ResizeMoreThanCpuAsynchronouslyMultipleTasks();

            Console.WriteLine($"More Than CPU Async - {stopWatch.Elapsed}");
        }

        [Benchmark]
        public void ResizeCpuSynchronously()
        {
            var dir = CpuSyncDirectory;

            Directory.CreateDirectory(dir);

            var files = ReadFiles().Take(NumberOfCpus);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                ProcessImage(file, dir + $"{fileInfo.Name}");
            }

            Directory.Delete(dir, true);
        }

        [Benchmark]
        public async Task ResizeCpuAsynchronously()
        {
            var dir = CpuAsyncDirectory;

            Directory.CreateDirectory(dir);

            var files = ReadFiles().Take(NumberOfCpus);

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                await Task.Run(() =>
                {
                    ProcessImage(file, dir + $"{fileInfo.Name}");
                });
            }

            Directory.Delete(dir, true);
        }

        [Benchmark]
        public async Task ResizeCpuAsynchronouslyMultipleTasks()
        {
            var dir = CpuAsyncMultipleDirectory;

            Directory.CreateDirectory(dir);

            var files = ReadFiles().Take(NumberOfCpus);

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                tasks.Add(Task.Run(() =>
                {
                    ProcessImage(file, dir + $"{fileInfo.Name}");
                }));
            }

            await Task.WhenAll(tasks);

            Directory.Delete(dir, true);
        }

        [Benchmark]
        public void ResizeMoreThanCpuSynchronously()
        {
            var dir = MoreThanCpuSyncDirectory;

            Directory.CreateDirectory(dir);

            var files = ReadFiles();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                ProcessImage(file, dir + $"{fileInfo.Name}");
            }

            Directory.Delete(dir, true);
        }

        [Benchmark]
        public async Task ResizeMoreThanCpuAsynchronouslyMultipleTasks()
        {
            var dir = MoreThanCpuAsyncMultipleDirectory;

            Directory.CreateDirectory(dir);

            var files = ReadFiles();

            var tasks = new List<Task>();

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                tasks.Add(Task.Run(() =>
                {
                    ProcessImage(file, dir + $"{fileInfo.Name}");
                }));
            }

            await Task.WhenAll(tasks);

            Directory.Delete(dir, true);
        }

        private static IEnumerable<string> ReadFiles()
            => Directory.GetFiles(ImagesDirectory);

        private static void ProcessImage(string inputPath, string outputPath)
        {
            using var image = Image.Load(inputPath);

            image.Mutate(x => x.Resize(2000, 2000));

            image.Save(outputPath);
        }
    }
}
