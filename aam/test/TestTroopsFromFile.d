import aam.Troop;
import aam.TroopsFromFile;
import aam.test.UnitTest;
import std.exception;

unittest
{
    Compare([], TroopsFromInput(new string[0]));
    Compare([Troop("name", 1)], TroopsFromInput([CreateTroopString( )]));
    Compare([Troop("name", 1), Troop("name", 1)],
            TroopsFromInput([CreateTroopString( ), CreateTroopString( )]));

    assertThrown!CannotOpenFile(OpenFile("noSuchFile"));
    {
        std.file.write("temporary", "abc");
        scope(exit) std.file.remove("temporary");
        Compare("abc", OpenFile("temporary").readln( ));
    }

    assertThrown!CannotOpenFile(TroopsFromFile("noSuchFile"));
    {
        std.file.write("temporary", CreateTroopString( ));
        scope(exit) std.file.remove("temporary");
        Compare([Troop("name", 1)], TroopsFromFile("temporary"));
    }
}

string CreateTroopString( )
{
    return "name\t" ~ "1";
}
