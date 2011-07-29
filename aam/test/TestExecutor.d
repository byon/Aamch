import aam.Executor;
import std.stdio;
import std.exception;

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

void CheckUnequal(T1, T2)(T1 left, T2 right)
{
    const bool COMPARISON = left == right;
    if (!COMPARISON)
    {
        return;
    }

    stderr.writeln("Comparison succeeded '", left, "' == '", right, "'");
    assert(COMPARISON);
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
    void Throw(string[] arguments)
    {
        throw new StartupException("Just for testing");
    }
    auto error = new Output;
    Compare(1, ExecuteAndCatchExceptions(arguments, error, &Throw));
    Compare(ErrorOutput("Just for testing"), error.lines_);
}

void TestExecutionSuccess(string[] arguments)
{
    auto output = new Output;
    Compare(0, ExecuteAndCatchExceptions(arguments, output, (string[]){}));
    Compare([], output.lines_);
}

string CreateTroopString( )
{
    return "name\t" ~ "1";
}

unittest
{
    TestExecutionFailure([]);
    TestExecutionFailure(["exe path"]);
    TestExecutionSuccess(["exe", "deleteme"]);

    CheckUnequal(Attack(1, 0, 0), Attack(1, 1, 1));
    CheckUnequal(Attack(1, 1, 0), Attack(1, 1, 1));
    CheckUnequal(Attack(0, 1, 1), Attack(1, 1, 1));
    Compare(Attack(1, 1, 1), Attack(1, 1, 1));

    Compare([], TroopsFromFile(new string[0]));
    //Compare([Troop("name", 1)], TroopsFromFile([CreateTroopString( )]));

    assertThrown!StartupException(Execute(["", "NosuchFile"]));
}
