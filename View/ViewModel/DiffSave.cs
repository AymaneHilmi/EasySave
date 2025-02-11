using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EasySave
{
    public class DiffSave : Save, ISave
    {
        /// <summary>
        /// Constructor who call his parent
        /// </summary>
        public DiffSave(string? appellation, string? sourcePath, string? targetPath) 
            : base(appellation, sourcePath, targetPath)
        {
        }
        
        /// <summary>
        /// Call parent file for do the Save
        /// </summary>
        public void RepositorySave()
        {
            GeneralTools.CreateLogsFiles();
            this.FilesSize = GeneralTools.DirectorySize(this.SourcePath, this.TargetPath);
            if (this.SourcePath != null)
            {
                this.TotalFiles = this.FilesToCopy = Directory.GetFiles(this.SourcePath, "*", SearchOption.AllDirectories).Length;
                CreateDirectoriesOfADirectory();
                CreateState(GeneralTools.conf);
                base.RepositorySave(this.SourcePath, this.TargetPath);
                if (GeneralTools.conf.GetSection("Priority_Ext").Get<List<string>>() != null)
                {
                    this._prioFilesExt = false;                
                    this.RepositorySave(this.SourcePath, this.TargetPath);
                }
            }
        }
    }
}