using SqlHelper;
using SqlHelper.Mapping;
using SqlHelper.Mapping.Attributes;
using SqlHelper.Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [Mapper("users")]
    public class OrderMapping :Mapper<Order>
    {
        public OrderMapping(SqlHelperContext sql) : base(sql)
        {
        }

        public async Task<IEnumerable<Order>> TestSelect(OrderParams users)
        {
            return await Query(users, "testSelect");
        }
    }
}
