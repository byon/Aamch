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

unittest
{
    auto error = new Output;
    assert(1 == ExecuteAndCatchExceptions!Output([], error));
    Compare(["Insufficient amount of arguments"], error.lines_);
    //assert(1 == ExecuteAndCatchExceptions(["exe path"]));
    //assert(0 == ExecuteAndCatchExceptions(["exe path", "1"]));
}
