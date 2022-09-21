using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Gantt;
using Syncfusion.Blazor.Grids;
using Tsi.Core.Utilities.Results;
using TsiErp.Entities.Entities.EquipmentRecord.Dtos;

namespace TsiErp.ErpUI.Pages.EquipmentRecord
{
    public partial class EquipmentRecordsListPage
    {
        bool cancelReasonVisible = false;
        protected override async void OnInitialized()
        {
            BaseCrudService = EquipmentRecordsService;
        }

        void CheckValueChanged(ChangeEventArgs args)
        {
            bool argsValue = Convert.ToBoolean(args.Value);

            if (argsValue)
            {
                DataSource.CancellationDate = DateTime.Today;
                cancelReasonVisible = true;
            }
            else
            {
                DataSource.CancellationDate = null;
                cancelReasonVisible = false;
            }
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectEquipmentRecordsDto()
            {
                IsActive = true
            };

            ShowEditPage();

            return Task.CompletedTask;
        }
    }
}
