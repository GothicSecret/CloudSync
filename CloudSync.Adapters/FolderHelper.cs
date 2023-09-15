namespace CloudSync.Adapters;

public static class FolderHelper
{
    public static void EnsureFolderExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public static void CreateFolderStructure(string path)
    {
        path = NormalizePath(path);
        var folders = SplitPathIntoFolders(path);
        var currentPath = string.Empty;

        foreach (var folder in folders)
        {
            currentPath = Path.Combine(currentPath, folder);
            EnsureFolderExists(currentPath);
        }
    }

    public static string[] SplitPathIntoFolders(string path)
    {
        switch (Environment.OSVersion.Platform)
        {
            case PlatformID.Win32NT:
                return path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            case PlatformID.Unix:
                var result = path.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                if (result.Length > 0 && result[0] == "")
                {
                    result[0] = "/";
                }
                return result;

            default: throw new InvalidOperationException($"Not supported OS: {Environment.OSVersion.Platform}");
        }
    }

    public static string NormalizePath(string path)
    {
        return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }
}
