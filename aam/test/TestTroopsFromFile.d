import aam.Troop;
import aam.TroopsFromFile;
import aam.test.UnitTest;
import std.exception;

unittest
{
    assertThrown!CannotOpenFile(TroopsFromFile("noSuchFile"));
    {
        std.file.write("temporary", CreateTroopString( ));
        scope(exit) std.file.remove("temporary");
        Compare([Expected( )], TroopsFromFile("temporary"));
    }

    assertThrown!NotEnoughTokens(TroopsFromInput(["notEnough"]));
    Compare("Not enough tokens on line 1",
            collectExceptionMsg!NotEnoughTokens(TroopsFromInput(["foo"])));

    const auto invalid = ["name\tfoo\t1"];
    assertThrown!InvalidType(TroopsFromInput(invalid));
    Compare("Cannot convert 'foo' to double when reading cost on line 1",
            collectExceptionMsg!InvalidType(TroopsFromInput(invalid)));

    Compare([], TroopsFromInput(new string[0]));
    Compare([Expected( )], TroopsFromInput([CreateTroopString( )]));
    Compare([Expected( ), Expected( )],
            TroopsFromInput([CreateTroopString( ), CreateTroopString( )]));
}

private Troop Expected( )
{
    return Troop("name", 1, 1, 1, 1, Attack(2, 3, 4), Attack(5, 6, 7),
                 "type", "sub", "nation", 1, "abilities", 2, "effect",
                 "rarity", "id", "set");
}

private string CreateTroopString( )
{
    return "name\t1\t1\t1\t1\t2\t3\t4\t5\t6\t7\ttype\tsub\tnation\t1"
           "\tabilities\t2\teffect\trarity\tid\tset";
}
