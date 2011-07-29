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

string UnequalityTest(string member, string value)
{
    return "{Troop changed; changed." ~ member ~ " = " ~ value ~ ";" ~
           "CheckUnequal(changed, Troop( ));}";
}

unittest
{
    TestExecutionFailure([]);
    TestExecutionFailure(["executable path"]);
    TestExecutionSuccess(["executable path", "supposedly a path"]);

    mixin(UnequalityTest("name", "\"a\""));
    mixin(UnequalityTest("cost", "1"));
    mixin(UnequalityTest("speed", "1"));
    mixin(UnequalityTest("frontDefense", "1"));
    mixin(UnequalityTest("rearDefense", "1"));
    mixin(UnequalityTest("soldierAttack", "Attack(1)"));
    mixin(UnequalityTest("vehicleAttack", "Attack(1)"));
    mixin(UnequalityTest("type", "\"a\""));
    mixin(UnequalityTest("subType", "\"a\""));
    mixin(UnequalityTest("nation", "\"a\""));
    mixin(UnequalityTest("year", "1"));
    mixin(UnequalityTest("specialAbilities", "\"a\""));
    mixin(UnequalityTest("commandValue", "1"));
    mixin(UnequalityTest("commandEffect", "\"a\""));
    mixin(UnequalityTest("rarity", "\"a\""));
    mixin(UnequalityTest("id", "\"a\""));
    mixin(UnequalityTest("set", "\"a\""));

    Troop compareTo = Troop("a", 1, 1, 1, 1, Attack(1), Attack(1), "a", "a",
                            "a", 1, "a", 1, "a", "a", "a", "a");
    Troop exactCopy = compareTo;
    Compare(exactCopy, compareTo);

    Compare([], TroopsFromFile(new string[0]));
    Compare([Troop("name", 1)], TroopsFromFile([CreateTroopString( )]));
    Compare([Troop("name", 1), Troop("name", 1)],
            TroopsFromFile([CreateTroopString( ), CreateTroopString( )]));

    assertThrown!StartupException(Execute(["", "NosuchFile"]));
}
