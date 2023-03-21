using System.Data.SqlClient;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;

var connection = new SqlConnection("Server=DESKTOP-ULA47PR\\SQLEXPRESS;Database=Northwind;UID=sa;PWD=Logo1234567890;MultipleActiveResultSets=True;");
if (connection.State == System.Data.ConnectionState.Closed)
    connection.Open();

var db = new QueryFactory(connection);

var query = db.Query().From("Employees").Select(new string[] { "FirstName", "LastName" });
var query2 = db.Query().From("Employees");
var employess = db.GetList<Employees>(query2);


Console.WriteLine(query.Sql);
Console.WriteLine(query2.Sql);

Console.ReadLine();