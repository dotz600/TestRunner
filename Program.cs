using System.Reflection;
using System.Text;
using TestRunner;

int passed = 0, failed = 0;
StringBuilder fileOutput = new();
var projectRoot = AppContext.BaseDirectory;
var dir = Directory.GetParent(projectRoot)?.Parent?.Parent?.Parent;
var filePath = Path.Combine(dir!.FullName, "TestResults.txt");

//find all test classes
var testAssembly = Assembly.GetExecutingAssembly();
var testClasses = testAssembly.GetTypes()
    .Where(t => t.GetMethods().Any(
        m => m.GetCustomAttributes<TestAttribute>().Any()));

foreach (var testClass in testClasses)
{
    object instance = Activator.CreateInstance(testClass)
        ?? throw new NullReferenceException("Failed to create instance of test class");

    //find all test method
    var testMethods = testClass.GetMethods()
    .Where(m => m.GetCustomAttributes<TestAttribute>().Any());


    foreach (var method in testMethods)
    {
        //find all the attributes of each method
        var testCases = method.GetCustomAttributes<TestAttribute>().ToArray();
        string testMsg = "";
        foreach (var tc in testCases)
        {
            switch (tc)
            {
                case TestCaseWithResultAttribute resultTc:
                    testMsg = RunTest(ref passed, ref failed, instance, method, resultTc.Arguments, resultTc.ExpectedResult);
                    break;
                case TestCaseAttribute testCase:
                    testMsg = RunTest(ref passed, ref failed, instance, method, testCase.Arguments, null);
                    break;
                case TestAttribute:
                    testMsg = RunTest(ref passed, ref failed, instance, method, null, null);
                    break;
                default:
                    testMsg = $"[Warning] Method '{method.Name}' has an unknown or unsupported test attribute: {tc.GetType().Name}";
                    Console.WriteLine(testMsg);
                    break;
            }
            fileOutput.AppendLine(testMsg);
        }
    }
}

//print summary to console and all the data into file
string summary = "========== Test Summary ==========\n" +
                $"Total Tests:   {passed + failed}\n" +
                $"Passed:        {passed}\n" +
                $"Failed:        {failed}\n" +
                "==================================";
Console.WriteLine(summary);
fileOutput.AppendLine(summary);
File.WriteAllText(filePath, fileOutput.ToString());
Console.WriteLine($"Test results saved to {filePath}");


/// <summary>
/// Executes a test method and returns the result message
/// </summary>
static string RunTest(ref int passed, ref int failed, object instance,
    MethodInfo method, object[]? args, object? expected)
{
    string msg ="";
    try
    {
        var res = method.Invoke(instance, args);

        // Verify result if expected value was provided
        if (expected != null)
            if (res == null || !res.Equals(expected))
                throw new Exception($"Expected: {expected}, but got: {res}");

        passed++;
        Console.ForegroundColor = ConsoleColor.Green;
        msg = $"[PASS] {method.Name}({FormatArgs(args)}) => {res}";
        return msg;
    }
    catch (Exception e)
    {
        // Extract the actual exception message from TargetInvocationException
        var exceptionMsg = e is TargetInvocationException tie
            ? tie.InnerException?.Message ?? e.Message
            : e.Message;

        failed++;
        Console.ForegroundColor = ConsoleColor.Red;
        msg = $"[FAIL] {method.Name}({FormatArgs(args)}): {exceptionMsg}";
        return msg;
    }
    finally
    {
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}

/// <summary>
/// Formats an array of arguments for display
/// </summary>
static string FormatArgs(object[]? args)
{
    if (args == null || args.Length == 0) return "()";
    return string.Join(", ", args.Select(a => a?.ToString() ?? "null"));
}
