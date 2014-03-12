using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intech.Business
{
    public class FileProcessor
    {
        Result result;

        public FileProcessor()
        {
        }

        public Result Process(String path)
        {
            result = new Result();

            if (Directory.Exists(path))
                ProcessDirectory(path);
            else
                throw new Exception("Directory not exists");

            return result;
        }

        private void ProcessDirectory(string path, bool parentIsHidden = false)
        {
            string[] subDirectories = Directory.GetDirectories(path);
            foreach (var subDirectory in subDirectories)
                ProcessSubDirectory(subDirectory, parentIsHidden);

            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
                ProcessFile(file, parentIsHidden);
        }

        private void ProcessSubDirectory(string name, bool parentIsHidden)
        {
            if ((File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                result.HiddenDirectoryCount++;
                parentIsHidden = true;
            }

            result.TotalDirectoryCount++;
            ProcessDirectory(name, parentIsHidden);
        }

        private void ProcessFile(string name, bool parentIsHidden)
        {
            if (parentIsHidden)
                result.UnaccessibleFileCount++;

            if ((File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                result.HiddenFileCount++;
                if (!parentIsHidden)
                    result.UnaccessibleFileCount++;
            }

            result.TotalFileCount++;
        }
    }
}
