using System.Data.SqlClient;
using TSI.QueryBuilder;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;

var connection = new SqlConnection("Server=DESKTOP-ULA47PR\\SQLEXPRESS;Database=Northwind;UID=sa;PWD=Logo1234567890;MultipleActiveResultSets=True;");
if (connection.State == System.Data.ConnectionState.Closed)
    connection.Open();

var db = new QueryFactory(connection);

//var query = db.Query().From("Employees").Select(new string[] { "FirstName", "LastName","Title" });
var query2 = db.Query().From("Employees").Where("FirstName","Janet");
var employess = db.GetList<Employees>(query2);
Console.WriteLine(query2.Sql);

//foreach (var item in employess)
//{
//    Console.WriteLine("First Name: " + item.FirstName + " " + "Last Name: " + item.LastName + " " + "Title: " + item.Title);
//}



Console.ReadLine();