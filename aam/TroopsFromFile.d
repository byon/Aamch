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
    foreach (int i, string line; input)
    {
        try
        {
            HandleLine(line, result);
        }
        catch (ParseError e)
        {
            e.SetLine(i + 1);
            throw e;
        }
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

private class ParseError : StartupException
{
    this(string what)
    {
        super(what);
    }

    void SetLine(uint line)
    {
        msg ~= " on line " ~ to!string(line);
    }
}

private class NotEnoughTokens : ParseError
{
    this( )
    {
        super("Not enough tokens");
    }
}
