using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent
{
    public class Dict
    {
        public Dict(string name, Dict? parent)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; set; }
        public Dict? Parent { get; set; }
        public ICollection<Dict> SubDirectories { get; set; } = new List<Dict>();
        public ICollection<SystemFile> Files { get; set; } = new List<SystemFile>();
        public int Size { get; set; }
    }
    public class SystemFile
    {
        public SystemFile(string name, long size)
        {
            Name = name;
            Size = size;
        }

        private string Name { get; set; }
        private long Size { get; set; }
    }
}
