import aam.Troop;
import aam.TroopsFromFile;
import aam.StartupException;

import std.stdio;

int ExecuteAndCatchExceptions(Error, Executor)(string[] arguments, Error error,
                                               Executor executor)
{
    try
    {
        executor(arguments);
        return 0;
    }
    catch (StartupException e)
    {
        error.writeln(e.msg);
        Usage(error);
    }

    return 1;
}

void Execute(string[] arguments)
{
    Troop[] troops;
    auto file = OpenFile(InputFileName(arguments));
    foreach (string line; lines(file))
    {
        /// @todo What if two chars for end of line?
        HandleLine(line[0..$-1], troops);
    }

    foreach (Troop troop; troops)
    {
        stdout.writeln(troop);
    }
}

string InputFileName(string[] arguments)
{
    if (arguments.length < 2)
    {
        throw new InsufficientArguments;
    }

    return arguments[1];
}

void Usage(T)(T error)
{
    error.writeln("Usage:");
    error.writeln("AamTroops [path]");
    error.writeln("  where [path] is a path to troop information file");
}

class InsufficientArguments : StartupException
{
    this( )
    {
        super("Insufficient amount of arguments");
    }
}
