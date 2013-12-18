using System.IO;

namespace Pleiades.App.Utility
{
    public class DirectoryHelpers
    {
        public static void DeleteAll(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                Directory.Delete(directory.FullName, true);
            }
        }

        public static void DeleteAll2(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).ForEach(x => x.Delete());
            directoryInfo.GetDirectories().ForEach(x => x.Delete());
            
        }
    }
}
