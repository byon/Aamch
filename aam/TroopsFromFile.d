import aam.Troop;
import aam.StartupException;

import std.array;
import std.conv;
import std.exception;
import std.stdio;
import std.string;

auto TroopsFromFile(string path)
{
    return TroopsFromInput(lines(OpenFile(path)));
}

auto TroopsFromInput(Input)(Input input)
{
    Troop[] result;
    foreach (int i, string line; input)
    {
        auto e = collectException!ParseError(HandleLine(chomp(line), result));
        CheckFailure(e, i);
    }

    return result;
}

void CheckFailure(ParseError exception, int line)
{
    if (exception)
    {
        exception.SetLine(line + 1);
        throw exception;
    }
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

    mixin(Conversion("name"));
    mixin(Conversion("cost"));
    mixin(ConversionSpeed( ));
    mixin(ConversionAllowingEmpty("frontDefense"));
    mixin(ConversionAllowingEmpty("rearDefense"));
    mixin(Conversion("soldierAttack.shortDistance"));
    mixin(Conversion("soldierAttack.mediumDistance"));
    mixin(Conversion("soldierAttack.longDistance"));
    mixin(Conversion("vehicleAttack.shortDistance"));
    mixin(Conversion("vehicleAttack.mediumDistance"));
    mixin(Conversion("vehicleAttack.longDistance"));
    mixin(Conversion("type"));
    mixin(Conversion("subType"));
    mixin(Conversion("nation"));
    mixin(Conversion("year"));
    mixin(Conversion("specialAbilities"));
    mixin(ConversionAllowingEmpty("commandValue"));
    mixin(Conversion("commandEffect"));
    mixin(Conversion("rarity"));
    mixin(Conversion("id"));
    mixin(Conversion("set"));

    return result;
}

private string ConversionSpeed( )
{
    return "auto t = tokens[i];if (\"\" == t || \"A\" == t) { result.speed"
           " = 0; ++i; } else {" ~ Conversion("speed") ~ "}";
}

private string ConversionAllowingEmpty(string field)
{
    return "if (0 == tokens[i].length) { result." ~ field ~
           " = 0; ++i; } else {" ~ Conversion(field) ~ "}";
}

private string Conversion(string field)
{
    return "Convert(tokens[i++], result." ~ field ~ ", \"" ~ field ~ "\");";
}

private void Convert(T)(string source, ref T target, string field)
{
    try
    {
        target = to!T(source);
        return;
    }
    catch (ConvException e)
    {
    }

    throw new InvalidType(source, to!string(typeid(target)), field);
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
    this(string value, string type, string field)
    {
        super("Cannot convert '" ~ value ~ "' to " ~ type ~ " when reading " ~
              field);
    }
}

private class NotEnoughTokens : ParseError
{
    this( )
    {
        super("Not enough tokens");
    }
}
