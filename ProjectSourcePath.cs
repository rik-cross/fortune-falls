// Credit to Mike Nakis https://stackoverflow.com/a/66285728

using System.Runtime.CompilerServices;

internal static class ProjectSourcePath
{
    private const string myRelativePath = nameof(ProjectSourcePath) + ".cs";
    private static string lazyValue;
    public static string Value => lazyValue ??= CalculatePath();

    private static string CalculatePath()
    {
        string pathName = GetSourceFilePathName();
        //Assert(pathName.EndsWith(myRelativePath, StringComparison.Ordinal));
        return pathName.Substring(0, pathName.Length - myRelativePath.Length);
    }

    public static string GetSourceFilePathName([CallerFilePath] string callerFilePath = null) //
        => callerFilePath ?? "";
}

    