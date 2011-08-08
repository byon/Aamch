import std.conv;
import std.math;

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

    bool opEquals(ref const Troop other) const
    {
        return name == other.name &&
               CompareCost(other) &&
               speed == other.speed &&
               frontDefense == other.frontDefense &&
               rearDefense == other.rearDefense &&
               soldierAttack == other.soldierAttack &&
               vehicleAttack == other.vehicleAttack &&
               type == other.type &&
               subType == other.subType &&
               nation == other.nation &&
               year == other.year &&
               specialAbilities == other.specialAbilities &&
               commandValue == other.commandValue &&
               commandEffect == other.commandEffect &&
               rarity == other.rarity &&
               id == other.id &&
               set == other.set;
    }

private:

    bool CompareCost(ref const Troop other) const
    {
        if (cost == other.cost)
        {
            return true;
        }

        return isNaN(cost) && isNaN(other.cost);
    }
}

struct Attack
{
    uint shortDistance;
    uint mediumDistance;
    uint longDistance;

    string toString( )
    {
        return "[" ~
               to!string(shortDistance) ~ " " ~
               to!string(mediumDistance) ~ " " ~
               to!string(longDistance) ~
               "]";
    }
}
