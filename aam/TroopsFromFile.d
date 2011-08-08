import aam.Troop;
import aam.StartupException;

import std.array;
import std.conv;
import std.stdio;
import std.exception;

auto TroopsFromFile(string path)
{
    return TroopsFromInput(lines(OpenFile(path)));
}

auto TroopsFromInput(Input)(Input input)
{
    Troop[] result;
    foreach (string line; input)
    {
        HandleLine(line, result);
    }

    return result;
}

private void HandleLine(string line, ref Troop[] troops)
{
    HandleTokens(split(line, "\t"), troops);
}

private void HandleTokens(string[] tokens, ref Troop[] troops)
{
    troops ~= CreateTroop(tokens);
}

private Troop CreateTroop(string[] tokens)
{
    enforce (tokens.length > 2, new NotEnoughTokens);
    return Troop(tokens[0],
                 to!double(tokens[1]),
                 to!uint(tokens[2]),
                 to!uint(tokens[3]),
                 to!uint(tokens[4]),
                 Attack(to!uint(tokens[5]),
                        to!uint(tokens[6]),
                        to!uint(tokens[7])),
                 Attack(to!uint(tokens[8]),
                        to!uint(tokens[9]),
                        to!uint(tokens[10])),
                 tokens[11],
                 tokens[12],
                 tokens[13],
                 to!uint(tokens[14]),
                 tokens[15],
                 to!uint(tokens[16]),
                 tokens[17],
                 tokens[18],
                 tokens[19],
                 tokens[20]);
}

private File OpenFile(string path)
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

private class CannotOpenFile : StartupException
{
    this(string path)
    {
        super("Could not open file '" ~ path ~ "'");
    }
}

private class NotEnoughTokens : StartupException
{
    this( )
    {
        super("Not enough tokens on a line");
    }
}
