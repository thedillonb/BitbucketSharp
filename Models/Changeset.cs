using System.Collections.Generic;

namespace BitBucketSharp.Models
{
    public class ChangesetsModel
    {
        public int Count { get; set; }
        public string Start { get; set; }
        public int Limit { get; set; }
    }

    public class ChangesetModel
    {
        public string Node { get; set; }
        public string Author { get; set; }
        public string Timestamp { get; set; }
        public string Branch { get; set; }
        public string Message { get; set; }
        public int Revision { get; set; }
        public int Size { get; set; }
        public List<FileModel> Files { get; set; } 

        public class FileModel
        {
            public string Type { get; set; }
            public string File { get; set; }
        }
    }
}
