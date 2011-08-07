import aam.Troop;

import std.array;
import std.conv;
import std.stdio;
import std.exception;

Troop CreateTroop(string[] tokens)
{
    assert (tokens.length > 0);
    return Troop(tokens[0], to!double(tokens[1]));
}

void HandleTokens(string[] tokens, ref Troop[] troops)
{
    troops ~= CreateTroop(tokens);
}

void HandleLine(string line, ref Troop[] troops)
{
    HandleTokens(split(line, "\t"), troops);
}

class StartupException : Exception
{
    this(string what)
    {
        super(what);
    }
}

class InsufficientArguments : StartupException
{
    this( )
    {
        super("Insufficient amount of arguments");
    }
}

class CannotOpenFile : StartupException
{
    this(string path)
    {
        super("Could not open file '" ~ path ~ "'");
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

File OpenFile(string path)
{
    try
    {
        return File(path);
    }
    catch (ErrnoException e)
    {
    }

    throw new CannotOpenFile(path);
}

auto TroopsFromFile(Input)(Input input)
{
    Troop[] result;
    foreach (string line; input)
    {
        HandleLine(line, result);
    }

    return result;
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

void Usage(T)(T error)
{
    error.writeln("Usage:");
    error.writeln("AamTroops [path]");
    error.writeln("  where [path] is a path to troop information file");
}

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
