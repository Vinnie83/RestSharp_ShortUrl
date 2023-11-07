using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpAPITests
{
    internal class Urls
    {
        public string url { get; set; }
        public string shortCode { get; set; }
        public string shortUrl { get; set; }
        public string dateCreated { get; set; }

        public int visits { get; set; }
        public bool message { get; internal set; }
    }
}
