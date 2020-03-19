namespace CustomAwait
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    public static class Awaiters
    {
        public static TaskAwaiter<string> GetAwaiter(this Uri uri) 
            => new HttpClient().GetStringAsync(uri).GetAwaiter();

        public static TaskAwaiter GetAwaiter(this TimeSpan timeSpan) 
            => Task.Delay(timeSpan).GetAwaiter();

        public static TaskAwaiter GetAwaiter(this int number)
            => TimeSpan.FromMilliseconds(number).GetAwaiter();

        public static TaskAwaiter GetAwaiter(this IEnumerable<Task> tasks)
            => Task.WhenAll(tasks).GetAwaiter();
    }
}
