import aam.Executor;
import aam.Troop;
import std.stdio;

int main(string[] arguments)
{
    alias void function(Troop[]) Output;
    return ExecuteAndCatchExceptions(arguments, stderr, &Execute!Output);
}
