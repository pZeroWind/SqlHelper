using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Builders
{
    public class MySqlBuilder : BaseSqlBuilder
    {
        private MySqlBuilder() { }
        #region 静态实例化方法
        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MySqlBuilder InstanceSelect<T>(params string[] tableName) where T : class
        {
            return (MySqlBuilder)new MySqlBuilder().Select<T>(tableName);
        }

        /// <summary>
        /// 查询实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MySqlBuilder InstanceSelect(string tableName, params string[] colNames)
        {
            return (MySqlBuilder)new MySqlBuilder().Select(tableName, colNames);
        }

        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MySqlBuilder InstanceUpdate<T>(string tableName, T parameters)
        {
            return (MySqlBuilder)new MySqlBuilder().Update(tableName, parameters);
        }

        /// <summary>
        /// 删除实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static MySqlBuilder InstanceDelete(string tableName)
        {
            return (MySqlBuilder)new MySqlBuilder().Delete(tableName);
        }
        #endregion

        #region 数据库专用语句
        /// <summary>
        /// Limit语句(MySql)
        /// </summary>
        /// <returns></returns>
        public MySqlBuilder Limit(params int[] param)
        {
            if (param.Length > 0)
            {
                SQL.AppendLine($"LIMIT {string.Join(',', param.Take(2))}");
            }
            return this;
        }
        #endregion
    }
}
