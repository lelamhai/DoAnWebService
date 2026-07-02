namespace Xunit;

[AttributeUsage(AttributeTargets.Method)]
public sealed class FactAttribute : Attribute
{
}

public static class Assert
{
    public static T IsType<T>(object? value)
    {
        if (value is T typed)
        {
            return typed;
        }

        throw new Exception($"Expected type {typeof(T).Name}, got {value?.GetType().Name ?? "null"}.");
    }

    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
        {
            throw new Exception($"Expected '{expected}', got '{actual}'.");
        }
    }

    public static void NotEqual<T>(T notExpected, T actual)
    {
        if (EqualityComparer<T>.Default.Equals(notExpected, actual))
        {
            throw new Exception($"Did not expect '{actual}'.");
        }
    }

    public static void True(bool condition)
    {
        if (!condition)
        {
            throw new Exception("Expected condition to be true.");
        }
    }

    public static void False(bool condition)
    {
        if (condition)
        {
            throw new Exception("Expected condition to be false.");
        }
    }

    public static void Null(object? value)
    {
        if (value is not null)
        {
            throw new Exception("Expected value to be null.");
        }
    }

    public static void NotNull(object? value)
    {
        if (value is null)
        {
            throw new Exception("Expected value to be not null.");
        }
    }
}
