using System.Data.SqlClient;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Extensions;
using TSI.QueryBuilder.Models;

var connection = new SqlConnection("Server=DESKTOP-C5H9A88\\SQLEXPRESS;Database=Northwind;UID=sa;PWD=Logo1234567890;MultipleActiveResultSets=True;");
if (connection.State == System.Data.ConnectionState.Closed)
    connection.Open();

var db = new QueryFactory(connection);
db.CommandTimeOut = 600;


//var query = db.Query().From("Employees");
//var query2 = db.Query().From("Employees").Select("EmployeeID","FirstName","LastName").Where("FirstName", "Janet");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where("FirstName", "aaaa");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where("Age", ">","45");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where<Employees>(t => t.FirstName == "Janet");
var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").WhereIn("City", "Seattle", "London");
var query3 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").WhereNotIn("City", "Seattle", "London");
//var query3 = db.Query().From("Employees").OrderBy("Extension");
//var query4 = db.Query().From("Employees").OrderByDescending("Extension");
//var query5 = db.Query().From("Employees").Select("EmployeeID","FirstName","LastName").Take(3).OrderByDescending("EmployeeID");
//var query6 = db.Query().From("Employees").Distinct("City");
//var query7 = db.Query().ExecuteSql("select * from Employees where Title={0} and FirstName={1}", "'Sales Representative'", "'Janet'");



var employess = db.GetList<Employees>(query2);

//var employess2 = db.GetArray<Employees>(query7);

foreach (var item in employess)
{
    Console.WriteLine("ID: " + item.EmployeeID + " " + "First Name: " + item.FirstName + " " + "Last Name: " + item.LastName + " " + "Title: " + item.Title + " " + "City: " + item.City);
}

//Console.WriteLine(query7.Sql);


Console.ReadLine();