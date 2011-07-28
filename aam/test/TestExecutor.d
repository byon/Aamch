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

string[] ErrorOutput(string reason)
{
    string[] usage = ["Usage:",
                      "AamTroops [path]",
                      "  where [path] is a path to troop information file"];
    return [reason] ~ usage;
}

void TestExecutionFailure(string[] arguments, string expectedReason)
{
    auto error = new Output;
    Compare(1, ExecuteAndCatchExceptions!(Output)(arguments, error));
    Compare(ErrorOutput(expectedReason), error.lines_);
}

void TestExecutionSuccess(string[] arguments)
{
    auto output = new Output;
    Compare(0, ExecuteAndCatchExceptions!(Output)(arguments, output));
    Compare([], output.lines_);
}

struct Foo
{
    this(string bar) {}
}

unittest
{
    TestExecutionFailure([], "Insufficient amount of arguments");
    TestExecutionFailure(["exe path"], "Insufficient amount of arguments");

    auto file = File("deleteme", "w");
    scope(exit) std.file.remove("deleteme");
    TestExecutionSuccess(["exe", "deleteme"]);
}
