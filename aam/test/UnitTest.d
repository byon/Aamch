import std.stdio;
import std.exception;

void CheckUnequal(T1, T2)(T1 left, T2 right)
{
    const bool COMPARISON = left == right;
    if (!COMPARISON)
    {
        return;
    }

    stderr.writeln("Comparison succeeded '", left, "' == '", right, "'");
    assert(COMPARISON);
}

void Compare(T1, T2)(T1 left, T2 right)
{
    const bool COMPARISON = left == right;
    if (COMPARISON)
    {
        return;
    }

    stderr.writeln("Comparison failed '", left, "' != '", right, "'");
    assert(COMPARISON);
}
