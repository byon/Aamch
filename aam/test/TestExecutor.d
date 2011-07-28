import aam.Executor;

unittest
{
    assert(1 == ExecuteAndCatchExceptions([]));
    //assert(1 == ExecuteAndCatchExceptions(["exe path"]));
    //assert(0 == ExecuteAndCatchExceptions(["exe path", "1"]));
}
