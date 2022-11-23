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
        private SqlBuilder ()
        {
            _sql = new StringBuilder();
        }

        #region 静态实例化方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceSelect<T>(params string[] tableName) where T : class
        {
            return new SqlBuilder().Select<T>(tableName);
        }

        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceSelect(string tableName, params string[] colNames)
        {
            return new SqlBuilder().Select(tableName, colNames);
        }

        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceUpdate<T>(string tableName, T parameters)
        {
            return new SqlBuilder().Update(tableName, parameters);
        }

        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceDelete(string tableName)
        {
            return new SqlBuilder().Delete(tableName);
        }
        #endregion

        /// <summary>
        /// 构建SQL语句
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return _sql.ToString();
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
