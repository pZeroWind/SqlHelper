using Dapper;
using Microsoft.ClearScript.V8;
using SqlHelper.Mapping.Attributes;
using SqlHelper.Structs;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SqlHelper.Mapping
{
    public abstract class Mapper<T>
    {
        //当前对象的JsonMap
        private readonly JsonNode? json = null;
        private readonly DbConnection _db;

        public Mapper(SqlHelperContext sql)
        {
            var mapper = GetType().GetCustomAttribute<MapperAttribute>();
            if (SqlHelperContext.Mapping.ContainsKey(mapper!.MapperName))
            {
                json = SqlHelperContext.Mapping[mapper!.MapperName];
            }
            _db =  sql.Connection;
        }

        #region json映射sql
        /// <summary>
        /// 读取json来映射sql语句
        /// </summary>
        /// <returns></returns>
        private string MappingSQL<S>(S parameter, string funcName)
        {
            //Sql字符串构建器
            StringBuilder Sql = new StringBuilder();
            //获取所有参数列表
            var props = typeof(S).GetProperties(BindingFlags.Instance | BindingFlags.Public);
            //解析JSON
            if (json != null)
            {
                var jsonObj = json[funcName];
                if (jsonObj != null)
                {
                    var obj = jsonObj.AsObject();
                    foreach (var node in obj)
                    {
                        //捕获main字段
                        if (node.Key.ToLower() == "main")
                        {
                            Sql.AppendLine(node.Value!.ToString());
                        }
                        else
                        {
                            ForEach f = new ForEach();
                            int ifNum = 0;
                            foreach (var node2 in node.Value!.AsObject())
                            {
                                switch (node2.Key.ToLower())
                                {
                                    case "if":
                                        ifNum++;
                                        //将字符转化为js代码执行并获取到判断结果
                                        if(CheckIF(node2,props, parameter)) ifNum = 0;
                                        continue;
                                    case "elseif":
                                        //将字符转化为js代码执行并获取到判断结果
                                        if (ifNum > 0)
                                        {
                                            ifNum++;
                                            if (CheckIF(node2, props, parameter)) ifNum = 0;
                                        }
                                        continue;
                                    case "else":
                                        if (ifNum > 0)
                                        {
                                            Sql = SetSQL(Sql, node2, props, parameter);
                                            ifNum = 0;
                                        }
                                        continue;
                                    case "for":
                                        SetFor(ref f, node2, props, parameter);
                                        continue;
                                }
                                //拼接SQL
                                if (f.isFor && node2.Key.ToLower() == "sql" && ifNum == 0)
                                {
                                    dynamic list = props.FirstOrDefault(p => p.Name.ToLower() == f.name!.ToLower())!.GetValue(parameter)!;
                                    for (int i = 0; i < f.length; i++)
                                    {
                                        Sql = SetForSQL(Sql, i, f, list, node2, props, parameter);
                                    }
                                    goto finish;
                                }
                                else if (node2.Key.ToLower().Contains("sql") && ifNum == 0)
                                {
                                    Sql = SetSQL(Sql, node2, props, parameter);
                                    goto finish;
                                }
                            }
                        }
                    finish: continue;
                    }
                }
            }
            return Sql.ToString() ;
        }

        /// <summary>
        /// 判断IF-ELES语句
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="node"></param>
        /// <param name="props"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private bool CheckIF<S>(KeyValuePair<string, JsonNode?> node, PropertyInfo[] props, S? parameter)
        {
            string exp = node.Value!.ToString().ToLower();
            bool pass = false;
            foreach (var item in props)
            {
                if (exp.Contains(item.Name))
                {
                    var s = item.GetValue(parameter)!.ToString();
                    exp = exp.Replace($"{item.Name}", s != "" ? s : "null");
                }
            }
            using (var engine = new V8ScriptEngine())
            {
                engine.Execute($"function e(){{ return {exp} }}");
                pass = (bool)engine.Script.e();
            }
            return pass;
        }

        private StringBuilder SetSQL<S>(StringBuilder Sql,KeyValuePair<string, JsonNode?> node, PropertyInfo[] props, S? parameter)
        {
            string exp = node.Value!.ToString();
            foreach (var item in props)
            {
                if (exp.Contains(item.Name.ToLower()))
                {
                    exp = exp.Replace($"?:{item.Name}".ToLower(), item.GetValue(parameter)!.ToString());
                }
            }
            Sql.AppendLine(exp);
            return Sql;
        }

        private StringBuilder SetForSQL<S>(StringBuilder Sql, int i, ForEach f, dynamic list, KeyValuePair<string, JsonNode?> node, PropertyInfo[] props, S? parameter)
        {
            string exp = node.Value!.ToString();
            exp = exp.Replace($"?:{f.name}", list[i]!.ToString());
            foreach (var item in props)
            {
                if (exp.Contains(item.Name.ToLower()))
                {
                    exp = exp.Replace($"?:{item.Name}", JsonSerializer.Serialize(item.GetValue(parameter)));
                }
            }
            Sql.AppendLine(exp);
            return Sql;
        }

        private void SetFor<S>(ref ForEach f, KeyValuePair<string, JsonNode?> node, PropertyInfo[] props, S? parameter)
        {
            string exp4 = node.Value!.ToString().ToLower();
            foreach (var item in props)
            {
                if (exp4 == item.Name.ToLower())
                {
                    f.isFor = true;
                    f.name = item.Name.ToLower();
                    dynamic data = item.GetValue(parameter)!;
                    if (int.TryParse(data.ToString(), out int len))
                        f.length = len;
                    else
                        f.length = data.Count;
                }
            }
        }
        #endregion

        #region json映射模式
        /// <summary>
        /// 查询-json映射
        /// </summary>
        /// <returns></returns>
        protected Task<IEnumerable<T>> Query<S>(S parameter, string funcName)
        {
            return _db.QueryAsync<T>(MappingSQL(parameter,funcName));
        }

        /// <summary>
        /// 查询第一个实例-json映射
        /// </summary>
        /// <returns></returns>
        protected Task<T> QueryFirst<S>(S parameter, string funcName)
        {
            return _db.QueryFirstOrDefaultAsync<T>(MappingSQL(parameter, funcName));
        }

        /// <summary>
        /// 查询单个实例-json映射
        /// </summary>
        /// <returns></returns>
        protected Task<T> QuerySingle<S>(S parameter, string funcName)
        {
            return _db.QuerySingleOrDefaultAsync<T>(MappingSQL(parameter, funcName));
        }

        /// <summary>
        /// 执行-json映射
        /// </summary>
        /// <returns></returns>
        protected Task<int> Execute<S>(S parameter, string funcName)
        {
            return _db.ExecuteAsync(MappingSQL(parameter, funcName));
        }
        #endregion

        #region 普通模式
        /// <summary>
        /// 查询第一个实例
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T>> Query(string sql)
        {
            return _db.QueryAsync<T>(sql);
        }

        /// <summary>
        /// 查询单个实例
        /// </summary>
        /// <returns></returns>
        public Task<T> QueryFirst(string sql)
        {
            return _db.QueryFirstOrDefaultAsync<T>(sql);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Task<T> QuerySingle(string sql)
        {
            return _db.QuerySingleOrDefaultAsync<T>(sql);
        }

        public Task<int> Execute<S>(string sql)
        {
            return _db.ExecuteAsync(sql);
        }
        #endregion
    }
}
