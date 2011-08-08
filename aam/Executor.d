import aam.Troop;
import aam.TroopsFromFile;
import aam.StartupException;

import std.exception;
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

private void Usage(T)(T error)
{
    error.writeln("Usage:");
    error.writeln("AamTroops [path]");
    error.writeln("  where [path] is a path to troop information file");
}

private void OutputTroops(Troop[] troops)
{
    foreach (Troop troop; troops)
    {
        stdout.writeln(troop);
    }
}

private string InputFileName(string[] arguments)
{
    enforce(arguments.length > 1, new InsufficientArguments);

    return arguments[1];
}

private class InsufficientArguments : StartupException
{
    this( )
    {
        super("Insufficient amount of arguments");
    }
}
