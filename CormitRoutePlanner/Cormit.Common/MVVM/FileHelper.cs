#region

using System.IO;

#endregion

namespace Imarda.Lib.MVVM
{
    public static class FileHelper
    {
        public static bool TryDeleteFiles(string directoryPath, string fileWildcard, bool createDirectory = true)
        {
            try
            {
                var dinfo = new DirectoryInfo(directoryPath);
                if (dinfo.Exists)
                {
                    FileInfo[] fileInfos = dinfo.GetFiles(fileWildcard);
                    foreach (FileInfo fi in fileInfos)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    if (createDirectory)
                    {
                        dinfo.Create();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}