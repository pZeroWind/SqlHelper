using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper
{
    public class SqlBuilder
    {
        private readonly StringBuilder _sql;
        internal StringBuilder SQL { get=>_sql; }
        public SqlBuilder ()
        {
            _sql = new StringBuilder();
        }

        /// <summary>
        /// 构建SQL语句
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return _sql.ToString().ToUpper();
        }

        /// <summary>
        /// 联结SQL语句
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public SqlBuilder Union(SqlBuilder builder)
        {
            _sql.AppendLine("UNION").AppendLine(builder.SQL.ToString());
            return this;
        }

        /// <summary>
        /// 联结SQL语句-不进行去重排序
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public SqlBuilder UnionALL(SqlBuilder builder)
        {
            _sql.AppendLine("UNION ALL").AppendLine(builder.SQL.ToString());
            return this;
        }
    }
}
