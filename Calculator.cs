namespace TestRunner;

internal class Calculator
{
    [TestCase (1, 2)]
    [TestCaseWithResult(3, 1, 2)]
    [TestCaseWithResult(4, 1, 2)]
    public int Add(int x, int y) => x + y;
    
    [TestCase(2, 2)]
    [TestCaseWithResult(0, 1, 1)]
    public int Subtract(int x, int y) => x - y;

    [TestCase(2, 2)]
    [TestCaseWithResult(2, 1, 2)]
    public int Multiply(int x, int y) => x * y;

    [Test]
    [TestCase(2, 2)]
    [TestCaseWithResult(0, 1, 2)]
    public int Divide(int x, int y) => x / y;

    [TestCase(2, 2)]
    [TestCaseWithResult(3, 1, 2)]
    public int Func1() => throw new Exception("I am always throwing an exception.");

    [Test]
    [TestCase(2, 2)]
    [TestCaseWithResult(3, 1, 2)]
    public void Func2() { }

    [Test]
    public int Func3()
    {
        Random random = new();
        int randNum = random.Next(3);
        if (randNum == 2) throw new Exception("Hi! it's 33% chance to throw exception");

        return 1;
    }

}
