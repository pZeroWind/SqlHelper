using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;
using SqlHelper.Enums;
using Oracle.ManagedDataAccess.Client;
using System.Data.OleDb;
using Microsoft.Data.Sqlite;

namespace SqlHelper
{
    public class DbDapper
    {
        private readonly DbConnection _connection;
        public DbDapper(string conn, DBTypes types) 
        {
            _connection = types switch
            {
                DBTypes.MySql => new MySqlConnection(conn),
                DBTypes.SqlServer => new SqlConnection(conn),
                DBTypes.Oracle => new OracleConnection(conn),
                DBTypes.Access => new OleDbConnection(conn),
                DBTypes.SqlLite => new SqliteConnection(conn),
                _ => new MySqlConnection(conn),
            };
        }

        public DbConnection Connection { get { return _connection; } }
    }
}
