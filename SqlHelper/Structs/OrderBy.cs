using SqlHelper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Structs
{
    public struct OrderBy
    {
        public static OrderBy Instance(string col, OrderType type = OrderType.ASC)
        {
            return new OrderBy
            {
                ColunmName = col,
                Type = type
            };
        }
        public string ColunmName { get; set; }

        public OrderType Type { get; set; }
    }
}
