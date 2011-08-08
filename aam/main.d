import aam.Executor;
import aam.Troop;
import std.stdio;

int main(string[] arguments)
{
    alias void function(Troop[]) Output;
    alias Troop[] function( ) Input;
    return ExecuteAndCatchExceptions(arguments, stderr,
                                     &Execute!(Input, Output));
}
