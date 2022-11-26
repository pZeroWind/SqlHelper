using SqlHelper.Builders;
using SqlHelper.Enums;
using SqlHelper.Structs;
using System.Reflection;
using System.Text;

namespace SqlHelper.Builders
{
    public static class QueryCriteria
    {
        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="parameters"></param>
        public static T Where<T, S>(this T sqlBuilder, S? parameters)
            where T : BaseSqlBuilder
            where S : class
        {
            if (parameters != null)
            {
                var Props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null && Props.First() == prop)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"WHERE {prop.Name} {value}");
                    }
                    else if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"AND {prop.Name} {value}");
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
        /// <param name="SqlBuilder"></param>
        /// <param name="parameters"></param>
        public static T Where<T>(this T sqlBuilder)
             where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine("WHERE 1=1");
            return sqlBuilder;
        }

        /// <summary>
        /// 条件-And
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="parameters"></param>
        public static T And<T, S>(this T sqlBuilder, S parameters)
            where T : BaseSqlBuilder
            where S : class
        {
            if (parameters != null)
            {
                var Props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"AND {prop.Name} {value}");
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
        public static T Or<T, S>(this T sqlBuilder, S parameters)
            where T : BaseSqlBuilder
            where S : class
        {
            if (parameters != null)
            {
                var Props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                foreach (PropertyInfo prop in Props)
                {
                    if (prop.GetValue(parameters) != null)
                    {
                        string? value = prop.GetValue(parameters)!.ToString();
                        sqlBuilder.SQL.AppendLine($"OR {prop.Name} {value}");
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
        public static T BetweenAnd<T>(this T sqlBuilder, string col, string start, string end)
            where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine($"AND {col} BETWEEN '{start}' AND '{end}'");
            return sqlBuilder;
        }

        /// <summary>
        /// 分组
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="colunmNames"></param>
        /// <returns></returns>
        public static T GroupBy<T>(this T sqlBuilder, params string[] colunmNames)
            where T : BaseSqlBuilder
        {
            foreach (var col in colunmNames)
            {
                if (col == colunmNames.First())
                    sqlBuilder.SQL.AppendLine($"GROUP BY {col}");
                else
                    sqlBuilder.SQL.AppendLine("," + col);
            }
            return sqlBuilder;
        }

        /// <summary>
        /// Having语句
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static T Having<T, S>(this T sqlBuilder, S? parameters)
            where T : BaseSqlBuilder
            where S : class
        {
            if (parameters != null)
            {
                var Props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
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
        /// <param name="SqlBuilder"></param>
        /// <param name="parameters"></param>
        public static T Having<T>(this T sqlBuilder)
            where T : BaseSqlBuilder
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
        public static T OrderBy<T>(this T sqlBuilder, params OrderBy[] orderByModel)
            where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine($"ORDER BY " +
                $"{string.Join(',', orderByModel.Select(p => $"{p.ColunmName} {Enum.GetName(p.Type)}"))} ");
            return sqlBuilder;
        }

        /// <summary>
        /// 连接查询
        /// </summary>
        /// <param name="SqlBuilder"></param>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static T JoinOn<T>(this T sqlBuilder, string tableName, string where, JoinType type = JoinType.INNER)
            where T : BaseSqlBuilder
        {
            sqlBuilder.SQL.AppendLine($"{Enum.GetName(type)} JOIN {tableName} ON {where}");
            return sqlBuilder;
        }
    }
}