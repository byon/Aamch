import aam.Executor;
import aam.StartupException;
import aam.Troop;
import aam.test.UnitTest;
import std.exception;

class Output
{
    string[] lines_;

    void writeln(string line)
    {
        lines_ ~= line;
    }
}

string[] ErrorOutput(string reason)
{
    string[] usage = ["Usage:",
                      "AamTroops [path]",
                      "  where [path] is a path to troop information file"];
    return [reason] ~ usage;
}

void TestExecutionFailure(string[] arguments)
{
    void Throw(string[], void function(Troop[]))
    {
        throw new StartupException("Just for testing");
    }
    auto error = new Output;
    Compare(1, ExecuteAndCatchExceptions(arguments, error, &Throw));
    Compare(ErrorOutput("Just for testing"), error.lines_);
}

void TestExecutionSuccess(string[] arguments)
{
    void DoNothing(string[], void function(Troop[])) {}
    auto output = new Output;
    Compare(0, ExecuteAndCatchExceptions(arguments, output, &DoNothing));
    Compare([], output.lines_);
}

string CreateTroopString( )
{
    return "name\t" ~ "1";
}

unittest
{
    TestExecutionFailure([]);
    TestExecutionFailure(["executable path"]);
    TestExecutionSuccess(["executable path", "supposedly a path"]);

    void Sink(Troop[] troops) {}

    assertThrown!StartupException(Execute(["", "NosuchFile"], &Sink));

    /// @todo refactor this to happen without actually reading file
    Troop[] result;
    void Store(Troop[] troops)
    {
        result = troops;
    }
    std.file.write("temporary", "a\t1.0\n");
    scope(exit) std.file.remove("temporary");
    Execute(["", "temporary"], &Store);
    Compare([Troop("a", 1.0)], result);
}

