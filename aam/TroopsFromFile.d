import aam.Troop;
import aam.StartupException;

import std.array;
import std.conv;
import std.stdio;
import std.exception;

auto TroopsFromInput(Input)(Input input)
{
    Troop[] result;
    foreach (string line; input)
    {
        HandleLine(line, result);
    }

    return result;
}

void HandleLine(string line, ref Troop[] troops)
{
    HandleTokens(split(line, "\t"), troops);
}

void HandleTokens(string[] tokens, ref Troop[] troops)
{
    troops ~= CreateTroop(tokens);
}

Troop CreateTroop(string[] tokens)
{
    assert (tokens.length > 0);
    return Troop(tokens[0], to!double(tokens[1]));
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

class CannotOpenFile : StartupException
{
    this(string path)
    {
        super("Could not open file '" ~ path ~ "'");
    }
}
