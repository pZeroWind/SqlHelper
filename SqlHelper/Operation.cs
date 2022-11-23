﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlHelper
{
    public static class Operation
    {
        /// <summary>
        /// 装填Sql
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="sql"></param>
        public static SqlBuilder SQL(this SqlBuilder sqlBuilder, string sql)
        {
            sqlBuilder.SQL.AppendLine(sql);
            return sqlBuilder;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        public static SqlBuilder Select<T>(this SqlBuilder sqlBuilder, params string[] tableName) where T : class
        {
            sqlBuilder.SQL.AppendLine("SELECT ");
            var Props = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo p in Props)
            {
                if (Props.Last() == p)
                {
                    sqlBuilder.SQL.AppendLine($"{p.Name}");
                }
                else
                {
                    sqlBuilder.SQL.AppendLine($"{p.Name},");
                }
            }
            sqlBuilder.SQL.AppendLine($" FROM {string.Join(',',tableName)} ");
            return sqlBuilder;
        }

        /// <summary>
        /// 查询语句
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="tableName"></param>
        /// <param name="colNames"></param>
        /// <returns></returns>
        public static SqlBuilder Select(this SqlBuilder sqlBuilder, string tableName, params string[] colNames)
        {
            sqlBuilder.SQL.AppendLine($"SELECT {string.Join(',', colNames)} FROM {tableName} ");
            return sqlBuilder;
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        public static SqlBuilder Update<T>(this SqlBuilder sqlBuilder, string tableName, T parameters)
        {
            sqlBuilder.SQL.AppendLine($"UPDATE INTO {tableName} SET ");
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.GetValue(parameters) != null)
                {
                    string? value = prop.GetValue(parameters)!.ToString();
                    sqlBuilder.SQL.AppendLine($" {prop.Name}= '{value}', ");
                }
            }
            return sqlBuilder;
        }

        /// <summary>
        /// 删除语句
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="table"></param>
        public static SqlBuilder Delete(this SqlBuilder sqlBuilder, string tableName)
        {
            sqlBuilder.SQL.AppendLine($"DELETE FROM {tableName} ");
            return sqlBuilder;
        }

        /// <summary>
        /// 注入参数
        /// </summary>
        /// <param name="sqlBuilder"></param>
        /// <param name="parameters"></param>
        public static SqlBuilder SetParameter<T>(this SqlBuilder sqlBuilder, T parameters)
        {
            foreach (PropertyInfo prop in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.GetValue(parameters) != null)
                {
                    string? value = prop.GetValue(parameters)!.ToString();
                    sqlBuilder.SQL.Replace($"?[{prop.Name}]",
                    $"{prop.Name}='{(value==null?"": value)}'");
                }
            }
            return sqlBuilder;
        }
    }
}