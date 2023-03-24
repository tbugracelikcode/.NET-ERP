using System.Data.SqlClient;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Models;

var connection = new SqlConnection("Server=DESKTOP-C5H9A88\\SQLEXPRESS;Database=Northwind;UID=sa;PWD=Logo1234567890;MultipleActiveResultSets=True;");
if (connection.State == System.Data.ConnectionState.Closed)
    connection.Open();

var db = new QueryFactory(connection);

//var query = db.Query().From("Employees");
//var query2 = db.Query().From("Employees");
//var query3 = db.Query().From("Employees").OrderBy("Extension");
//var query4 = db.Query().From("Employees").OrderByDescending("Extension");
//var query5 = db.Query().From("Employees").Select("EmployeeID","FirstName","LastName").Take(3).OrderByDescending("EmployeeID");
var query6 = db.Query().From("Employees").Distinct("City");
var employess = db.GetList<Employees>(query6);

foreach (var item in employess)
{
    Console.WriteLine("ID: "+item.EmployeeID +" "+ "First Name: " + item.FirstName + " " + "Last Name: " + item.LastName + " " + "Title: " + item.Title + " " + "City: " + item.City);
}

Console.WriteLine(query6.Sql);

Console.ReadLine();