namespace TestRunner;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class TestAttribute : Attribute { }


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseAttribute : TestAttribute
{
    public object[] Arguments { get; }
    public TestCaseAttribute(params object[] args)
    {
        Arguments = args;
    }
}


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class TestCaseWithResultAttribute : TestCaseAttribute
{
    //public object[] Arguments { get; }
    public object ExpectedResult { get; }
    public TestCaseWithResultAttribute(object expected, params object[] args) : base(args)
    {
        ExpectedResult = expected;
    }
}

