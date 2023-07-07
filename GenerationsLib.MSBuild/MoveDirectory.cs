using Microsoft.Build.Framework;
using MSBuildTask = Microsoft.Build.Utilities.Task;
using System.IO;
using System;

namespace GenerationsLib.MSBuild
{
    public class MoveDirectory : MSBuildTask
    {
        public override bool Execute()
        {
            try
            {
                var Source = Path.GetFullPath(SourceFolder.ToString());
                var Destination = Path.Combine(DestinationFolder.ToString(), new DirectoryInfo(Source).Name);
                try
                {
                    Directory.Move(Source, Destination);
                }
                catch (Exception ex2)
                {
                    Log.LogError(ex2.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.ToString());
                return false;
            }
        }

        [Required]
        public bool OverwriteReadOnlyFiles { get; set; } = false;

        [Required]
        public ITaskItem SourceFolder { get; set; }

        [Required]
        public ITaskItem DestinationFolder { get; set; }
    }
}
