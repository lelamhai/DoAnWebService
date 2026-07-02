using System.Reflection;
using Xunit;

namespace DoAnWebService.Tests;

public static class Program
{
    public static async Task<int> Main()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var testTypes = assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract)
            .ToList();

        var passed = 0;
        var failed = 0;
        var failures = new List<string>();

        foreach (var type in testTypes)
        {
            var testMethods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttributes(typeof(FactAttribute), inherit: true).Any())
                .ToList();

            if (testMethods.Count == 0)
            {
                continue;
            }

            object? instance;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                failed += testMethods.Count;
                failures.Add($"{type.Name}: could not create instance - {ex.Message}");
                continue;
            }

            foreach (var method in testMethods)
            {
                try
                {
                    var result = method.Invoke(instance, null);
                    if (result is Task task)
                    {
                        await task;
                    }

                    passed++;
                    Console.WriteLine($"PASS {type.Name}.{method.Name}");
                }
                catch (TargetInvocationException tie) when (tie.InnerException is not null)
                {
                    failed++;
                    var message = tie.InnerException.Message;
                    failures.Add($"{type.Name}.{method.Name}: {message}");
                    Console.WriteLine($"FAIL {type.Name}.{method.Name} - {message}");
                }
                catch (Exception ex)
                {
                    failed++;
                    failures.Add($"{type.Name}.{method.Name}: {ex.Message}");
                    Console.WriteLine($"FAIL {type.Name}.{method.Name} - {ex.Message}");
                }
            }
        }

        Console.WriteLine();
        Console.WriteLine($"Total: {passed + failed}, Passed: {passed}, Failed: {failed}");

        if (failed > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Failures:");
            foreach (var failure in failures)
            {
                Console.WriteLine(failure);
            }

            return 1;
        }

        return 0;
    }
}
