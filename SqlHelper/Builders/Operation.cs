using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper.Builders
{
    public static class Operation
    {
        /// <summary>
        /// 直接装填Sql
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="sql"></param>
        public static T SQL<T, S>(this T SqlBuilder, string sql = "", S? parameters = null)
            where T : BaseSqlBuilder
            where S : class
        {
            if (parameters != null)
            {
                var props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in props)
                {
                    if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sql!.Replace($"?:{prop.Name}",
                        $"{prop.Name}='{(value == null ? "" : value)}'");
                    }
                }
            }
            SqlBuilder.SQL.AppendLine(sql);
            return SqlBuilder;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        internal static BaseSqlBuilder Select<T>(this BaseSqlBuilder SqlBuilder, params string[] tableName) where T : class
        {
            SqlBuilder.SQL.AppendLine("SELECT");
            var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo p in Props)
            {
                if (Props.Last() == p)
                {
                    SqlBuilder.SQL.AppendLine($"{p.Name}");
                }
                else
                {
                    SqlBuilder.SQL.AppendLine($"{p.Name},");
                }
            }
            SqlBuilder.SQL.AppendLine($"FROM {string.Join(',', tableName)} ");
            return SqlBuilder;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        internal static BaseSqlBuilder Select(this BaseSqlBuilder sqlBuilder, string tableName, params string[] colNames)
        {
            if (colNames.Length > 0)
                sqlBuilder.SQL.AppendLine($"SELECT {string.Join(',', colNames)} FROM {tableName} ");
            else
                sqlBuilder.SQL.AppendLine($"SELECT * FROM {tableName} ");
            return sqlBuilder;
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        internal static BaseSqlBuilder Update<T>(this BaseSqlBuilder sqlBuilder, string tableName, T parameters)
        {
            sqlBuilder.SQL.AppendLine($"UPDATE {tableName} SET ");
            var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo prop in Props)
            {
                if (prop.GetValue(parameters) != null && prop == Props.First())
                {
                    string? value = prop.GetValue(parameters)!.ToString();
                    sqlBuilder.SQL.AppendLine($"{prop.Name}= '{value}'");
                }
                else if (prop.GetValue(parameters) != null)
                {
                    string? value = prop.GetValue(parameters)!.ToString();
                    sqlBuilder.SQL.AppendLine($",{prop.Name}= '{value}'");
                }
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 删除语句
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="table"></param>
        internal static BaseSqlBuilder Delete(this BaseSqlBuilder sqlBuilder, string tableName)
        {
            sqlBuilder.SQL.AppendLine($"DELETE FROM {tableName} ");
            return sqlBuilder;
        }

        #region 联结SQL语句
        /// <summary>
        /// 联结SQL语句
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T Union<T>(this T sqlBuilder, BaseSqlBuilder builder)
            where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine("UNION").AppendLine(builder.SQL.ToString());
            return sqlBuilder;
        }

        /// <summary>
        /// 联结SQL语句-不进行去重排序
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static T UnionALL<T>(this T sqlBuilder, BaseSqlBuilder builder)
            where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine("UNION ALL").AppendLine(builder.SQL.ToString());
            return sqlBuilder;
        }
        #endregion

        /// <summary>
        /// 注入参数
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="parameters"></param>
        [Obsolete("建议直接使用SQL方法进行参数填充")]
        public static BaseSqlBuilder SetParameter<T>(this BaseSqlBuilder SqlBuilder, T parameters)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.GetValue(parameters) != null)
                {
                    string? value = prop.GetValue(parameters)!.ToString();
                    SqlBuilder.SQL.Replace($"?[{prop.Name}]",
                    $"{prop.Name}='{(value == null ? "" : value)}'");
                }
            }
            return SqlBuilder;
        }
    }
}
