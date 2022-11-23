using SqlHelper.Enums;
using SqlHelper.Structs;
using System.Reflection;
using System.Text;

namespace SqlHelper
{
    public static class QueryCriteria
    {
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder Where<T>(this SqlBuilder sqlBuilder, T? parameters)
        {
            if (parameters != null)
            {
                var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null && Props.First() == prop)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"WHERE {prop.Name} = '{value}'");
                    }
                    else if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"AND {prop.Name} = '{value}'");
                    }
                }
            }
            else
            {
                sqlBuilder.SQL.AppendLine("WHERE 1=1");
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder Where(this SqlBuilder sqlBuilder)
        {
            sqlBuilder.SQL.AppendLine("WHERE 1=1");
            return sqlBuilder;
        }

        /// <summary>
        /// 条件-And
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder And<T>(this SqlBuilder sqlBuilder, T parameters)
        {
            if (parameters != null)
            {
                var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"AND {prop.Name} = '{value}'");
                    }
                }
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 条件-Or
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder Or<T>(this SqlBuilder sqlBuilder, T parameters)
        {
            if (parameters != null)
            {
                var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"OR {prop.Name} = '{value}'");
                    }
                }
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="colunmNames"></param>
        /// <returns></returns>
        public static SqlBuilder GroupBy(this SqlBuilder sqlBuilder,params string[] colunmNames)
        {
            foreach (var col in colunmNames)
            {
                if (col == colunmNames.First())
                    sqlBuilder.SQL.AppendLine($"GROUP BY {col}");
                else
                    sqlBuilder.SQL.AppendLine(","+col);
            }
            return sqlBuilder;
        }

        /// <summary>
        /// Having语句
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder Having<T>(this SqlBuilder sqlBuilder, T? parameters)
        {
            if (parameters != null)
            {
                var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null && Props.First() == prop)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"HAVING {prop.Name} = '{value}'");
                    }
                    else if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"AND {prop.Name} = '{value}'");
                    }
                }
            }
            else
            {
                sqlBuilder.SQL.AppendLine("HAVING 1=1");
            }
            return sqlBuilder;
        }

        /// <summary>
        /// Having语句
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder Having(this SqlBuilder sqlBuilder)
        {
            sqlBuilder.SQL.AppendLine("HAVING 1=1");
            return sqlBuilder;
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="colunmName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static SqlBuilder OrderBy(this SqlBuilder sqlBuilder, params OrderBy[] orderByModel)
        {
            sqlBuilder.SQL.AppendLine($"ORDER BY " +
                $"{string.Join(',', orderByModel.Select(p=> $"{p.ColunmName} {Enum.GetName(p.Type)}"))} ");
            return sqlBuilder;
        }

        /// <summary>
        /// Limit语句(MySql)
        /// </summary>
        /// <returns></returns>
        public static SqlBuilder Limit(this SqlBuilder sqlBuilder,params int[] param)
        {
            if (param.Length > 0)
            {
                sqlBuilder.SQL.AppendLine($"LIMIT {string.Join(',', param.Take(2))}");
            }
            return sqlBuilder;
        }

        /// <summary>
        /// Offset语句(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public static SqlBuilder Offset(this SqlBuilder sqlBuilder, int param)
        {
            sqlBuilder.SQL.AppendLine($"OFFSET {param} ROWS");
            return sqlBuilder;
        }

        /// <summary>
        /// Fetch(SqlServer2012以上版本可用)
        /// </summary>
        /// <returns></returns>
        public static SqlBuilder Fetch(this SqlBuilder sqlBuilder, int param)
        {
            sqlBuilder.SQL.AppendLine($"Fetch {param} ROWS ONLY");
            return sqlBuilder;
        }

        /// <summary>
        /// 连接查询
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static SqlBuilder JoinOn(this SqlBuilder sqlBuilder,string tableName, string where, JoinType type = JoinType.INNER)
        {
            sqlBuilder.SQL.AppendLine($"{Enum.GetName(type)} JOIN {tableName} ON {where}");
            return sqlBuilder;
        }
    }
}