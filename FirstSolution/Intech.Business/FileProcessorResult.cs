using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Intech.Business
{
    public class FileProcessorResult
    {
        public int TotalFileCount { get; internal set; }

        public int TotalDirectoryCount { get; internal set; }

        public int HiddenFileCount { get; internal set; }

        public int HiddenDirectoryCount { get; internal set; }

        public int UnaccessibleFileCount { get; internal set; }
    }
}
