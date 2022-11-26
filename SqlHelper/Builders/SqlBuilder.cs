using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Builders
{
    public class SqlBuilder:BaseSqlBuilder
    {
        private SqlBuilder() { }
        #region 静态实例化方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceSelect<T>(params string[] tableName) where T : class
        {
            return (SqlBuilder)new SqlBuilder().Select<T>(tableName);
        }

        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceSelect(string tableName, params string[] colNames)
        {
            return (SqlBuilder)new SqlBuilder().Select(tableName, colNames);
        }

        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceUpdate<T>(string tableName, T parameters)
        {
            return (SqlBuilder)new SqlBuilder().Update(tableName, parameters);
        }

        /// <summary>
        /// 删除实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static SqlBuilder InstanceDelete(string tableName)
        {
            return (SqlBuilder)new SqlBuilder().Delete(tableName);
        }
        #endregion

        #region 数据库专用语句
        /// <summary>
        /// Limit语句(MySql)
        /// </summary>
        /// <returns></returns>
        public SqlBuilder Limit(params int[] param)
        {
            if (param.Length > 0)
            {
                SQL.AppendLine($"LIMIT {string.Join(',', param.Take(2))}");
            }
            return this;
        }

        /// <summary>
        /// Offset语句(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public SqlBuilder Offset(int param)
        {
            SQL.AppendLine($"OFFSET {param} ROWS");
            return this;
        }

        /// <summary>
        /// Fetch(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public SqlBuilder Fetch(int param)
        {
            SQL.AppendLine($"Fetch {param} ROWS ONLY");
            return this;
        }
        #endregion
    }
}
