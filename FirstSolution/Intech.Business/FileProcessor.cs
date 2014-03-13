﻿using System;
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
            FileProcessorResult result = new FileProcessorResult(path);

            if (Directory.Exists(path))
                ProcessDirectory(path, result);

            return result;
        }

        private void ProcessDirectory(string path, FileProcessorResult result, bool parentIsHidden = false)
        {
            result.TotalDirectoryCount++;

            foreach (var subDirectory in Directory.GetDirectories(path))
                ProcessSubDirectory(subDirectory, result, parentIsHidden);

            foreach (var file in Directory.GetFiles(path))
                ProcessFile(file, result, parentIsHidden);
        }

        private void ProcessSubDirectory(string name, FileProcessorResult result, bool parentIsHidden)
        {
            if ((File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                result.HiddenDirectoryCount++;
                parentIsHidden = true;
            }

            ProcessDirectory(name, result, parentIsHidden);
        }

        private void ProcessFile(string name, FileProcessorResult result, bool parentIsHidden)
        {
            if ((File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
                result.HiddenFileCount++;

            if(parentIsHidden || (File.GetAttributes(name) & FileAttributes.Hidden) == FileAttributes.Hidden)
                result.UnaccessibleFileCount++;

            result.TotalFileCount++;
        }
    }
}
