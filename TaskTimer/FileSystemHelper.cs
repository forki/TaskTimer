using System.IO;

namespace TaskTimer
{
    public class FileSystemHelper
    {
        public static void CreateDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}