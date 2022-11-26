using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Mapping.Attributes
{
    public class MapperAttribute:Attribute
    {
        public MapperAttribute(string mapperName = "")
        {
            MapperName = mapperName;
        }

        public string MapperName { get; set; }
    }
}
