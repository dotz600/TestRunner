# Custom C# Test Runner

## 🎯 Overview

This project is a **custom C# test runner** built from scratch to demonstrate test discovery, execution, and result reporting without relying on any external test frameworks (like NUnit, xUnit, or MSTest).

It supports:
- ✅ Discovery of methods marked with `[Test]`, `[TestCase]`, or `[TestCaseWithResult]`
- ✅ Parameterized test execution
- ✅ Validation against expected results
- ✅ Exception handling and failure reporting
- ✅ Console output and file logging (`TestResults.txt`)
- ✅ Summary of passed and failed tests

## 🏗️ Project Structure

| File                   | Purpose                                  |
|------------------------|------------------------------------------|
| `Program.cs`           | Main test runner logic                   |
| `TestAttribute.cs`     | Custom attributes for defining tests     |
| `Calculator.cs`        | Example test class with various methods  |
| `TestResults.txt`      | Output log written after execution       |

## 🧪 How to Run

1. Open the solution in Visual Studio or any IDE that supports .NET
2. Build the project
3. Run the application:
   ```bash
   dotnet run
   ```

After execution:
- The test results will be printed to the console
- A `TestResults.txt` file will be created in the project root directory with the same content

## 🧾 Test Syntax

### ✅ No-parameter test
```csharp
[Test]
public void SimpleTest() { ... }
```

### ✅ Parameterized test
```csharp
[TestCase(1, 2)]
public void Multiply(int a, int b) { ... }
```

### ✅ Test with expected result
```csharp
[TestCaseWithResult(3, 1, 2)]
public int Add(int a, int b) => a + b;
```

## ✅ Output Example

```
[PASS] Add(1, 2) => 3
[FAIL] Divide(1, 0): Attempted to divide by zero.
[FAIL] Func1(): I am Always Throwing Exception:)

========== TEST SUMMARY ==========
Total Tests:   3
Passed:        1
Failed:        2
==================================
File Saved in C:\Path\To\Project\TestResults.txt
```

## 🧠 Design Decisions

- Attributes are used to mimic NUnit/xUnit-style `[Test]`, `[TestCase]`, and `[TestCase(ExpectedResult, ...)]`
- Reflection is used for test discovery
- `TargetInvocationException` is handled to unwrap test failures
- A `StringBuilder` logs results for output and file writing

## 📁 Notes

- File output path is resolved to the root of the project using `AppContext.BaseDirectory` and navigating up three directories
- Randomized tests like `Func3()` may occasionally fail by design (to simulate flaky test behavior)
```
