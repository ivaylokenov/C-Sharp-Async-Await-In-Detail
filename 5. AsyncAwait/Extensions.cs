namespace AsyncAwait
{
    using System;

    public static class Extensions
    {
        public static void Deconstruct<T>(this T[] items, out T first, out T second, out T third)
        {
            if (items.Length < 3)
            {
                throw new InvalidOperationException("Items must be at least 3 to deconstruct them with this method.");
            }

            first = items[0];
            second = items[1]; 
            third = items[2];
        }
    }
}
