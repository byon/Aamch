import aam.Executor;
import aam.StartupException;
import aam.Troop;
import aam.test.UnitTest;
import std.exception;

unittest
{
    TestExecutionFailure([]);
    TestExecutionFailure(["executable path"]);
    TestExecutionSuccess(["executable path", "supposedly a path"]);

    void Sink(Troop[] troops) {}

    int callCount = 0;
    Troop[] NoTroops(string path)
    {
        ++callCount;
        Compare("path", path);
        Troop[] result;
        return result;
    }

    Execute(&NoTroops, ["", "path"], &Sink);
    Compare(1, callCount);

    Troop[] OneTroop(string path)
    {
        return [Troop("a", 1.0)];
    }

    callCount = 0;
    void Store(Troop[] troops)
    {
        ++callCount;
        Compare([Troop("a", 1.0)], troops);
    }

    Execute(&OneTroop, ["", ""], &Store);
    Compare(1, callCount);
}

private void TestExecutionFailure(string[] arguments)
{
    void Throw(Troop[] function(string), string[], void function(Troop[]))
    {
        throw new StartupException("Just for testing");
    }
    auto error = new Output;
    Compare(1, ExecuteAndCatchExceptions(arguments, error, &Throw));
    Compare(ErrorOutput("Just for testing"), error.lines_);
}

private void TestExecutionSuccess(string[] arguments)
{
    void NoOp(Troop[] function(string), string[], void function(Troop[])) {}
    auto output = new Output;
    Compare(0, ExecuteAndCatchExceptions(arguments, output, &NoOp));
    Compare([], output.lines_);
}

private string[] ErrorOutput(string reason)
{
    string[] usage = ["Usage:",
                      "AamTroops [path]",
                      "  where [path] is a path to troop information file"];
    return [reason] ~ usage;
}

private class Output
{
    string[] lines_;

    void writeln(string line)
    {
        lines_ ~= line;
    }
}
