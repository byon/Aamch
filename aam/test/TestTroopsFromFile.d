import aam.Troop;
import aam.TroopsFromFile;
import aam.test.UnitTest;

string CreateTroopString( )
{
    return "name\t" ~ "1";
}

unittest
{
    Compare([], TroopsFromFile(new string[0]));
    Compare([Troop("name", 1)], TroopsFromFile([CreateTroopString( )]));
    Compare([Troop("name", 1), Troop("name", 1)],
            TroopsFromFile([CreateTroopString( ), CreateTroopString( )]));
}
