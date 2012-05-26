using System;
using System.Collections.Generic;

namespace BitBucketSharp.Models
{
    public class EmailsModel : List<EmailModel> { }

    public class EmailModel
    {
        public bool Active { get; set; }
        public String Email { get; set; }
        public bool Primary { get; set; }
    }
}
