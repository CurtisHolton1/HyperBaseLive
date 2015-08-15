using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Models
{
    public class ServiceVersionsResponse
    {
        public int Id { get; set; }
        public string Version { get; set; }
        public string Path { get; set; }
        public string Bytes { get; set; }
    }
}
