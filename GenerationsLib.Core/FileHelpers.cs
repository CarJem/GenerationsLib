using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GenerationsLib.Core
{
    public static class FileHelpers
    {
        public static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists)
            {
                destination.Create();
            }

            // Copy all files.
            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName,
                    file.Name));
            }

            // Process subdirectories.
            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Get destination directory.
                string destinationDir = Path.Combine(destination.FullName, dir.Name);

                // Call CopyDirectory() recursively.
                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }

        public static void DeleteFilesFiltered(DirectoryInfo source, List<string> folderNamesToIgnore = null)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (!(folderNamesToIgnore != null && folderNamesToIgnore.Contains(dir.Name)))
                {
                    dir.Delete(true);
                }
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.Delete();
            }

        }

        public static void MoveFilesRecursively(DirectoryInfo source, DirectoryInfo target, List<string> folderNamesToIgnore = null)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (!(folderNamesToIgnore != null && folderNamesToIgnore.Contains(dir.Name)))
                {
                    MoveFilesRecursivelyInternal(dir, target.CreateSubdirectory(dir.Name));
                }
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.MoveTo(Path.Combine(target.FullName, file.Name));
            }

        }

        private static void MoveFilesRecursivelyInternal(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                MoveFilesRecursivelyInternal(dir, target.CreateSubdirectory(dir.Name));
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.MoveTo(Path.Combine(target.FullName, file.Name));
            }
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, List<string> folderNamesToIgnore = null)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                if (!(folderNamesToIgnore != null && folderNamesToIgnore.Contains(dir.Name)))
                {
                    CopyFilesRecursivelyInternal(dir, target.CreateSubdirectory(dir.Name));
                }
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name));
            }

        }

        private static void CopyFilesRecursivelyInternal(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursivelyInternal(dir, target.CreateSubdirectory(dir.Name));
            }

            foreach (FileInfo file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name));
            }
        }
    }
}
