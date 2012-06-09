﻿using System;
using System.Collections.Generic;

namespace BitbucketSharp.Models
{
    public class BranchModel
    {
        public string Node { get; set; }
        public List<FileModel> Files { get; set; }
        public string RawAuthor { get; set; }
        public string Author { get; set; }
        public string Timestamp { get; set; }
        public string RawNode { get; set; }
        public List<string> Parents { get; set; }
        public string Branch { get; set; }
        public string Message { get; set; }
        public string Revision { get; set; }
        public int Size { get; set; }

        public class FileModel
        {
            public string Type { get; set; }
            public string File { get; set; }
        }
    }


}
