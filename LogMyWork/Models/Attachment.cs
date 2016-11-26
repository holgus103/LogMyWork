using LogMyWork.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogMyWork.Models
{
    public class Attachment
    {
        public int AttachmentID { get; set; }

        public int TaskID { get; set; }
        public ProjectTask Task { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
        public FileType Type { get; set; }
    }
}