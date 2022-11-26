using Microsoft.ClearScript.V8;
using SqlHelper.Enums;
using SqlHelper.Structs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SqlHelper.Builders
{
    public abstract class BaseSqlBuilder
    {
        internal StringBuilder SQL;
        protected BaseSqlBuilder()
        {
            SQL = new StringBuilder();

        }

        /// <summary>
        /// 构建SQL语句
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            return SQL.ToString();
        }



    }
}
