namespace Xutils.Extensions
{
    using System;
    using System.IO;
    using System.Linq;

    public static class DirectoryInfoExtensions
    {
        public static DirectoryInfo GetOrCreateSubdirectory(this DirectoryInfo directory, string path)
        {
            DirectoryInfo subDirectory = directory.EnumerateDirectories().FirstOrDefault(di => di.Name.Equals(path, StringComparison.InvariantCultureIgnoreCase));
            return subDirectory ?? directory.CreateSubdirectory(path);
        }

        public static DirectoryInfo Clear(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
                file.Delete();
            foreach (DirectoryInfo subDirectory in directory.EnumerateDirectories())
                directory.Delete(true);
            return directory;
        }
    }
}
