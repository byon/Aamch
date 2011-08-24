module grid.winmain;

import core.runtime;
import std.c.windows.windows;

import grid.main;

extern (Windows)
int WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
    int result = 1;

    void exceptionHandler(Throwable e)
    {
        throw e;
    }

    try
    {
        Runtime.initialize(&exceptionHandler);

        string[] arguments;
        main(arguments);

        Runtime.terminate(&exceptionHandler);
    }
    catch (Throwable o)		// catch any uncaught exceptions
    {
        MessageBoxA(null, cast(char *)o.toString(), "Error", MB_OK | MB_ICONEXCLAMATION);
        result = 0;		// failed
    }

    return result;
}
