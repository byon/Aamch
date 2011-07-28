import aam.Executor;
import std.stdio;

class Output
{
    string[] lines_;

    void writeln(string line)
    {
        lines_ ~= line;
    }
}

void Compare(T1, T2)(T1 left, T2 right)
{
    const bool COMPARISON = left == right;
    if (COMPARISON)
    {
        return;
    }

    stderr.writeln("Comparison failed '", left, "' != '", right, "'");
    assert(COMPARISON);
}

void TestExecutionFailure(string[] arguments, string[] expectedOutput)
{
    auto error = new Output;
    Compare(1, ExecuteAndCatchExceptions!Output(arguments, error));
    Compare(expectedOutput, error.lines_);
}

unittest
{
    TestExecutionFailure([], ["Insufficient amount of arguments"]);
    TestExecutionFailure(["exe path"], ["Insufficient amount of arguments"]);
    //assert(1 == ExecuteAndCatchExceptions(["exe path"]));
    //assert(0 == ExecuteAndCatchExceptions(["exe path", "1"]));
}
