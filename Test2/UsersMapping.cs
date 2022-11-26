using SqlHelper.Mapping;
using SqlHelper.Mapping.Attributes;
using SqlHelper.Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2
{
    [Mapper("users")]
    public class UsersMapping :Mapper<Users>
    {
        public UsersMapping(DbConnection db) : base(db)
        {
        }

        public async Task<IEnumerable<Users>> TestSelect(UserParams users)
        {
            return await Query(users, "testSelect");
        }
    }
}
