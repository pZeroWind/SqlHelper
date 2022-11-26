// See https://aka.ms/new-console-template for more information
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlHelper;
using SqlHelper.Builders;
using SqlHelper.Enums;
using SqlHelper.Structs;
using System.Data;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Test;
using static System.Runtime.InteropServices.JavaScript.JSType;


SqlHelperContext.Run("Mapping");

//var daten = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
//var dbn = SqlHelper.SqlHelper.Connection();
//var usern = await new UsersMapping(dbn).TestSelect(new UserParams
//{
//    id = "1"
//});
////Console.WriteLine(JsonConvert.SerializeObject(user));
//Console.WriteLine($"查询耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - daten) + "ms");
int a = 10;
for (int i = 0; i < a; i++)
{
    int f = i;
    //_ = Task.Run(async () =>
    //{

    //});
    var db = new DBContext("server=127.0.0.1;database=test;port=3306;user=root;password=Zw123456");
    var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    var user = await db.Order!.TestSelect(new OrderParams
    {
        id = "1"
    });
    
    //Console.WriteLine(JsonConvert.SerializeObject(user));
    //db.Order.Query("sele");
    Console.WriteLine($"封装后第{f+1}次查询耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - date) + "ms");
    Console.WriteLine("---");
}

for (int i = 0; i < a; i++)
{
    int f = i;
    //_ = Task.Run(async () =>
    //{

    //});
    var date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    var db = new DBContext("server=127.0.0.1;database=test;port=3306;user=root;password=Zw123456").Connection;
    var user = await db.QueryAsync<Order>("select * from d_meter_assembly_info where id = 3 limit 1,10000");
    //Console.WriteLine(JsonConvert.SerializeObject(user));
    Console.WriteLine($"未封装第{f+1}次查询耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - date) + "ms");
    Console.WriteLine("---");
}
/*
Console.WriteLine("功能测试:\r\n");
var date1 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
BaseSqlBuilder builder = MySqlBuilder
    .InstanceSelect("USERS")
    .JoinOn("CLASS", "CID = ID", JoinType.INNER)
    .Where(new
    {
        ID = "='Test'"
    })
    .And(new
    {
        Age = "<12"
    })
    .Or(new
    {
        Birthday = ">'2000-1-1'"
    })
    .GroupBy("ID")
    .OrderBy(OrderBy.Instance("ID", OrderType.ASC));
string sql = MSSqlBuilder
    .InstanceSelect("USERS")
    .JoinOn("CLASS", "CID = ID", JoinType.INNER)
    .Where(new
    {
        ID = "=Test"
    })
    .And(new
    {
        Age = "=12"
    })
    .Or(new
    {
        Birthday = "<'2000-1-1'"
    })
    .BetweenAnd("ID","1","10")
    .GroupBy("ID")
    .OrderBy(OrderBy.Instance("ID",OrderType.ASC))
    .Offset(1)
    .Fetch(2).UnionALL(builder).Build();
Console.WriteLine("SELECT:");
Console.WriteLine(sql);
Console.WriteLine("构建耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - date1) + "ms");
Console.WriteLine("---");
var date4 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
sql = MySqlBuilder
    .InstanceUpdate("USER",new
    {
        ID = "Test2",
        Age = "18",
        Birthday = "1999-1-1"
    })
    .Where(new
    {
        ID = "='Test'"
    })
    .And(new
    {
        Age = "<=12"
    })
    .Or(new
    {
        Birthday = ">=2000-1-1"
    }).Build();
Console.WriteLine("UPDATE:");
Console.WriteLine(sql);
Console.WriteLine("构建耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - date4) + "ms");
Console.WriteLine("---");
var date5 = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
sql = MySqlBuilder
    .InstanceDelete("USER")
    .Where(new
    {
        ID = "=Test"
    })
    .And(new
    {
        Age = "<=12"
    })
    .Or(new
    {
        Birthday = "<=2000-1-1"
    }).Build();
Console.WriteLine("DELETE:");
Console.WriteLine(sql);
Console.WriteLine("构建耗时：" + (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - date5) + "ms");
Console.WriteLine();
Console.WriteLine("---");
*/
Console.ReadKey();




