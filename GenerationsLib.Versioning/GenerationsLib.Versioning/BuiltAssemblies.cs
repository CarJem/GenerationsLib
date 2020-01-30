using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenerationsLib.Versioning
{
    public class BuildList
    {
        public class BuiltAssembly
        {
            public DateTime LastBuilt { get; set; }
            public string Identifier { get; set; }
            public int BuildNumber { get; set; }
        }

        public List<BuiltAssembly> BuiltAssemblies { get; set; }
    }
}
