// See https://aka.ms/new-console-template for more information
using SqlHelper;
using SqlHelper.Enums;
using SqlHelper.Structs;

Console.WriteLine("功能测试:\r\n");
SqlBuilder sqlBuilder = SqlBuilder
    .InstanceSelect("USERS")
    .JoinOn("CLASS", "CID = ID", JoinType.INNER)
    .Where(new
    {
        ID = "Test"
    })
    .And(new
    {
        Age = 12
    })
    .Or(new
    {
        Birthday = "2000-1-1"
    })
    .GroupBy("ID")
    .OrderBy(OrderBy.Instance("ID", OrderType.ASC));
string sql = SqlBuilder
    .InstanceSelect("USERS")
    .JoinOn("CLASS", "CID = ID", JoinType.INNER)
    .Where(new
    {
        ID = "Test"
    })
    .And(new
    {
        Age = 12
    })
    .Or(new
    {
        Birthday = "2000-1-1"
    })
    .GroupBy("ID")
    .OrderBy(OrderBy.Instance("ID",OrderType.ASC))
    .Offset(1)
    .Fetch(2).UnionALL(sqlBuilder).Build();
Console.WriteLine("SELECT:");
Console.WriteLine(sql);
Console.WriteLine("---");
sql = SqlBuilder
    .InstanceUpdate("USER",new
    {
        ID = "Test2",
        Age = "18",
        Birthday = "1999-1-1"
    })
    .Where(new
    {
        ID = "Test"
    })
    .And(new
    {
        Age = 12
    })
    .Or(new
    {
        Birthday = "2000-1-1"
    }).Build();
Console.WriteLine("UPDATE:");
Console.WriteLine(sql);
Console.WriteLine("---");
sql = SqlBuilder
    .InstanceDelete("USER")
    .Where(new
    {
        ID = "Test"
    })
    .And(new
    {
        Age = 12
    })
    .Or(new
    {
        Birthday = "2000-1-1"
    }).Build();
Console.WriteLine("DELETE:");
Console.WriteLine(sql);
