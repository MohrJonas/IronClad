using System.Runtime.InteropServices;

namespace Mohr.Jonas.IronClad;

public static partial class CInterop
{
    [LibraryImport("libc")]
    private static partial uint getuid();

    [LibraryImport("libc")]
    private static partial uint getgid();

    public static int GetUserId() => (int)getuid();

    public static int GetGroupId() => (int)getgid();
}