import aam.Troop;
import aam.test.UnitTest;
import std.stdio;

string UnequalityTest(string member, string value)
{
    return "{Troop changed; changed." ~ member ~ " = " ~ value ~ ";" ~
           "CheckUnequal(changed, Troop( ));}";
}

unittest
{
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
