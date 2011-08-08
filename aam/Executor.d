import aam.Troop;
import aam.TroopsFromFile;
import aam.StartupException;

import std.stdio;

int ExecuteAndCatchExceptions(Error, Executor)(string[] arguments, Error error,
                                               Executor executor)
{
    try
    {
        executor(arguments, &OutputTroops);
        return 0;
    }
    catch (StartupException e)
    {
        error.writeln(e.msg);
        Usage(error);
    }

    return 1;
}

void Execute(Sink)(string[] arguments, Sink sink)
{
    sink(TroopsFromFile(InputFileName(arguments)));
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
