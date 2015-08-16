using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperBaseLiveWpf.Models
{
   public class GetServiceVersionResponse
    {      
            public int Id { get; set; }
            public string Version { get; set; }
            public string Path { get; set; }
            public byte[] Bytes { get; set; }
    }
}
