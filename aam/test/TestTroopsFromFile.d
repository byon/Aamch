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

    Compare([Expected(1, 1, 0)],
            TroopsFromInput([CreateTroopString("1", "1", "")]));
    Compare([Expected(0)], TroopsFromInput([CreateTroopString("")]));
    Compare([Expected(0)], TroopsFromInput([CreateTroopString("A")]));
    Compare([Expected(1, 0)], TroopsFromInput([CreateTroopString("1", "")]));

    Compare([Expected( )], TroopsFromInput([CreateTroopString( ) ~ "\n"]));
}

private Troop Expected(uint speed = 1, uint defense = 1, uint commandValue = 2)
{
    return Troop("name", 1, speed, defense, defense, Attack(2, 3, 4),
                 Attack(5, 6, 7), "type", "sub", "nation", 1, "abilities",
                 commandValue, "effect", "rarity", "id", "set");
}

private string CreateTroopString(string speed = "1", string defense = "1",
                                 string commandValue = "2")
{
    return "name\t1\t" ~ speed ~ "\t" ~ defense ~ "\t" ~ defense ~
           "\t2\t3\t4\t5\t6\t7\ttype\tsub\tnation\t1"
           "\tabilities\t" ~ commandValue ~ "\teffect\trarity\tid\tset";
}
