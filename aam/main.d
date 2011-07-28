import aam.Executor;
import std.stdio;

int main(string[] arguments)
{
    return ExecuteAndCatchExceptions!File(arguments);
}
