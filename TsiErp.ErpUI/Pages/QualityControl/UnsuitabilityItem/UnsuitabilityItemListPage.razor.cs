using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.StationGroup.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityTypesItem.Dtos;

namespace TsiErp.ErpUI.Pages.QualityControl.UnsuitabilityItem
{
    partial class UnsuitabilityItemListPage
    {
        private List<SelectionList> SelectionLists = new List<SelectionList>() { new SelectionList { Text = "Evet", Id=1 }, new SelectionList { Text = "Hayır", Id=2 } };

        List<ListStationGroupsDto> StationGroupList = new List<ListStationGroupsDto>();

        List<ListUnsuitabilityTypesItemsDto> UnsuitabilityTypesItemList = new List<ListUnsuitabilityTypesItemsDto>();

        protected override async void OnInitialized()
        {
            BaseCrudService = UnsuitabilityItemsService;
            _L = L;
            UnsuitabilityTypesItemList = (await UnsuitabilityTypesItemsService.GetListAsync(new ListUnsuitabilityTypesItemsParameterDto())).Data.ToList();
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectUnsuitabilityItemsDto()
            {
                IsActive = true
            };

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        #region İş Merkezi ButtonEdit

        SfTextBox StationGroupButtonEdit;
        bool SelectStationGroupPopupVisible = false;

        public async Task StationGroupOnCreateIcon()
        {
            var StationGroupButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, StationGroupButtonClickEvent);
            await StationGroupButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", StationGroupButtonClick } });
        }

        public async void StationGroupButtonClickEvent()
        {
            SelectStationGroupPopupVisible = true;
            StationGroupList = (await StationGroupsService.GetListAsync(new ListStationGroupsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void StationGroupOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.StationGroupId = Guid.Empty;
                DataSource.StationGroupName = string.Empty;
            }
        }

        public async void StationGroupDoubleClickHandler(RecordDoubleClickEventArgs<ListStationGroupsDto> args)
        {
            var selectedStationGroup = args.RowData;

            if (selectedStationGroup != null)
            {
                DataSource.StationGroupId = selectedStationGroup.Id;
                DataSource.StationGroupName = selectedStationGroup.Name;
                SelectStationGroupPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        public void LifeThreateningOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void LossOfPrestigeOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ExtraCostOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ProductLifeShorteningOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void DetectabilityOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }

        public void ToBeUsedAsOnChange(Syncfusion.Blazor.DropDowns.ChangeEventArgs<string, SelectionList> args)
        {
        }
    }


    public class SelectionList
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }
}
