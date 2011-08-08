import aam.Troop;
import aam.test.UnitTest;
import std.stdio;

unittest
{
    Compare("[12 345 6]", Attack(12, 345, 6).toString( ));

    Troop full = Troop("a", 1.2, 1, 1, 1, Attack(1), Attack(1), "a", "a",
                       "a", 1, "a", 1, "a", "a", "a", "a");
    Compare("a 1.2 1 1 1 [1 0 0] [1 0 0] a a a 1 a 1 a a a a",
            full.toString( ));

    mixin(UnequalityTest("name", "\"a\""));
    mixin(UnequalityTest("cost", "1"));
    mixin(UnequalityTest("speed", "1"));
    mixin(UnequalityTest("frontDefense", "1"));
    mixin(UnequalityTest("rearDefense", "1"));
    mixin(UnequalityTest("soldierAttack", "Attack(1)"));
    mixin(UnequalityTest("vehicleAttack", "Attack(1)"));
    mixin(UnequalityTest("type", "\"a\""));
    mixin(UnequalityTest("subType", "\"a\""));
    mixin(UnequalityTest("nation", "\"a\""));
    mixin(UnequalityTest("year", "1"));
    mixin(UnequalityTest("specialAbilities", "\"a\""));
    mixin(UnequalityTest("commandValue", "1"));
    mixin(UnequalityTest("commandEffect", "\"a\""));
    mixin(UnequalityTest("rarity", "\"a\""));
    mixin(UnequalityTest("id", "\"a\""));
    mixin(UnequalityTest("set", "\"a\""));

    Troop compareTo = Troop("a", 1, 1, 1, 1, Attack(1), Attack(1), "a", "a",
                            "a", 1, "a", 1, "a", "a", "a", "a");
    Troop exactCopy = compareTo;
    Compare(exactCopy, compareTo);
}

private string UnequalityTest(string member, string value)
{
    return "{Troop changed; changed." ~ member ~ " = " ~ value ~ ";" ~
           "CheckUnequal(changed, Troop( ));}";
}
