import std.array;
import std.conv;
import std.stdio;
import std.traits;
import std.exception;

struct Troop
{
    string name;
    double cost;
    uint speed;
    uint frontDefense;
    uint rearDefense;
    Attack soldierAttack;
    Attack vehicleAttack;
    string type;
    string subType;
    string nation;
    uint year;
    string specialAbilities;
    uint commandValue;
    string commandEffect;
    string rarity;
    string id;
    string set;

    string toString( )
    {
        return name ~ " " ~
               to!string(cost) ~ " " ~
               to!string(speed) ~ " " ~
               to!string(frontDefense) ~ " " ~
               to!string(rearDefense) ~ " " ~
               to!string(soldierAttack) ~ " " ~
               to!string(vehicleAttack) ~ " " ~
               type ~ " " ~
               subType ~ " " ~
               nation ~ " " ~
               to!string(year) ~ " " ~
               specialAbilities ~ " " ~
               to!string(commandValue) ~ " " ~
               commandEffect ~ " " ~
               rarity ~ " " ~
               id ~ " " ~
               set;
    }
}

struct Attack
{
    uint shortDistance;
    uint mediumDistance;
    uint longDistance;

    string toString( )
    {
        return to!string(shortDistance) ~ " " ~
               to!string(mediumDistance) ~ " " ~
               to!string(longDistance);
    }
}

T To(T)(string source)
{
    static if (__traits(isArithmetic, T))
    {
        if (source.length == 0)
        {
            return T.init;
        }
    }

    return to!T(source);
}

Troop CreateTroop(string[] tokens)
{
    assert (tokens.length > 0);
    return Troop(tokens[0], To!double(tokens[1]));
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

void ExperimentWithMemberList(Troop[] troops)
{
    auto types = __traits(allMembers, Troop);
    foreach (t; types)
    {
        if (isSomeFunction!(t))
        {
            writeln("Skipping method ", t);
        }
        writeln(typeid(t), " ", t);
    }
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
