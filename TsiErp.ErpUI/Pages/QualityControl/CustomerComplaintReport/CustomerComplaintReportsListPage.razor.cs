using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Localization;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using TsiErp.Business.Entities.QualityControl.UnsuitabilityTypesItem.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Menu.Dtos;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.UserPermission.Dtos;
using TsiErp.Entities.Entities.QualityControl.CustomerComplaintReport.Dtos;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;
using TsiErp.Entities.Entities.QualityControl.UnsuitabilityItem.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrder.Dtos;
using TsiErp.Entities.Entities.SalesManagement.SalesOrderLine.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.ErpUI.Utilities.ModalUtilities;

namespace TsiErp.ErpUI.Pages.QualityControl.CustomerComplaintReport
{
    public partial class CustomerComplaintReportsListPage : IDisposable
    {
        [Inject]
        ModalManager ModalManager { get; set; }

        CreateReport8DsDto Report8DDataSource;

        public List<SelectUserPermissionsDto> UserPermissionsList = new List<SelectUserPermissionsDto>();
        public List<ListMenusDto> MenusList = new List<ListMenusDto>();
        public List<ListMenusDto> contextsList = new List<ListMenusDto>();


        public int stateComboIndex = 0;

        protected override async void OnInitialized()
        {
            BaseCrudService = CustomerComplaintReportsAppService;
            _L = L;
            #region Context Menü Yetkilendirmesi

            MenusList = (await MenusAppService.GetListAsync(new ListMenusParameterDto())).Data.ToList();
            var parentMenu = MenusList.Where(t => t.MenuName == "CustCompRecordsChildMenu").Select(t => t.Id).FirstOrDefault();
            contextsList = MenusList.Where(t => t.ParentMenuId == parentMenu).ToList();
            UserPermissionsList = (await UserPermissionsAppService.GetListAsyncByUserId(LoginedUserService.UserId)).Data.ToList();

            contextsList = contextsList.OrderBy(t => t.ContextOrderNo).ToList();
            #endregion
        }

        protected override async Task BeforeInsertAsync()
        {
            DataSource = new SelectCustomerComplaintReportsDto()
            {
                ReportDate = GetSQLDateAppService.GetDateFromSQL(),
                ReportNo = FicheNumbersAppService.GetFicheNumberAsync("CustCompRecordsChildMenu")
            };

            foreach (var item in _reportStateComboBox)
            {
                item.Text = L[item.Text];
            }

            EditPageVisible = true;

            await Task.CompletedTask;
        }

