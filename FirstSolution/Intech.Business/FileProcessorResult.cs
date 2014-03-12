using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intech.Business
{
    public class FileProcessorResult
    {
        readonly string _rootPath;
        readonly DateTime _date;

        internal FileProcessorResult(string rootPath)
        {
            _date = DateTime.Now;
            _rootPath = rootPath;
        }

        public string RootPath
        {
            get { return _rootPath; }
        }

        public DateTime CreationDate
        {
            get { return _date; }
        }

        public int TotalFileCount { get; internal set; }

        public int TotalDirectoryCount { get; internal set; }

        public int HiddenFileCount { get; internal set; }

        public int HiddenDirectoryCount { get; internal set; }

        public int UnaccessibleFileCount { get; internal set; }
    }
}
