using SqlHelper.Enums;
using SqlHelper.Mapping.Attributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SqlHelper
{
    public abstract class SqlHelperContext
    {
        private readonly DbConnection _connection;

        public SqlHelperContext(string conn, DBTypes? types = DBTypes.MySql)
        {
            _connection = new DbDapper(conn, types!.Value).Connection;
            //反射字段并实例化字段
            GetType().GetFields().ToList().ForEach(p =>
            {
                if (p.FieldType.GetCustomAttribute<MapperAttribute>() != null)
                {
                    p.SetValue(this, Activator.CreateInstance(p.FieldType, new object[] { this }));
                }
            });
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public DbConnection Connection { get => _connection; }

        public static void Run(string path)
        {
            DirectoryInfo root = new DirectoryInfo(path);
            FileInfo[] files = root.GetFiles().Where(p=>p.Extension == ".json").ToArray();
            foreach (var file in files)
            {
                string key = file.Name.Replace($"{path}\\", "").Replace(file.Extension, "");
                if (!Mapping.ContainsKey(key))
                {
                    JsonNode json = JsonNode.Parse(File.ReadAllText(path+"/"+file.Name))!;
                    Mapping.Add(key, json);
                }
            }
            
        }

        //已经映射的Json列表
        public static Dictionary<string, JsonNode> Mapping = new Dictionary<string, JsonNode>();
    }
}