        public override async void ShowEditPage()
        {
            if (DataSource != null)
            {

                if (DataSource.DataOpenStatus == true && DataSource.DataOpenStatus != null)
                {
                    EditPageVisible = false;

                    string MessagePopupInformationDescriptionBase = L["MessagePopupInformationDescriptionBase"];

                    MessagePopupInformationDescriptionBase = MessagePopupInformationDescriptionBase.Replace("{0}", LoginedUserService.UserName);

                    await ModalManager.MessagePopupAsync(L["MessagePopupInformationTitleBase"], MessagePopupInformationDescriptionBase);
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    foreach (var item in _reportStateComboBox)
                    {
                        item.Text = L[item.Text];
                    }
                    #region String Combobox Index Ataması
                    string waiting = L["WaitingState"].Value;
                    string underreview = L["ComboboxUnderReview"].Value;
                    string report = L["Combobox8DReport"].Value;
                    string completed = L["ComboboxCompleted"].Value;
                    var a = DataSource.ReportState;

                    if (DataSource.ReportState == waiting) stateComboIndex = 0;
                    else if (DataSource.ReportState == underreview) stateComboIndex = 1;
                    else if (DataSource.ReportState == report) stateComboIndex = 2;
                    else if (DataSource.ReportState == completed) stateComboIndex = 3;
                    #endregion

                    EditPageVisible = true;
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
                            case "CusCompReportContextAdd":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CusCompReportContextAdd"], Id = "new" }); break;
                            case "CusCompReportContextChange":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CusCompReportContextChange"], Id = "changed" }); break;
                            case "CusCompReportContextCreate8D":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CusCompReportContextCreate8D"], Id = "create8d" }); break;
                            case "CusCompReportContextDelete":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CusCompReportContextDelete"], Id = "delete" }); break;
                            case "CusCompReportContextRefresh":
                                GridContextMenu.Add(new ContextMenuItemModel { Text = L["CusCompReportContextRefresh"], Id = "refresh" }); break;
                            default: break;
                        }
                    }
                }
            }
        }

        public override async void OnContextMenuClick(ContextMenuClickEventArgs<ListCustomerComplaintReportsDto> args)
        {
            switch (args.Item.Id)
            {
                case "new":
                    await BeforeInsertAsync();
                    break;

                case "changed":
                    IsChanged = true;
                    SelectFirstDataRow = false;
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;
                    ShowEditPage();
                    await InvokeAsync(StateHasChanged);
                    break;

                case "create8d":
                    DataSource = (await GetAsync(args.RowInfo.RowData.Id)).Data;

                    if (DataSource.is8DReport)
                    {
                        #region 8D Rapor Create Input Oluşturma

                        var customer = (await SalesOrdersAppService.GetAsync(DataSource.SalesOrderID.GetValueOrDefault())).Data;
                        var supplier = (await CurrentAccountCardsAppService.GetListAsync(new ListCurrentAccountCardsParameterDto())).Data.Where(t => t.IsSoftwareCompanyInformation == true).FirstOrDefault();
                        var technicaldrawing = (await TechnicalDrawingsAppService.GetListAsync(new ListTechnicalDrawingsParameterDto())).Data.Where(t => t.ProductID == DataSource.ProductID && t.CustomerApproval == true && t.IsApproved == true).FirstOrDefault();

                        Report8DDataSource = new CreateReport8DsDto
                        {
                            CustomerID = customer.CurrentAccountCardID,
                            SupplierID = supplier.Id,
                            ProductID = DataSource.ProductID,
                            State_ = L["WaitingState"],
                            TechnicalDrawingID = technicaldrawing.Id,
                            Code = FicheNumbersAppService.GetFicheNumberAsync("Report8DChildMenu"),
                            AtCustomerBlocked = 0,
                            AtCustomerChecked = 0,
                            AtCustomerDefect = 0,
                            AtSupplierBlocked = 0,
                            AtSupplierChecked = 0,
                            AtSupplierDefect = 0,
                            PartNumber = string.Empty,
                            CA1ContainmentAction = string.Empty,
                            CA1ContainmentActionD6 = string.Empty,
                            CA1ImplementationDate = null,
                            CA1PotentialRisk = string.Empty,
                            CA1RemovalDateD6 = null,
                            CA1Responsible = string.Empty,
                            CA1ResponsibleD6 = string.Empty,
                            CA2ContainmentAction = string.Empty,
                            CA2ContainmentActionD6 = string.Empty,
                            CA2ImplementationDate = null,
                            CA2PotentialRisk = string.Empty,
                            CA2RemovalDateD6 = null,
                            CA2Responsible = string.Empty,
                            CA2ResponsibleD6 = string.Empty,
                            CA3ContainmentAction = string.Empty,
                            CA3ContainmentActionD6 = string.Empty,
                            CA3ImplementationDate = null,
                            CA3PotentialRisk = string.Empty,
                            CA3RemovalDateD6 = null,
                            CA3Responsible = string.Empty,
                            CA3ResponsibleD6 = string.Empty,
                            ClaimedQuantity = 0,
                            ClaimingPlants = string.Empty,
                            ClaimOpeningDate = GetSQLDateAppService.GetDateFromSQL(),
                            ComplaintJustified = string.Empty,
                            ContainmentActionDate = null,
                            ControlPlanRevisionCompletionDate = null,
                            ControlPlanRevisionDocumentNumber = string.Empty,
                            ControlPlanRevisionProofAttached = string.Empty,
                            ControlPlanRevisionRelevant = string.Empty,
                            ControlPlanRevisionResponsible = string.Empty,
                            ControlPlanRevisionVersion = string.Empty,
                            Customer8DClosureDate = null,
                            Customer8DClosureFunctionDepartment = string.Empty,
                            Customer8DClosureName = string.Empty,
                            DateFinalRelease = null,
                            DateInterimReportD3 = null,
                            DateInterimReportD5 = null,
                            DeliveredQuantity = 0,
                            DeviationsProblems = string.Empty,
                            DeviationsSymptoms = string.Empty,
                            DFMEARevisionCompletionDate = null,
                            DFMEARevisionDocumentNumber = string.Empty,
                            DFMEARevisionProofAttached = string.Empty,
                            DFMEARevisionRelevant = string.Empty,
                            DFMEARevisionResponsible = string.Empty,
                            DFMEARevisionVersion = string.Empty,
                            DrawingIndex = string.Empty,
                            FailureOccurance = string.Empty,
                            IA1CorrectiveAction = string.Empty,
                            IA1EffectiveFromDate = null,
                            IA1ImplementationDate = null,
                            IA1ProofAttached = string.Empty,
                            IA1RootCause = string.Empty,
                            IA1ValidatedDate = null,
                            IA2CorrectiveAction = string.Empty,
                            IA2EffectiveFromDate = null,
                            IA2ImplementationDate = null,
                            IA2ProofAttached = string.Empty,
                            IA2RootCause = string.Empty,
                            IA2ValidatedDate = null,
                            IA3CorrectiveAction = string.Empty,
                            IA3EffectiveFromDate = null,
                            IA3ImplementationDate = null,
                            IA3ProofAttached = string.Empty,
                            IA3RootCause = string.Empty,
                            IA3ValidatedDate = null,
                            IA4CorrectiveAction = string.Empty,
                            IA4EffectiveFromDate = null,
                            IA4ImplementationDate = null,
                            IA4ProofAttached = string.Empty,
                            IA4RootCause = string.Empty,
                            IA4ValidatedDate = null,
                            IA5CorrectiveAction = string.Empty,
                            IA5EffectiveFromDate = null,
                            IA5ImplementationDate = null,
                            IA5ProofAttached = string.Empty,
                            IA5RootCause = string.Empty,
                            IA5ValidatedDate = null,
                            InTransitBlocked = 0,
                            InTransitChecked = 0,
                            InTransitDefect = 0,
                            LessonsLearnedDate = null,
                            LessonsLearnedFunctionDepartment = string.Empty,
                            LessonsLearnedProofAttached = string.Empty,
                            LessonsLearnedRelevant = string.Empty,
                            LessonsLearnedResponsible = string.Empty,
                            OtherAffectedPlants = string.Empty,
                            PA1PlannedImplementationDate = null,
                            PA1PotentialCorrectiveAction = string.Empty,
                            PA1Responsible = string.Empty,
                            PA1RootCause = string.Empty,
                            PA1ToBeImplemented = string.Empty,
                            PA2PlannedImplementationDate = null,
                            PA2PotentialCorrectiveAction = string.Empty,
                            PA2Responsible = string.Empty,
                            PA2RootCause = string.Empty,
                            PA2ToBeImplemented = string.Empty,
                            PA3PlannedImplementationDate = null,
                            PA3PotentialCorrectiveAction = string.Empty,
                            PA3Responsible = string.Empty,
                            PA3RootCause = string.Empty,
                            PA3ToBeImplemented = string.Empty,
                            PA4PlannedImplementationDate = null,
                            PA4PotentialCorrectiveAction = string.Empty,
                            PA4Responsible = string.Empty,
                            PA4RootCause = string.Empty,
                            PA4ToBeImplemented = string.Empty,
                            PA5PlannedImplementationDate = null,
                            PA5PotentialCorrectiveAction = string.Empty,
                            PA5Responsible = string.Empty,
                            PA5RootCause = string.Empty,
                            PA5ToBeImplemented = string.Empty,
                            PFMEARevisionCompletionDate = null,
                            PFMEARevisionDocumentNumber = string.Empty,
                            PFMEARevisionProofAttached = string.Empty,
                            PFMEARevisionRelevant = string.Empty,
                            PFMEARevisionResponsible = string.Empty,
                            PFMEARevisionVersion = string.Empty,
                            ProductionPlant = string.Empty,
                            Report8DAccepted = string.Empty,
                            Report8DRevision = string.Empty,
                            Revision1Action = string.Empty,
                            Revision1CompletionDate = null,
                            Revision1DocumentNumber = string.Empty,
                            Revision1ProofAttached = string.Empty,
                            Revision1Relevant = string.Empty,
                            Revision1Responsible = string.Empty,
                            Revision1Version = string.Empty,
                            Revision2Action = string.Empty,
                            Revision2CompletionDate = null,
                            Revision2DocumentNumber = string.Empty,
                            Revision2ProofAttached = string.Empty,
                            Revision2Relevant = string.Empty,
                            Revision2Responsible = string.Empty,
                            Revision2Version = string.Empty,
                            Revision3Action = string.Empty,
                            Revision3CompletionDate = null,
                            Revision3DocumentNumber = string.Empty,
                            Revision3ProofAttached = string.Empty,
                            Revision3Relevant = string.Empty,
                            Revision3Responsible = string.Empty,
                            Revision3Version = string.Empty,
                            RN1AnalysisMethod = string.Empty,
                            RN1NonDetectionReason = string.Empty,
                            RN1Share = string.Empty,
                            RN2AnalysisMethod = string.Empty,
                            RN2NonDetectionReason = string.Empty,
                            RN2Share = string.Empty,
                            RO1AnalysisMethod = string.Empty,
                            RO1OccuranceReason = string.Empty,
                            RO1Share = string.Empty,
                            RO2AnalysisMethod = string.Empty,
                            RO2OccuranceReason = string.Empty,
                            RO2Share = string.Empty,
                            Sponsor = string.Empty,
                            SponsorD8 = string.Empty,
                            SponsorDateD8 = null,
                            SponsorEMail = string.Empty,
                            SponsorFunctionDepartment = string.Empty,
                            SponsorPhone = string.Empty,
                            TeamLeader = string.Empty,
                            TeamLeaderD8 = string.Empty,
                            TeamLeaderDateD8 = null,
                            TeamLeaderEMail = string.Empty,
                            TeamLeaderFunctionDepartment = string.Empty,
                            TeamLeaderPhone = string.Empty,
                            TeamMember1 = string.Empty,
                            TeamMember1EMail = string.Empty,
                            TeamMember1FunctionDepartment = string.Empty,
                            TeamMember1Phone = string.Empty,
                            TeamMember2 = string.Empty,
                            TeamMember2EMail = string.Empty,
                            TeamMember2FunctionDepartment = string.Empty,
                            TeamMember2Phone = string.Empty,
                            TeamMember3 = string.Empty,
                            TeamMember3EMail = string.Empty,
                            TeamMember3FunctionDepartment = string.Empty,
                            TeamMember3Phone = string.Empty,
                            TopicTitle = string.Empty,
                            UpdateRequiredUntilDate = null,

                            CreationTime = DateTime.Now,
                            CreatorId = LoginedUserService.UserId,
                            DataOpenStatus = false,
                            DataOpenStatusUserId = Guid.Empty,
                            DeleterId = Guid.Empty,
                            DeletionTime = null,
                            //Id = addedEntityId,
                            IsDeleted = false,
                            LastModificationTime = null,
                            LastModifierId = Guid.Empty,
                        };

                        #endregion

                        await Report8DsAppService.CreateAsync(Report8DDataSource);

                    }

                    else
                    {
                        await ModalManager.WarningPopupAsync(L["UIWarning8DReportTitle"], L["UIWarning8DReportMessage"]);
                    }

                    break;

                case "delete":

                    var res = await ModalManager.ConfirmationAsync(L["DeleteConfirmationTitleBase"], L["DeleteConfirmationDescriptionBase"]);


                    if (res == true)
                    {
                        SelectFirstDataRow = false;
                        await DeleteAsync(args.RowInfo.RowData.Id);
                        await GetListDataSourceAsync();
                        await InvokeAsync(StateHasChanged);
                    }

                    break;

                case "refresh":
                    await GetListDataSourceAsync();
                    await InvokeAsync(StateHasChanged);
                    break;

                default:
                    break;
            }
        }


        public void AddFile()
        {

        }


        #region Şikayet Başlığı ButtonEdit

        SfTextBox UnsuitabilityItemsButtonEdit;
        bool SelectUnsuitabilityItemsPopupVisible = false;
        List<ListUnsuitabilityItemsDto> UnsuitabilityItemsList = new List<ListUnsuitabilityItemsDto>();

        public async Task UnsuitabilityItemsOnCreateIcon()
        {
            var UnsuitabilityItemsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, UnsuitabilityItemsButtonClickEvent);
            await UnsuitabilityItemsButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", UnsuitabilityItemsButtonClick } });
        }

        public async void UnsuitabilityItemsButtonClickEvent()
        {
            SelectUnsuitabilityItemsPopupVisible = true;
            await GetUnsuitabilityItemsList();
            await InvokeAsync(StateHasChanged);
        }
    


        public void UnsuitabilityItemsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.UnsuitqabilityItemsID = Guid.Empty;
                DataSource.UnsuitqabilityItemsName = string.Empty;
            }
        }

        public async void UnsuitabilityItemsDoubleClickHandler(RecordDoubleClickEventArgs<ListUnsuitabilityItemsDto> args)
        {
            var selectedUnsuitabilityItem = args.RowData;

            if (selectedUnsuitabilityItem != null)
            {
                DataSource.UnsuitqabilityItemsID = selectedUnsuitabilityItem.Id;
                DataSource.UnsuitqabilityItemsName = selectedUnsuitabilityItem.Name;
                SelectUnsuitabilityItemsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion
        private async Task GetUnsuitabilityItemsList()
        {

            var unsuitabilityTypesItem = (await UnsuitabilityTypesItemsAppService.GetWithUnsuitabilityItemDescriptionAsync("Advertisement")).Data;

            if (unsuitabilityTypesItem != null)
            {
                UnsuitabilityItemsList = (await UnsuitabilityItemsAppService.GetListAsync(new ListUnsuitabilityItemsParameterDto())).Data.Where(t => t.UnsuitabilityTypesItemsId == unsuitabilityTypesItem.Id).ToList();
            }
        }

        #region Satış Siparişi ButtonEdit

        SfTextBox SalesOrdersButtonEdit;
        bool SelectSalesOrdersPopupVisible = false;
        List<ListSalesOrderDto> SalesOrdersList = new List<ListSalesOrderDto>();
        List<SelectSalesOrderLinesDto> SalesOrdersLineList = new List<SelectSalesOrderLinesDto>();

        public async Task SalesOrdersOnCreateIcon()
        {
            var SalesOrdersButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, SalesOrdersButtonClickEvent);
            await SalesOrdersButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", SalesOrdersButtonClick } });
        }

        public async void SalesOrdersButtonClickEvent()
        {
            SelectSalesOrdersPopupVisible = true;
            SalesOrdersList = (await SalesOrdersAppService.GetListAsync(new ListSalesOrderParameterDto())).Data.ToList();

            await InvokeAsync(StateHasChanged);
        }


        public void SalesOrdersOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.SalesOrderID = Guid.Empty;
                DataSource.SalesOrderFicheNo = string.Empty;
                SalesOrdersLineList.Clear();
            }
        }

        public async void SalesOrdersDoubleClickHandler(RecordDoubleClickEventArgs<ListSalesOrderDto> args)
        {
            var selectedSalesOrder = args.RowData;

            if (selectedSalesOrder != null)
            {
                DataSource.SalesOrderID = selectedSalesOrder.Id;
                DataSource.SalesOrderFicheNo = selectedSalesOrder.FicheNo;
                SalesOrdersLineList = (await SalesOrdersAppService.GetAsync(selectedSalesOrder.Id)).Data.SelectSalesOrderLines.ToList();
                SelectSalesOrdersPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }
        #endregion

        #region Stok Kartı Button Edit

        SfTextBox ProductsCodeButtonEdit = new();
        SfTextBox ProductsNameButtonEdit = new();
        bool SelectProductsPopupVisible = false;
        List<ListProductsDto> ProductsList = new List<ListProductsDto>();
        public async Task ProductsCodeOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsCodeButtonClickEvent);
            await ProductsCodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsCodeButtonClickEvent()
        {
            if (DataSource.SalesOrderID == Guid.Empty || DataSource.SalesOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSalesOrderTitle"], L["UIWarningSalesOrderMessage"]);
            }
            else
            {
                ProductsList = new List<ListProductsDto>();

                var tempproductsList = (await ProductAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                foreach (var product in tempproductsList)
                {
                    if (SalesOrdersLineList.Any(t => t.ProductID == product.Id))
                    {
                        ProductsList.Add(product);
                    }
                }

                SelectProductsPopupVisible = true;
            }
            await InvokeAsync(StateHasChanged);
        }

        public async Task ProductsNameOnCreateIcon()
        {
            var ProductsButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, ProductsNameButtonClickEvent);
            await ProductsNameButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", ProductsButtonClick } });
        }

        public async void ProductsNameButtonClickEvent()
        {
            if (DataSource.SalesOrderID == Guid.Empty || DataSource.SalesOrderID == null)
            {
                await ModalManager.WarningPopupAsync(L["UIWarningSalesOrderTitle"], L["UIWarningSalesOrderMessage"]);
            }
            else
            {
                ProductsList = new List<ListProductsDto>();
                var tempproductsList = (await ProductAppService.GetListAsync(new ListProductsParameterDto())).Data.ToList();

                foreach (var product in tempproductsList)
                {
                    if (SalesOrdersLineList.Any(t => t.ProductID == product.Id))
                    {
                        ProductsList.Add(product);
                    }
                }

                SelectProductsPopupVisible = true;
            }
            await InvokeAsync(StateHasChanged);
        }

        public void ProductsOnValueChange(ChangedEventArgs args)
        {
            if (args.Value == null)
            {
                DataSource.ProductID = Guid.Empty;
                DataSource.ProductCode = string.Empty;
                DataSource.ProductName = string.Empty;
                DataSource.DeliveredQuantity = 0;
            }
        }

        public async void ProductsDoubleClickHandler(RecordDoubleClickEventArgs<ListProductsDto> args)
        {
            var selectedProduct = args.RowData;

            if (selectedProduct != null)
            {
                DataSource.ProductID = selectedProduct.Id;
                DataSource.ProductCode = selectedProduct.Code;
                DataSource.ProductName = selectedProduct.Name;
                DataSource.DeliveredQuantity = SalesOrdersLineList.Where(t => t.ProductID == selectedProduct.Id).Sum(t => t.Quantity);
                SelectProductsPopupVisible = false;
                await InvokeAsync(StateHasChanged);
            }
        }

        #endregion

        #region Kod ButtonEdit

        SfTextBox CodeButtonEdit;

        public async Task CodeOnCreateIcon()
        {
            var CodesButtonClick = EventCallback.Factory.Create<MouseEventArgs>(this, CodeButtonClickEvent);
            await CodeButtonEdit.AddIconAsync("append", "e-search-icon", new Dictionary<string, object>() { { "onclick", CodesButtonClick } });
        }

        public async void CodeButtonClickEvent()
        {
            DataSource.ReportNo = FicheNumbersAppService.GetFicheNumberAsync("CustCompRecordsChildMenu");
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region ComboBox İşlemleri

        public class ReportStateComboBox
        {
            public string ID { get; set; }
            public string Text { get; set; }
        }

        List<ReportStateComboBox> _reportStateComboBox = new List<ReportStateComboBox>
        {
            new ReportStateComboBox(){ID = "Pending", Text="WaitingState"},
            new ReportStateComboBox(){ID = "UnderReview", Text="ComboboxUnderReview"},
            new ReportStateComboBox(){ID = "8DReport", Text="Combobox8DReport"},
            new ReportStateComboBox(){ID = "Completed", Text="ComboboxCompleted"}
        };

        private void ReportStateComboBoxValueChangeHandler(ChangeEventArgs<string, ReportStateComboBox> args)
        {
            if (args.ItemData != null)
            {

                switch (args.ItemData.ID)
                {
                    case "Pending":
                        DataSource.ReportState = L["WaitingState"].Value;
                        stateComboIndex = 0;
                        break;

                    case "UnderReview":
                        DataSource.ReportState = L["ComboboxUnderReview"].Value;
                        stateComboIndex = 1;
                        break;

                    case "8DReport":
                        DataSource.ReportState = L["Combobox8DReport"].Value;
                        stateComboIndex = 2;
                        break;

                    case "Completed":
                        DataSource.ReportState = L["ComboboxCompleted"].Value;
                        stateComboIndex = 3;
                        break;

                    default: break;
                }
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
