using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudStorage
{
    public class FileItem
    {
        public int FileID { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public DateTime UploadedOn { get; set; }
    }
}
