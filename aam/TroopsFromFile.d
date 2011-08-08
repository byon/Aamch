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

    Troop result;
    uint i = 0;

    Convert(tokens[i++], result.name);
    Convert(tokens[i++], result.cost);
    Convert(tokens[i++], result.speed);
    Convert(tokens[i++], result.frontDefense);
    Convert(tokens[i++], result.rearDefense);
    Convert(tokens[i++], result.soldierAttack.shortDistance);
    Convert(tokens[i++], result.soldierAttack.mediumDistance);
    Convert(tokens[i++], result.soldierAttack.longDistance);
    Convert(tokens[i++], result.vehicleAttack.shortDistance);
    Convert(tokens[i++], result.vehicleAttack.mediumDistance);
    Convert(tokens[i++], result.vehicleAttack.longDistance);
    Convert(tokens[i++], result.type);
    Convert(tokens[i++], result.subType);
    Convert(tokens[i++], result.nation);
    Convert(tokens[i++], result.year);
    Convert(tokens[i++], result.specialAbilities);
    Convert(tokens[i++], result.commandValue);
    Convert(tokens[i++], result.commandEffect);
    Convert(tokens[i++], result.rarity);
    Convert(tokens[i++], result.id);
    Convert(tokens[i++], result.set);

    return result;
}

private void Convert(T)(string source, ref T target)
{
    try
    {
        target = to!T(source);
        return;
    }
    catch (ConvException e)
    {
    }

    throw new InvalidType(source, to!string(typeid(target)));
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

private class InvalidType : ParseError
{
    this(string value, string type)
    {
        super("Token '" ~ value ~ "' is not of expected type " ~ type);
    }
}

private class NotEnoughTokens : ParseError
{
    this( )
    {
        super("Not enough tokens");
    }
}
