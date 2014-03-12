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
        public FileProcessor()
        {
        }

        public FileProcessorResult Process(String path)
        {
            FileProcessorResult result = new FileProcessorResult();

            if (Directory.Exists(path))
                ProcessDirectory(path, result);
            else
                throw new Exception("Directory not exists");

            return result;
        }

        private void ProcessDirectory(string path, FileProcessorResult result, bool parentIsHidden = false)
        {
            string[] subDirectories = Directory.GetDirectories(path);
            foreach (var subDirectory in subDirectories)
                ProcessSubDirectory(subDirectory, result, parentIsHidden);

            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
                ProcessFile(file, result, parentIsHidden);
        }

        private void ProcessSubDirectory(string name, FileProcessorResult result, bool parentIsHidden)
        {
            if ((File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                result.HiddenDirectoryCount++;
                parentIsHidden = true;
            }

            result.TotalDirectoryCount++;
            ProcessDirectory(name, result, parentIsHidden);
        }

        private void ProcessFile(string name, FileProcessorResult result, bool parentIsHidden)
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
