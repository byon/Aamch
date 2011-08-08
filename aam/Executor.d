import aam.Troop;
import aam.TroopsFromFile;
import aam.StartupException;

import std.stdio;

int ExecuteAndCatchExceptions(Error, Executor)(string[] arguments, Error error,
                                               Executor executor)
{
    try
    {
        executor(&TroopsFromFile, arguments, &OutputTroops);
        return 0;
    }
    catch (StartupException e)
    {
        error.writeln(e.msg);
        Usage(error);
    }

    return 1;
}

void Execute(Input, Sink)(Input input, string[] arguments, Sink sink)
{
    sink(input(InputFileName(arguments)));
}

void Usage(T)(T error)
{
    error.writeln("Usage:");
    error.writeln("AamTroops [path]");
    error.writeln("  where [path] is a path to troop information file");
}

void OutputTroops(Troop[] troops)
{
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

class InsufficientArguments : StartupException
{
    this( )
    {
        super("Insufficient amount of arguments");
    }
}
