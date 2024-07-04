using Syncfusion.Blazor.Grids;
using TsiErp.Business.Entities.GeneralSystemIdentifications.UserPermission.Services;
using TsiErp.Business.Entities.Menu.Services;
using TsiErp.Business.Entities.PalletRecord.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using static TsiErp.ErpUI.Pages.ShippingManagement.PalletRecord.PalletRecordsListPage;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecordLine.Dtos;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Entities.Entities.ShippingManagement.PalletRecord.Dtos;
using Microsoft.AspNetCore.Components;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.TestPages.Exercise1
{
    public partial class PersonsandSchoolsListPage
    {

        private SfGrid<Person> _PersonGrid;
        private SfGrid<School> _SchoolGrid;

        [Inject]
        ModalManager ModalManager { get; set; }

        Person PersonDataSource;
        School SchoolDataSource;

        public List<ContextMenuItemModel> PersonGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public List<ContextMenuItemModel> SchoolGridContextMenu { get; set; } = new List<ContextMenuItemModel>();
        public class Person
        {
            public int Id { get; set; }
            public string NameSurname { get; set; }

            public DateTime BirthDay { get; set; }

            public int Age { get; set; }

            public string Description_ { get; set; }

            public List<School> SchoolLineList { get; set; }
        }

        public List<Person> PersonsList = new List<Person>();

        public class School
        {
            public int Id { get; set; }
            public int PersonId { get; set; }
            public string SchoolName { get; set; }
            public DateTime Date { get; set; }
            public int TotalStudents { get; set; }
            public string Type { get; set; }
        }

        public List<School> SchoolsList = new List<School>();
        public List<School> SchoolGridList = new List<School>();

        public bool personEditModalVisible = false;
        public bool schoolEditModalVisible = false;

        protected override async void OnInitialized()
        {
            CreatePersonContextMenuItems();
            CreateSchoolContextMenuItems();
        }

        protected void CreatePersonContextMenuItems()
        {
            if (PersonGridContextMenu.Count() == 0)
            {

                PersonGridContextMenu.Add(new ContextMenuItemModel { Text =    "Ekle", Id = "add" });
                PersonGridContextMenu.Add(new ContextMenuItemModel { Text =    "Değiştir", Id = "change" });
                PersonGridContextMenu.Add(new ContextMenuItemModel { Text =    "Sil", Id = "delete" });
                PersonGridContextMenu.Add(new ContextMenuItemModel { Text =    "Güncelle", Id = "refresh" });
            }
        }

        protected void CreateSchoolContextMenuItems()
        {
            if (SchoolGridContextMenu.Count() == 0)
            {

                SchoolGridContextMenu.Add(new ContextMenuItemModel { Text = "Ekle", Id = "add" });
                SchoolGridContextMenu.Add(new ContextMenuItemModel { Text = "Değiştir", Id = "change" });
                SchoolGridContextMenu.Add(new ContextMenuItemModel { Text = "Sil", Id = "delete" });
                SchoolGridContextMenu.Add(new ContextMenuItemModel { Text = "Güncelle", Id = "refresh" });
            }
        }

        public async void PersonContextMenuClick(ContextMenuClickEventArgs<Person> args)
        {
            switch (args.Item.Id)
            {
                case "add":

                    PersonDataSource = new Person()
                    {
                       Id = PersonsList.Count +1,
                    };

                    SchoolGridList = new List<School>();

                    personEditModalVisible = true;

                    break;

                case "change":

                    PersonDataSource = args.RowInfo.RowData;
                    SchoolGridList = SchoolsList.Where(t=>t.PersonId == PersonDataSource.Id).ToList();

                    personEditModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;


                case "delete":
                    var res = await ModalManager.ConfirmationAsync(   "Onay",   "Silmek istediğinize emin misiniz?");
                    if (res == true)
                    {
                        PersonsList.Remove(args.RowInfo.RowData);
                        await _PersonGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await _PersonGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void SchoolContextMenuClick(ContextMenuClickEventArgs<School> args)
        {
            switch (args.Item.Id)
            {
                case "add":

                    SchoolDataSource = new School()
                    {
                        Id = SchoolsList.Count + 1,
                        PersonId = PersonDataSource.Id,
                    };

                    schoolEditModalVisible = true;

                    break;

                case "change":

                    SchoolDataSource = args.RowInfo.RowData;

                    schoolEditModalVisible = true;
                    await InvokeAsync(StateHasChanged);
                    break;
                    

                case "delete":
                    var res = await ModalManager.ConfirmationAsync("Onay", "Silmek istediğinize emin misiniz?");
                    if (res == true)
                    {
                        SchoolsList.Remove(args.RowInfo.RowData);
                        SchoolGridList.Remove(args.RowInfo.RowData);
                        await _SchoolGrid.Refresh();
                        await InvokeAsync(StateHasChanged);
                    }
                    break;

                case "refresh":
                    await _SchoolGrid.Refresh();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }

        public async void OnSubmit()
        {
            if(PersonsList.Count + 1 == PersonDataSource.Id) // Yeni kayıt
            {
                PersonsList.Add(PersonDataSource);

                foreach(var line in SchoolGridList)
                {
                    if (SchoolsList.Contains(line))//update
                    {
                        var updatedSchool = SchoolsList.Where(t => t.Id == line.Id).FirstOrDefault();
                        int indexSchool = SchoolsList.IndexOf(updatedSchool);

                        SchoolsList[indexSchool] = line;

                    }
                    else//yeni
                    {
                        SchoolsList.Add(line);
                    }
                }


                
                personEditModalVisible = false;

            }
            else
            {
              var updatedPerson = PersonsList.Where(t=>t.Id == PersonDataSource.Id).FirstOrDefault();

                int updatedIndex = PersonsList.IndexOf(updatedPerson);

                PersonsList[updatedIndex] = PersonDataSource;

                personEditModalVisible = false;

            }

            await _PersonGrid.Refresh();
            await InvokeAsync(StateHasChanged);

        }

        public async void OnLineSubmit()
        {
            if (SchoolGridList.Count + 1 == SchoolDataSource.Id) // Yeni kayıt
            {
                SchoolGridList.Add(SchoolDataSource);

                schoolEditModalVisible = false;

            }
            else
            {

                var updatedSchoolGrid = SchoolGridList.Where(t => t.Id == SchoolDataSource.Id).FirstOrDefault();

                int updatedIndexGrid = SchoolGridList.IndexOf(updatedSchoolGrid);

                SchoolGridList[updatedIndexGrid] = SchoolDataSource;

                schoolEditModalVisible = false;

            }

            await _SchoolGrid.Refresh();
            await InvokeAsync(StateHasChanged);

        }

        public void HidePersonEditModal()
        {
            personEditModalVisible = false;
        }

        public void HideSchoolEditModal()
        {
            schoolEditModalVisible = false;
        }
    }
}
