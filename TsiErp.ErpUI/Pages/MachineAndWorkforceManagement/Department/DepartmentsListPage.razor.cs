using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department.Dtos;

namespace TsiErp.ErpUI.Pages.MachineAndWorkforceManagement.Department
{
    public partial class DepartmentsListPage
    {
        protected override async void OnInitialized()
        {
            BaseCrudService = DepartmentsService;
            _L = L;
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectDepartmentsDto()
            {
                IsActive = true,
                Code = FicheNumbersAppService.GetFicheNumberAsync("DepartmentsChildMenu")
            };

            EditPageVisible = true;
            return Task.CompletedTask;
        }


        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.Code = FicheNumbersAppService.GetFicheNumberAsync("DepartmentsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion
    }
}
