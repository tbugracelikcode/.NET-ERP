﻿using DevExpress.Xpo.DB;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPC.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationalSPCLine.Dtos;
using TsiErp.Entities.Entities.QualityControl.PFMEA.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.PFMEA
{
    public partial class PFMEAsListPage : IDisposable
    {

        public List<SelectOperationalSPCLinesDto> OperationalSPCLineList = new List<SelectOperationalSPCLinesDto>();
        public List<SelectOperationalSPCLinesDto> SecondOperationalSPCLineList = new List<SelectOperationalSPCLinesDto>();

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();

        [Inject]
        ModalManager ModalManager { get; set; }

        public bool firstSPCButtonDisabled = true;
        public bool secondSPCButtonDisabled = true;
        private int? ddlIndex { get; set; } = 0;

        protected override async void OnInitialized()
        {
            BaseCrudService = PFMEAsService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "PFMEAChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override Task BeforeInsertAsync()
        {
            DataSource = new SelectPFMEAsDto()
            {
                Date_ = GetSQLDateAppService.GetDateFromSQL().Date,
                ActionCompletionDate = GetSQLDateAppService.GetDateFromSQL().Date,
                LineNr = ListDataSource.Count + 1,
                State = L["ComboboxTracking"]
            };

            foreach(var item in StatusList)
            {
                item.Name = L[item.Name];
            }

            ddlIndex = 0;

            EditPageVisible = true;

            return Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {

            if (DataSource != null)
            {
                bool? dataOpenStatus = DataSource.DataOpenStatus;

                if (dataOpenStatus == true && dataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    EditPageVisible = true;

                    foreach (var item in StatusList)
                    {
                        item.Name = L[item.Name];
                    }

                    if (DataSource.State == L["ComboboxTracking"].Value)
                    {
                        ddlIndex = 0;
                    }
                    else
                    {
                        ddlIndex = 1;
                    }
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        protected override void CreateContextMenuItems(IStringLocalizer L)
        {
            if (GridContextMenu.Count == 0)
            {

                foreach (var context in contextsList)
                {
                    var permission = UserPermissionsList.Where(t => t.MenuId == context.Id).Select(t => t.IsUserPermitted).FirstOrDefault();
                    if (permission)
                    {
                        switch (context.MenuName)
                        {
                            case "PFMEAContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PFMEAContextAdd"], Id = "new" }); break;
                            case "PFMEAContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PFMEAContextChange"], Id = "changed" }); break;
                            case "PFMEAContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PFMEAContextDelete"], Id = "delete" }); break;
                            case "PFMEAContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["PFMEAContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        #region 1. SPC ButtonEdit

        SfTextBox FirstSPCButtonEdit;
        bool SelectFirstSPCPopupVisible = false;
        List<ListOperationalSPCsDto> FirstSPCList = new List<ListOperationalSPCsDto>();

        public async Task FirstSPCOnCreateIcon()
        {
            var FirstSPCButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, FirstSPCButtonClickEvent);
            await FirstSPCButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", FirstSPCButtonClick } });
        }

        public async void FirstSPCButtonClickEvent()
        {
            SelectFirstSPCPopupVisible = true;
            FirstSPCList = (await OperationalSPCsAppService.GetListAsync(new ListOperationalSPCsParameterDto())).Data.ToList();
            await InvokeAsync(StateHasChanged);
        }

        public void FirstSPCOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.FirstOperationalSPCID = Guid.Empty;
                DataSource.FirstOperationalSPCCode = string.Empty;
                OperationalSPCLineList.Clear();
                firstSPCButtonDisabled = true;
                ddlIndex = 0;
                DataSource.State = L["ComboboxTracking"].Value;
            }
        }

        public async void FirstSPCDoubleClickHandler(RecordDoubleClickEventArgs<ListOperationalSPCsDto> args)
        {
            var selectedFirstSPC = args.RowData;

            if (selectedFirstSPC != null)
            {
                DataSource.FirstOperationalSPCID = selectedFirstSPC.Id;
                DataSource.FirstOperationalSPCCode = selectedFirstSPC.Code;
                OperationalSPCLineList.Clear();
                OperationalSPCLineList = (await OperationalSPCsAppService.GetAsync(selectedFirstSPC.Id)).Data.SelectOperationalSPCLines.ToList();
                firstSPCButtonDisabled = false;
                ddlIndex = 1;
                DataSource.State = L["ComboboxCompleted"].Value;
                SelectFirstSPCPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region 1. SPC Satır ButtonEdit


        bool SelectFirstSPCLinePopupVisible = false;

        public async void FirstSPCLineButtonClick()
        {

            SelectFirstSPCLinePopupVisible = true;
            await InvokeAsync(StateHasChanged);
        }


        public void FirstSPCLineOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.WorkCenterID = Guid.Empty;
                DataSource.WorkCenterName = string.Empty;
                DataSource.OperationID = Guid.Empty;
                DataSource.OperationName = string.Empty;
                DataSource.CurrentDetectability = 0;
                DataSource.CurrentFrequency = 0;
                DataSource.CurrentRPN = 0;
                DataSource.CurrentSeverity = 0;
            }
        }

        public async void FirstSPCLineDoubleClickHandler(RecordDoubleClickEventArgs<SelectOperationalSPCLinesDto> args)
        {
            var selectedFirstLineSPC = args.RowData;

            if (selectedFirstLineSPC != null)
            {
                DataSource.WorkCenterID = selectedFirstLineSPC.WorkCenterID;
                DataSource.WorkCenterName = selectedFirstLineSPC.WorkCenterName;
                DataSource.OperationID = selectedFirstLineSPC.OperationID;
                DataSource.OperationName = selectedFirstLineSPC.OperationName;
                DataSource.CurrentSeverity = selectedFirstLineSPC.Severity;
                DataSource.CurrentFrequency = selectedFirstLineSPC.Frequency;
                DataSource.CurrentDetectability = selectedFirstLineSPC.Detectability;
                DataSource.CurrentRPN = selectedFirstLineSPC.RPN;
                SelectFirstSPCLinePopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Uygunsuzluk Başlığı ButtonEdit

        SfTextBox UnsuitabilityItemButtonEdit;
        bool SelectUnsuitabilityItemPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemOnCreateIcon()
        {
            var UnsuitabilityItemButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemButtonClickEvent);
            await UnsuitabilityItemButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemButtonClick } });
        }

        public async void UnsuitabilityItemButtonClickEvent()
        {
            if (DataSource.WorkCenterID == null || DataSource.WorkCenterID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningWorkCenterTitle"], L["UIWarningWorkCenterMessage"]);
            }
            else
            {
                SelectUnsuitabilityItemPopupVisible = true;
                UnsuitabilityItemList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.Where(t => t.StationGroupId == DataSource.WorkCenterID).ToList();
            }

            await InvokeAsync(StateHasChanged);
        }

        public void UnsuitabilityItemOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.UnsuitabilityItemID = Guid.Empty;
                DataSource.UnsuitabilityItemName = string.Empty;
            }
        }

        public async void UnsuitabilityItemDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedUnsuitabilityItem = args.RowData;

            if (selectedUnsuitabilityItem != null)
            {
                DataSource.UnsuitabilityItemID = selectedUnsuitabilityItem.Id;
                DataSource.UnsuitabilityItemName = selectedUnsuitabilityItem.Name;
                SelectUnsuitabilityItemPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region 2. SPC ButtonEdit

        SfTextBox SecondSPCButtonEdit;
        bool SelectSecondSPCPopupVisible = false;
        List<ListOperationalSPCsDto> SecondSPCList = new List<ListOperationalSPCsDto>();

        public async Task SecondSPCOnCreateIcon()
        {
            var SecondSPCButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SecondSPCButtonClickEvent);
            await SecondSPCButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SecondSPCButtonClick } });
        }

        public async void SecondSPCButtonClickEvent()
        {
            if (DataSource.FirstOperationalSPCID == null || DataSource.FirstOperationalSPCID == Guid.Empty)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningFirstOperationalSPCTitle"], L["UIWarningFirstOperationalSPCMessage"]);
            }
            else
            {
                SelectSecondSPCPopupVisible = true;
                SecondSPCList = (await OperationalSPCsAppService.GetListAsync(new ListOperationalSPCsParameterDto())).Data.ToList();
            }
            await InvokeAsync(StateHasChanged);
        }

        public void SecondSPCOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SecondOperationalSPCID = Guid.Empty;
                DataSource.SecondOperationalSPCCode = string.Empty;
                OperationalSPCLineList.Clear();
                secondSPCButtonDisabled = true;
                ddlIndex = 1;
                DataSource.State = L["ComboboxCompleted"].Value;
            }
        }

        public async void SecondSPCDoubleClickHandler(RecordDoubleClickEventArgs<ListOperationalSPCsDto> args)
        {
            var selectedSecondSPC = args.RowData;

            if (selectedSecondSPC != null)
            {
                DataSource.SecondOperationalSPCID = selectedSecondSPC.Id;
                DataSource.SecondOperationalSPCCode = selectedSecondSPC.Code;
                SecondOperationalSPCLineList.Clear();
                SecondOperationalSPCLineList = (await OperationalSPCsAppService.GetAsync(selectedSecondSPC.Id)).Data.SelectOperationalSPCLines.Where(t => t.WorkCenterID == DataSource.WorkCenterID && t.OperationID == DataSource.OperationID).ToList();
                SelectSecondSPCPopupVisible = false;
                secondSPCButtonDisabled = false;
                ddlIndex = 0;
                DataSource.State = L["ComboboxTracking"].Value;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region 2. SPC Satır ButtonEdit

        bool SelectSecondSPCLinePopupVisible = false;

        public async void SecondSCPLineButtonClick()
        {
            SelectSecondSPCLinePopupVisible = true;
            await InvokeAsync(StateHasChanged);
        }

        public void SecondSPCLineOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.NewDetectability = 0;
                DataSource.NewFrequency = 0;
                DataSource.NewRPN = 0;
                DataSource.NewSeverity = 0;
            }
        }

        public async void SecondSPCLineDoubleClickHandler(RecordDoubleClickEventArgs<SelectOperationalSPCLinesDto> args)
        {
            var selectedSecondLineSPC = args.RowData;

            if (selectedSecondLineSPC != null)
            {
                DataSource.NewSeverity = selectedSecondLineSPC.Severity;
                DataSource.NewFrequency = selectedSecondLineSPC.Frequency;
                DataSource.NewDetectability = selectedSecondLineSPC.Detectability;
                DataSource.NewRPN = selectedSecondLineSPC.RPN;
                SelectSecondSPCLinePopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Combobox İşlemleri


        public class Status
        {
            public string Name { get; set; }

            public string Code { get; set; }
        }

        List<Status> StatusList = new List<Status>
{
        new Status() { Name = "ComboboxTracking", Code = "TK" },
        new Status() { Name = "ComboboxCompleted", Code = "TM" },
    };
        private void ValueChangeHandler(ChangeEventArgs<string, Status> args)
        {
            if (args.Value == "TK")
            {
                DataSource.State = L["ComboboxTracking"];
                ddlIndex = 0;
            }
            else if (args.Value == "TM")
            {
                DataSource.State = L["ComboboxCompleted"];
                ddlIndex = 1;
            }
        }

        #endregion


        public void Dispose()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
