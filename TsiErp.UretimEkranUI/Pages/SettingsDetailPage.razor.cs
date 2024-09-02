using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Business.Entities.Employee.Services;
using TsiErp.Business.Entities.OperationUnsuitabilityReport.Services;
using TsiErp.Business.Entities.ProductionTracking.Services;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Station.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class SettingsDetailPage
    {

        SystemGeneralStatusTable GeneralStatusDataSource = new SystemGeneralStatusTable();


        protected override async void OnInitialized()
        {
            GeneralStatusDataSource = (await SystemGeneralStatusLocalDbService.GetListAsync()).ToList().FirstOrDefault();

            await InvokeAsync(StateHasChanged);
        }

        #region İstasyon ButtonEdit

        SfTextBox StationsButtonEdit;
        bool SelectStationsPopupVisible = false;
        List<ListStationsDto> StationsList = new List<ListStationsDto>();

        public async Task StationsOnCreateIcon()
        {
            var StationsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationsButtonClickEvent);
            await StationsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationsButtonClick } });
        }

        public async void StationsButtonClickEvent()
        {

            SelectStationsPopupVisible = true;
            StationsList = (await StationsAppService.GetListAsync(new ListStationsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);


        }

        public async void StationsOnValueChange(ChangedEventArgs args)
        {

            if (args.Value == null)
            {
                GeneralStatusDataSource.StationID = Guid.Empty;
                GeneralStatusDataSource.StationCode = string.Empty;
                await InvokeAsync(StateHasChanged);
            }

        }

        public async void StationsDoubleClickHandler(RecordDoubleClickEventArgs<ListStationsDto> args)
        {
            var selectedStation = args.RowData;

            if (selectedStation != null)
            {
                GeneralStatusDataSource.StationID = selectedStation.Id;
                GeneralStatusDataSource.StationCode = selectedStation.Code;
                GeneralStatusDataSource.isLoadCell = selectedStation.IsLoadCell;

                await SystemGeneralStatusLocalDbService.UpdateAsync(GeneralStatusDataSource);

                SelectStationsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion
    }
}
