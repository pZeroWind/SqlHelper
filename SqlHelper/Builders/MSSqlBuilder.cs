using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Builders
{
    public class MSSqlBuilder : BaseSqlBuilder
    {
        private MSSqlBuilder() { }

        #region 静态实例化方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MSSqlBuilder InstanceSelect<T>(params string[] tableName) where T : class
        {
            return (MSSqlBuilder)new MSSqlBuilder().Select<T>(tableName);
        }

        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MSSqlBuilder InstanceSelect(string tableName, params string[] colNames)
        {
            return (MSSqlBuilder)new MSSqlBuilder().Select(tableName, colNames);
        }

        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MSSqlBuilder InstanceUpdate<T>(string tableName, T parameters)
        {
            return (MSSqlBuilder)new MSSqlBuilder().Update(tableName, parameters);
        }

        /// <summary>
        /// 删除实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MSSqlBuilder InstanceDelete(string tableName)
        {
            return (MSSqlBuilder)new MSSqlBuilder().Delete(tableName);
        }
        #endregion

        #region 数据库专用语句
        /// <summary>
        /// Offset语句(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public MSSqlBuilder Offset(int param)
        {
            SQL.AppendLine($"OFFSET {param} ROWS");
            return this;
        }

        /// <summary>
        /// Fetch(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public MSSqlBuilder Fetch(int param)
        {
            SQL.AppendLine($"Fetch {param} ROWS ONLY");
            return this;
        }
        #endregion
    }
}
