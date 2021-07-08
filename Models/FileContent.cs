using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace asp_fileserver.Models
{
    public class FileContent
    {
        public string FileName { get; set; }
        public Stream FileStream { get; set; }
    }
}
