using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Core
{
    public static class FileExtend
    {
        public static bool TryCreateDirectory(this string filePath)
        {
            // todo: log
            var dir = Path.GetDirectoryName(filePath);
            if (dir?.Length > 0)
            {
                if (Directory.Exists(dir))
                {
                    return true;
                }
                Directory.CreateDirectory(dir);
            }                        
            return Directory.Exists(dir);
        }
    }
}
