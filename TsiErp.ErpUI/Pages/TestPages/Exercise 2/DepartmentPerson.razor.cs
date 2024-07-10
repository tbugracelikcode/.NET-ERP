using static TsiErp.ErpUI.Pages.TestPages.Exercise1.PersonsandSchoolsListPage;
using Syncfusion.Blazor.Grids;
using DevExpress.CodeParser;
using Microsoft.AspNetCore.Components;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.TestPages.Exercise_2

{

    public partial class DepartmentPerson
    {

        [Inject]
        ModalManager ModalManager { get; set; }
        public class Employee
        {
            public int Id { get; set; }
            public int DepartmentId { get; set; }
            public string NameSurname { get; set; }
            public string University { get; set; }
            public int Age { get; set; }
        }

        public class Department
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public int Capasity { get; set; }
            public string Description { get; set; }
        }
        public List<Employee> EmployeesList = new List<Employee>();
        public List<Employee> EmployeeGridList = new List<Employee>();
        public List<Department> DepartmentList = new List<Department>();

        Employee EmployeeDataSource;
        Department DepartmentDataSource;

        private SfGrid<Employee> EmployeeGrid;
        private SfGrid<Department> DepartmentGrid;

        public bool employeeEditModalVisible = false;
        public bool departmentEditModalVisible = false;
        
        public List<ContextMenuItemModel>employeeContextMenu = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> departmentContextMenu = new List<ContextMenuItemModel>();

        protected override async void OnInitialized()
        {
            CreateEmployeeMenuItems();
            CreateDepartmentMenuItems();

        }

        protected void CreateEmployeeMenuItems()
        {
            if (employeeContextMenu.Count() == 0)
            {

                employeeContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "add" });
                employeeContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "change" });
                employeeContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                employeeContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }
        protected void CreateDepartmentMenuItems()
        {
            if (departmentContextMenu.Count() == 0)
            {

                departmentContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "add" });
                departmentContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "change" });
                departmentContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                departmentContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void DepartmentContextMenuClick(ContextMenuClickEventArgs<Department> args)
        {
            switch (args.Item.Id)
            {
                case "add":

                    DepartmentDataSource = new Department()
                    {
                        Id = DepartmentList.Count + 1,
                    };

                    EmployeeGridList = new List<Employee>();

                    departmentEditModalVisible = true;

                    break;

                case "change":

                    DepartmentDataSource = args.RowInfo.RowData;
                    EmployeeGridList = EmployeesList.Where(t => t.DepartmentId == DepartmentDataSource.Id).ToList();

                    departmentEditModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;


                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz?");
                    if (res == true)
                    {
                        DepartmentList.Remove(args.RowInfo.RowData);
                        await DepartmentGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await DepartmentGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }
        public async void EmployeeContextMenuClick(ContextMenuClickEventArgs<Employee> args)
        {
            switch (args.Item.Id)
            {
                case "add":

                    EmployeeDataSource = new Employee()
                    {
                        Id = EmployeesList.Count + 1,
                        DepartmentId = DepartmentDataSource.Id,
                    };

                    employeeEditModalVisible = true;

                    break;

                case "change":

                    EmployeeDataSource = args.RowInfo.RowData;

                    employeeEditModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;



                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz?");
                    if (res == true)
                    {
                        EmployeesList.Remove(args.RowInfo.RowData);
                        EmployeeGridList.Remove(args.RowInfo.RowData);
                        await EmployeeGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await EmployeeGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }
        public async void OnSubmit()
        {
            if (DepartmentList.Count + 1 == DepartmentDataSource.Id) // Yeni kayıt
            {
                DepartmentList.Add(DepartmentDataSource);

                foreach (var line in EmployeeGridList)
                {
               

                    if (EmployeesList.Contains(line))//update
                    {
                        var updatedEmployee = EmployeesList.Where(t => t.Id == line.Id).FirstOrDefault();
                        int indexEmployee = EmployeesList.IndexOf(updatedEmployee);

                        EmployeesList[indexEmployee] = line;

                    }
                    else//yeni
                    {
                        EmployeesList.Add(line);
                    }
                }



                departmentEditModalVisible = false;

            }
            else
            {
                var updatedDepartment = DepartmentList.Where(t => t.Id == DepartmentDataSource.Id).FirstOrDefault();

                int updatedIndex = DepartmentList.IndexOf(updatedDepartment);

                DepartmentList[updatedIndex] = DepartmentDataSource;

                departmentEditModalVisible = false;

            }

            await DepartmentGrid.Refresh();
            await InvokeAsync(StateHasChanged);

        }

        public async void OnLineSubmit()
        {
            if (EmployeeGridList.Count + 1 == EmployeeDataSource.Id) // Yeni kayıt
            {
                EmployeeGridList.Add(EmployeeDataSource);

                employeeEditModalVisible = false;

            }
            else
            {

                var updatedEmployeeGrid = EmployeeGridList.Where(t => t.Id == EmployeeDataSource.Id).FirstOrDefault();

                int updatedIndexGrid = EmployeeGridList.IndexOf(updatedEmployeeGrid);

                EmployeeGridList[updatedIndexGrid] = EmployeeDataSource;

                employeeEditModalVisible = false;

            }

            await EmployeeGrid.Refresh();
            await InvokeAsync(StateHasChanged);

        }

        public void HideDepartmentEditModal()
        {
            departmentEditModalVisible = false;
        }

        public void HideEmployeeEditModal()
        {
            employeeEditModalVisible = false;
        }

    }
}