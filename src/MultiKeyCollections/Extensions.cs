using System;

namespace TableCollections
{
    internal static class ExceptionHandling
    {
#if NETSTANDARD2_1 || NET472 || NET481
        public static void ThrowIfNull(object argument, string paramName = null)
        {
            if (argument == null)
                throw new ArgumentNullException(paramName ?? string.Empty);
        }
#else
        public static void ThrowIfNull(object? argument, string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);
        }
#endif
    }
}