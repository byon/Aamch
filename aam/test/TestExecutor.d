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

unittest
{
    auto error = new Output;
    assert(1 == ExecuteAndCatchExceptions!Output([], error));
    writeln("output ", error.lines_);
    assert(["Insufficient amount of arguments"] == error.lines_);
    //assert(1 == ExecuteAndCatchExceptions(["exe path"]));
    //assert(0 == ExecuteAndCatchExceptions(["exe path", "1"]));
}
