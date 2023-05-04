using System.Data.SqlClient;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Extensions;
using TSI.QueryBuilder.Models;

//var connection = new SqlConnection("Server=DESKTOP-C5H9A88\\SQLEXPRESS;Database=Northwind;UID=sa;PWD=Logo1234567890;MultipleActiveResultSets=True;");
//if (connection.State == System.Data.ConnectionState.Closed)
//    connection.Open();

var factory = new QueryFactory();
var connected = factory.ConnectToDatabase();
factory.CommandTimeOut = 600;

//var query = factory.Query().From("Employees").Insert(new CreateEmployeesDto
//{
//    Address = "adres",
//    City = "İstanbul",
//    FirstName = "Hüseyin",
//    LastName = "Özsüzer",
//    Title = "Müdür",
//    TitleOfCourtesy = "Mr.",
//    BirthDate = null,
//    HireDate = null,
//    Photo = null
//});

//var employee = factory.Insert<SelectEmployeesDto>(query, "EmployeeID", true);

//var query = db.Query().From("Employees").Select().Where("FirstName", "Janet");
//var query2 = db.Query().From("Employees").Select("EmployeeID","FirstName","LastName").Where("FirstName", "Janet");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where("FirstName", "aaaa");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where("Age", ">","45");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").Where<Employees>(t => t.FirstName == "Janet");
//var query2 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").WhereIn("City", "Seattle", "London");
//var query3 = db.Query().From("Employees").Select("EmployeeID", "FirstName", "LastName").WhereNotIn("City", "Seattle", "London");
//var query4 = db.Query().From("Employees").WhereContains("Title", "Representative");
//var query5 = db.Query().From("Employees").WhereStartingWith("LastName", "Pe");
//var query6 = db.Query().From("Employees").WhereEndingWith("LastName", "n");
//var query7 = db.Insert().Into("Employees", "Title", "TitleOfCourtesy").Values("Electronics and Communication Engineer", "Mr.");
//var query3 = db.Query().From("Employees").OrderBy("Extension");
//var query4 = db.Query().From("Employees").OrderByDescending("Extension");
//var query5 = db.Query().From("Employees").Select("EmployeeID","FirstName","LastName").Take(3).OrderByDescending("EmployeeID");
//var query6 = db.Query().From("Employees").Distinct("City");
//var query7 = db.Query().ExecuteSql("select * from Employees where Title={0} and FirstName={1}", "'Sales Representative'", "'Janet'");

//var query8 = db.Query().From("Employees").Insert(new[] { "FirstName", "LastName" }, new[] { "Hüseyin", "Özsüzer" });
//var query8 = db.Query().From("Employees").Insert(new CreateEmployeesDto
//{
//    Address = "adres",
//    City = "İstanbul",
//    FirstName = "Hüseyin",
//    LastName = "Özsüzer",
//    Title = "Müdür",
//    TitleOfCourtesy = "Mr.",
//    BirthDate = null,
//    HireDate = null,
//    Photo = null
//});

//int id = db.Insert(query8, "EmployeeID").GetValueOrDefault();

//db.Create<Employees>(query7);

//var query = db.Query().From("Employees").Select().Where<SelectEmployeesDto>(t => t.LastName == "Fuller" && t.FirstName== "Andrew");
//var a = db.Get<SelectEmployeesDto>(query);

//Console.WriteLine(query7.Sql);
//var employess = db.GetList<Employees>(query5);

//var employess2 = db.GetArray<Employees>(query7);

//foreach (var item in employess)
//{
//    Console.WriteLine("ID: " + item.EmployeeID + " " + "First Name: " + item.FirstName + " " + "Last Name: " + item.LastName + " " + "Title: " + item.Title + " " + "City: " + item.City);
//}

//var query8 = db.Query().From("Employees").Insert(new CreateEmployeesDto
//{
//    Address = "adres",
//    City = "İstanbul",
//    FirstName = "Hüseyin2",
//    LastName = "Özsüzer",
//    Title = "Müdür",
//    TitleOfCourtesy = "Mr.",
//    BirthDate = null,
//    HireDate = null,
//    Photo = null
//});

//var query8 = db.Query().From("Employees").Update(new CreateEmployeesDto
//{
//    Address = "Adres",
//    City = "İstanbul",
//    FirstName = "Hüseyin",
//    LastName = "Özsüzer",
//    Title = "Müdür",
//    TitleOfCourtesy = "Mr.",
//    BirthDate = null,
//    HireDate = null,
//    Photo = null
//}).Where("FirstName", "Nancy");

//var a = db.Insert<SelectEmployeesDto>(query8, "EmployeeID");
//var a = db.Update<SelectEmployeesDto>(query8, "EmployeeID");

//var query8 = db.Query().From("Employees").Delete(Guid.NewGuid()).Where("EmployeeID", "17");

//var ab = db.Delete(query8);

Console.ReadLine();