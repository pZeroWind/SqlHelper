using SqlHelper;
using SqlHelper.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class DBContext : SqlHelperContext
    {
        public DBContext(string conn, DBTypes? types = DBTypes.MySql) : base(conn, types)
        {
        }

        public OrderMapping? Order;
    }
}
