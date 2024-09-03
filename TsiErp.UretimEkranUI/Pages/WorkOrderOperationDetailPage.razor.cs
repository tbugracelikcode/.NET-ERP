using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Syncfusion.Blazor.Grids;
using System.Timers;
using TsiErp.Entities.Entities.ProductionManagement.HaltReason.Dtos;
using TsiErp.Entities.Entities.ProductionManagement.ProductionTracking.Dtos;
using TsiErp.Entities.Entities.QualityControl.OperationUnsuitabilityReport.Dtos;
using TsiErp.UretimEkranUI.Models;
using TsiErp.UretimEkranUI.Services;
using TsiErp.UretimEkranUI.Utilities.ModalUtilities;

namespace TsiErp.UretimEkranUI.Pages
{
    public partial class WorkOrderOperationDetailPage : IDisposable
    {

        #region Button Disable Properties

        public bool StartOperationButtonDisabled { get; set; } = false;
        public bool PauseOperationButtonDisabled { get; set; } = true;
        public bool EndOperationButtonDisabled { get; set; } = true;
        public bool ScrapQuantityButtonDisabled { get; set; } = true;
        public bool ChangeOperationButtonDisabled { get; set; } = false;
        public bool ChangeShiftButtonDisabled { get; set; } = false;
        public bool ChangeCaseButtonDisabled { get; set; } = false;
        public bool IncreaseQuantityDisabled { get; set; } = true;

        #endregion

        #region File Upload Değişkenleri

        //List<IFileListEntry> files = new List<IFileListEntry>();

        public List<IFileListEntry> lineFiles = new List<IFileListEntry>();

        List<System.IO.FileInfo> uploadedfiles = new List<System.IO.FileInfo>();

        bool UploadedFile = false;

        bool ImagePreviewPopup = false;

        string previewImagePopupTitle = string.Empty;

        string imageDataUri;

        string PDFrootPath;

        bool image = false;

        bool pdf = false;

        string PDFFileName;

        public bool OperationPicturesChangedCrudPopup = false;

        #endregion

        #region Değişkenler
        public string TotalAdjusmentTime { get; set; }
        public decimal QualityPercent { get; set; } = 0;

        public bool ScrapQuantityEntryModalVisible = false;

        public decimal scrapQuantity = 0;

        public decimal scrapQuantityEntry = 0;

        public string[] MenuItems = new string[] { "Group", "Ungroup", "ColumnChooser", "Filter" };

        public Dictionary<string, object> attr = new Dictionary<string, object>() {
          { "type", "number" }
    };

        public bool UnsuitabilityQuantityEntryModalVisible = false;

        public List<ScrapTable> UnsuitabilityQuantityEntriesList = new List<ScrapTable>();

        public bool isPasswordVisible = false;

        string password = string.Empty;

        ListHaltReasonsDto haltReasonIncidental = new ListHaltReasonsDto();

        public bool ChangeCrateButtonVisible = true;

        DateTime starthaltDate = DateTime.MinValue;

        SfGrid<Models.ScrapTable> _UnsuitabilityQuantityEntriesGrid;

        public bool TechnicalDrawingsModalVisible = false;

        #endregion

        [Inject]
        ModalManager ModalManager { get; set; }

        public IConfigurationRoot ConfigurationRoot { get; set; }

        protected override async void OnInitialized()
        {

            ConfigurationRoot = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();



            var totalAdjusmentTime = (await ProductionTrackingsAppService.GetListbyWorkOrderIDAsync(AppService.CurrentOperation.WorkOrderID)).Data.Sum(t => t.AdjustmentTime);

            if (totalAdjusmentTime > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(totalAdjusmentTime));

                TotalAdjusmentTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                    time.Hours,
                    time.Minutes,
                    time.Seconds);
            }

            AppService.CurrentOperation.EmployeeID = AppService.EmployeeID;
            AppService.CurrentOperation.EmployeeName = AppService.EmployeeName;

            var operationUnsuitabilityReportList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.WorkOrderID == AppService.CurrentOperation.WorkOrderID && t.StationCode == AppService.CurrentOperation.StationCode && t.Action_ == "Hurda").ToList();
            scrapQuantity = operationUnsuitabilityReportList.Sum(t => t.UnsuitableAmount);

            AppService.CurrentOperation.ScrapQuantity = scrapQuantity;

            QualityPercent = 1;

            ScrapQuantityCalculate();
            StartScrapTimer();

            var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

            var station = (await StationsAppService.GetAsync(generalStatus.StationID)).Data;

            if (station != null && station.Id != Guid.Empty)
            {
                generalStatus.isLoadCell = station.IsLoadCell;

                await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);

                if (station.IsLoadCell)
                {
                    ChangeCrateButtonVisible = true;
                }
                else
                {
                    ChangeCrateButtonVisible = false;
                }
            }
        }

        private async void ProducedQuantityAdded()
        {
            //AppService.CurrentOperation.WorkOrder.ProducedQuantity = AppService.CurrentOperation.WorkOrder.ProducedQuantity + 1;

            //QualityPercent = 1 - (AppService.CurrentOperation.ScrapQuantity / AppService.CurrentOperation.WorkOrder.ProducedQuantity);

            await InvokeAsync(StateHasChanged);
        }

        #region Operation Start

        public decimal FirstProducedQuantity = 0;

        public async void OperationStartButtonClicked()
        {
            FirstProducedQuantity = AppService.CurrentOperation.ProducedQuantity;

            ScrapQuantityButtonDisabled = false;
            EndOperationButtonDisabled = false;
            PauseOperationButtonDisabled = false;
            StartOperationButtonDisabled = true;
            IncreaseQuantityDisabled = false;
            ChangeOperationButtonDisabled = true;

            StartOperationTimer();


            await StationsAppService.UpdateStationWorkStateAsync(AppService.CurrentOperation.StationID, 2);

            #region Sistem Genel Durum Update
            var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

            if (generalStatus != null)
            {
                generalStatus.GeneralStatus = 2;

                await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);
            }
            #endregion

            await InvokeAsync(StateHasChanged);
        }

        public async void IncreaseQuantity()
        {
            AppService.CurrentOperation.ProducedQuantity = AppService.CurrentOperation.ProducedQuantity + 1;

            FirstProducedQuantity = AppService.CurrentOperation.ProducedQuantity;

            UpdatedOperationStartTime = DateTime.Now;

            var updatedWorkOrder = await OperationDetailLocalDbService.GetAsync(AppService.CurrentOperation.Id);

            updatedWorkOrder.ProducedQuantity += 1;

            await OperationDetailLocalDbService.UpdateAsync(updatedWorkOrder);

            ScrapQuantityCalculate();
        }

        #endregion

        #region Operation Stop

        public async void OperationStopButtonClicked()
        {
            StopOperationTimer();

            StartSystemIdleTimer();

            NavigationManager.NavigateTo("/halt-reasons");

            await InvokeAsync(StateHasChanged);
        }



        #endregion

        #region End Operation

        public async void EndOperationButtonClicked()
        {
            var plannedQuantity = AppService.CurrentOperation.PlannedQuantity;
            var producedQuantity = AppService.CurrentOperation.ProducedQuantity;
            var scrapQuantity = AppService.CurrentOperation.ScrapQuantity;
            var currentWorkOrderID = AppService.CurrentOperation.WorkOrderID;

            if (plannedQuantity <= producedQuantity + scrapQuantity) // Work Order has completed
            {

                #region Current Operation Delete

                var currentOperationList = (await OperationDetailLocalDbService.GetListAsync()).ToList();

                if (currentOperationList.Count > 0 && currentOperationList != null)
                {
                    await OperationDetailLocalDbService.DeleteAsync(currentOperationList[0]);
                }

                #endregion

                #region Unsuitable Table Delete

                var scraplist = (await ScrapLocalDbService.GetListAsync()).ToList();

                if (scraplist.Count > 0 && scraplist != null)
                {
                    foreach (var item in scraplist)
                    {
                        await ScrapLocalDbService.DeleteAsync(item);
                    }
                }

                #endregion

                #region Adjustment Table Delete

                var adjustmentList = (await OperationAdjustmentLocalDbService.GetListAsync()).ToList();

                if (adjustmentList.Count > 0 && adjustmentList != null)
                {
                    foreach (var item in adjustmentList)
                    {
                        await OperationAdjustmentLocalDbService.DeleteAsync(item);
                    }
                }

                #endregion

                #region Halt Reason Table Delete

                var haltList = (await OperationHaltReasonsTableLocalDbService.GetListAsync()).ToList();

                if (haltList.Count > 0 && haltList != null)
                {
                    foreach (var item in haltList)
                    {
                        await OperationHaltReasonsTableLocalDbService.DeleteAsync(item);
                    }
                }

                #endregion

                #region Logged User Table Delete

                var loggedUser = (await LoggedUserLocalDbService.GetListAsync()).ToList();

                if (loggedUser.Count > 0 && loggedUser != null)
                {
                    foreach (var item in loggedUser)
                    {
                        if (!item.IsAuthorizedUser)
                        {
                            await LoggedUserLocalDbService.DeleteAsync(item);
                        }
                    }
                }

                #endregion

                #region System General Status Table Update

                var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).ToList();

                if (generalStatus.Count > 0 && generalStatus != null)
                {
                    foreach (var item in generalStatus)
                    {
                        item.isLoadCell = false;
                        await SystemGeneralStatusLocalDbService.UpdateAsync(item);
                    }
                }

                #endregion

                #region ERP DB WorkOrder - Operation Stock Movement - Production Tracking Status Update

                var workOrderDataSource = (await WorkOrdersAppService.GetAsync(currentWorkOrderID)).Data;

                if (workOrderDataSource.Id != Guid.Empty && workOrderDataSource != null)
                {
                    string[] totalOprTime = TotalOperationTime.Split(':');
                    decimal oprTime = (Convert.ToDecimal(totalOprTime[0]) * 3600) + (Convert.ToDecimal(totalOprTime[1]) * 60) + Convert.ToDecimal(totalOprTime[2]);

                    decimal adjTime = Convert.ToDecimal(await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

                    var today = GetSQLDateAppService.GetDateFromSQL();

                    CreateProductionTrackingsDto productionTrackingModel = new CreateProductionTrackingsDto
                    {
                        Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                        CurrentAccountCardID = workOrderDataSource.CurrentAccountCardID.GetValueOrDefault(),
                        EmployeeID = AppService.CurrentOperation.EmployeeID,
                        Description_ = string.Empty,
                        IsFinished = true,
                        IsDeleted = false,
                        WorkOrderID = workOrderDataSource.Id,
                        OperationStartDate = workOrderDataSource.OccuredStartDate.GetValueOrDefault(),
                        OperationEndDate = today,
                        OperationStartTime = OperationStartTime.TimeOfDay,
                        StationID = AppService.CurrentOperation.StationID,
                        ProductionOrderID = workOrderDataSource.ProductionOrderID.GetValueOrDefault(),
                        PlannedQuantity = plannedQuantity,
                        ProducedQuantity = producedQuantity,
                        ProductID = AppService.CurrentOperation.ProductID,
                        ProductsOperationID = workOrderDataSource.ProductsOperationID.GetValueOrDefault(),
                        OperationTime = oprTime,
                        HaltReasonID = Guid.Empty,
                        ProductionTrackingTypes = 2,
                        AdjustmentTime = adjTime,
                        HaltTime = TotalHaltReasonTime,
                        FaultyQuantity = scrapQuantity,
                        OperationEndTime = today.TimeOfDay,
                        ShiftID = Guid.Empty,
                    };

                    await ProductionTrackingsAppService.CreateAsync(productionTrackingModel);

                }

                #endregion
            }
            else // Work Order keeps going
            {

                #region ERP DB WorkOrder - Operation Stock Movement - Production Tracking Status Update

                var workOrderDataSource = (await WorkOrdersAppService.GetAsync(currentWorkOrderID)).Data;

                if (workOrderDataSource.Id != Guid.Empty && workOrderDataSource != null)
                {
                    string[] totalOprTime = TotalOperationTime.Split(':');
                    decimal oprTime = (Convert.ToDecimal(totalOprTime[0]) * 3600) + (Convert.ToDecimal(totalOprTime[1]) * 60) + Convert.ToDecimal(totalOprTime[2]);

                    decimal adjTime = Convert.ToDecimal(await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

                    var today = GetSQLDateAppService.GetDateFromSQL();

                    CreateProductionTrackingsDto productionTrackingModel = new CreateProductionTrackingsDto
                    {
                        Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                        CurrentAccountCardID = workOrderDataSource.CurrentAccountCardID.GetValueOrDefault(),
                        EmployeeID = AppService.CurrentOperation.EmployeeID,
                        Description_ = string.Empty,
                        IsFinished = false,
                        IsDeleted = false,
                        WorkOrderID = workOrderDataSource.Id,
                        OperationStartDate = workOrderDataSource.OccuredStartDate.GetValueOrDefault(),
                        OperationEndDate = today,
                        OperationStartTime = OperationStartTime.TimeOfDay,
                        StationID = AppService.CurrentOperation.StationID,
                        ProductionOrderID = workOrderDataSource.ProductionOrderID.GetValueOrDefault(),
                        PlannedQuantity = plannedQuantity,
                        ProducedQuantity = producedQuantity,
                        ProductID = AppService.CurrentOperation.ProductID,
                        ProductsOperationID = workOrderDataSource.ProductsOperationID.GetValueOrDefault(),
                        OperationTime = oprTime,
                        AdjustmentTime = adjTime,
                        ProductionTrackingTypes = 2,
                        HaltReasonID = Guid.Empty,
                        HaltTime = TotalHaltReasonTime,
                        FaultyQuantity = scrapQuantity,
                        OperationEndTime = today.TimeOfDay,
                        ShiftID = Guid.Empty,
                    };

                    await ProductionTrackingsAppService.CreateAsync(productionTrackingModel);

                }

                #endregion
            }

            if (scrapQuantity > 0)
            {
                ScrapQuantityEntryModalVisible = true;
            }

            StopOperationTimer();
            StopScrapTimer();

            TotalOperationTime = "0:0:0";

            ScrapQuantityButtonDisabled = true;
            PauseOperationButtonDisabled = true;
            EndOperationButtonDisabled = true;
            StartOperationButtonDisabled = false;
            IncreaseQuantityDisabled = true;
            ChangeOperationButtonDisabled = false;

            AppService.CurrentOperation.PlannedQuantity = 0;
            AppService.CurrentOperation.ProducedQuantity = 0;
            AppService.CurrentOperation.ScrapQuantity = 0;

            NavigationManager.NavigateTo("/");

            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Scrap Quantity Entry 

        public async void ScrapQuantityEntryOnSubmit()
        {

            #region ERP DB OperationUnsuitabilityReport Create
            Guid stationGroupID = (await StationsAppService.GetAsync(AppService.CurrentOperation.StationID)).Data.GroupID;

            CreateOperationUnsuitabilityReportsDto createOperationUnsuitabilityReportsModel = new CreateOperationUnsuitabilityReportsDto
            {
                Description_ = string.Empty,
                Action_ = string.Empty,
                Date_ = GetSQLDateAppService.GetDateFromSQL(),
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                FicheNo = FicheNumbersAppService.GetFicheNumberAsync("OprUnsRecordsChildMenu"),
                IsUnsuitabilityWorkOrder = false,
                OperationID = AppService.CurrentOperation.ProductsOperationID,
                ProductID = AppService.CurrentOperation.ProductID,
                ProductionOrderID = AppService.CurrentOperation.ProductionOrderID,
                StationID = AppService.CurrentOperation.StationID,
                StationGroupID = stationGroupID,
                UnsuitabilityItemsID = Guid.Empty,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                UnsuitableAmount = scrapQuantityEntry,
            };

            var result = (await OperationUnsuitabilityReportsAppService.CreateAsync(createOperationUnsuitabilityReportsModel)).Data;
            #endregion

            #region Local DB ScrapTable Scrap Quantity 


            ScrapTable scrapTableModel = new ScrapTable
            {
                OperationUnsuitabilityRecordCode = result.FicheNo,
                OperationUnsuitabilityRecordID = result.Id,
                ScrapQuantity = scrapQuantityEntry,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                WorkOrderNo = AppService.CurrentOperation.WorkOrderNo,
                EmployeeName = AppService.CurrentOperation.EmployeeName,
                StationCode = AppService.CurrentOperation.StationCode,
                Action_ = string.Empty,
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                StationID = AppService.CurrentOperation.StationID,
            };

            await ScrapLocalDbService.InsertAsync(scrapTableModel);

            #endregion

            ScrapQuantityCalculate();

            //OperationStopButtonClicked();

            UnsuitabilityQuantityEntriesList = await ScrapLocalDbService.GetListbyEmployeeIDAsync(AppService.CurrentOperation.EmployeeID);

            scrapQuantityEntry = 0;

            await _UnsuitabilityQuantityEntriesGrid.Refresh();

            await InvokeAsync(StateHasChanged);
        }

        private async void ScrapQuantityCalculate()
        {

            decimal unsuitabilityQuantity = (await ScrapLocalDbService.GetListAsync()).Where(t => t.WorkOrderID == AppService.CurrentOperation.WorkOrderID && t.EmployeeID == AppService.CurrentOperation.EmployeeID && t.StationID == AppService.CurrentOperation.StationID).Sum(t => t.ScrapQuantity);

            if (unsuitabilityQuantity > 0)
            {

                if (unsuitabilityQuantity <= AppService.CurrentOperation.ProducedQuantity)
                {

                    QualityPercent = 1 - (unsuitabilityQuantity / AppService.CurrentOperation.ProducedQuantity);

                }
            }
            else if (unsuitabilityQuantity == 0 && AppService.CurrentOperation.ProducedQuantity > 0)
            {
                QualityPercent = 1;
            }


            await InvokeAsync(StateHasChanged);
        }

        #region Scrap Timer

        System.Timers.Timer scrapTimer = new System.Timers.Timer(1000);

        DateTime ScrapStartTime = DateTime.Now;

        DateTime UpdatedScrapStartTime = DateTime.Now;

        void StartScrapTimer()
        {
            ScrapStartTime = DateTime.Now;
            UpdatedScrapStartTime = DateTime.Now;
            scrapTimer = new System.Timers.Timer(10000);
            scrapTimer.Elapsed += ScrapTimerOnTimedEvent;
            scrapTimer.AutoReset = true;
            scrapTimer.Enabled = true;
        }

        private async void ScrapTimerOnTimedEvent(object source, ElapsedEventArgs e)
        {

            #region yorum
            //var operationUnsuitabilityReportList = (await OperationUnsuitabilityReportsAppService.GetListAsync(new ListOperationUnsuitabilityReportsParameterDto())).Data.Where(t => t.WorkOrderID == AppService.CurrentOperation.WorkOrderID && t.StationCode == AppService.CurrentOperation.StationCode).ToList();
            //scrapQuantity = operationUnsuitabilityReportList.Where(t => t.Action_ == "Hurda").Sum(t => t.UnsuitableAmount);

            //#region Local DB CurrentWorkOrder Scrap Quantity Update
            //AppService.CurrentOperation.ScrapQuantity = scrapQuantity;

            //var updatedWorkOrder = await OperationDetailLocalDbService.GetAsync(AppService.CurrentOperation.Id);

            //updatedWorkOrder.ScrapQuantity = scrapQuantity;

            //await OperationDetailLocalDbService.UpdateAsync(updatedWorkOrder);
            //#endregion

            //#region Local DB ScrapTable Scrap Quantity 

            //var operationUnsuitabilitybyEmployeeReportList = operationUnsuitabilityReportList.Where(t => t.EmployeeID == AppService.CurrentOperation.EmployeeID).ToList();

            //var scrapTableList = await ScrapLocalDbService.GetListAsync();

            //foreach (var operationUnsuitabilitybyEmployeeReport in operationUnsuitabilitybyEmployeeReportList)
            //{
            //    if (scrapTableList.Any(t => t.OperationUnsuitabilityRecordID == operationUnsuitabilitybyEmployeeReport.Id))
            //    {
            //        var updatedScrap = scrapTableList.Where(t => t.OperationUnsuitabilityRecordID == operationUnsuitabilitybyEmployeeReport.Id).FirstOrDefault();

            //        updatedScrap.ScrapQuantity = operationUnsuitabilitybyEmployeeReportList.Sum(t => t.UnsuitableAmount);

            //        updatedScrap.Action_ = operationUnsuitabilitybyEmployeeReport.Action_;

            //        await ScrapLocalDbService.UpdateAsync(updatedScrap);
            //    }
            //    else
            //    {
            //        ScrapTable scrapTableModel = new ScrapTable
            //        {
            //            OperationUnsuitabilityRecordCode = operationUnsuitabilitybyEmployeeReport.FicheNo,
            //            OperationUnsuitabilityRecordID = operationUnsuitabilitybyEmployeeReport.Id,
            //            ScrapQuantity = operationUnsuitabilitybyEmployeeReportList.Sum(t => t.UnsuitableAmount),
            //            WorkOrderID = AppService.CurrentOperation.WorkOrderID,
            //            WorkOrderNo = AppService.CurrentOperation.WorkOrderNo,
            //            EmployeeName = AppService.CurrentOperation.EmployeeName,
            //            StationCode = AppService.CurrentOperation.StationCode,
            //            Action_ = string.Empty,
            //            EmployeeID = AppService.CurrentOperation.EmployeeID,
            //            StationID = AppService.CurrentOperation.StationID,
            //        };

            //        await ScrapLocalDbService.InsertAsync(scrapTableModel);
            //    }
            //}
            //#endregion 
            #endregion

            ScrapQuantityCalculate();


        }

        void StopScrapTimer()
        {
            scrapTimer.Stop();
            scrapTimer.Enabled = false;
        }

        public void ScrapTimerDispose()
        {
            if (scrapTimer != null)
            {
                scrapTimer.Stop();
                scrapTimer.Enabled = false;
                scrapTimer.Dispose();
            }
        }

        #endregion

        #endregion

        #region Operation Timer

        public string TotalOperationTime { get; set; } = "0:0:0";

        System.Timers.Timer operationStartTimer = new System.Timers.Timer(1000);

        DateTime OperationStartTime = DateTime.Now;

        DateTime UpdatedOperationStartTime = DateTime.Now;

        void StartOperationTimer()
        {
            OperationStartTime = DateTime.Now;
            UpdatedOperationStartTime = DateTime.Now;
            operationStartTimer = new System.Timers.Timer(1000);
            operationStartTimer.Elapsed += OnTimedEvent;
            operationStartTimer.AutoReset = true;
            operationStartTimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            DateTime currentTime = e.SignalTime;

            TotalOperationTime = currentTime.Subtract(OperationStartTime).Hours + ":" + currentTime.Subtract(OperationStartTime).Minutes + ":" + currentTime.Subtract(OperationStartTime).Seconds;

            if (currentTime.Subtract(UpdatedOperationStartTime).Minutes == 3 && AppService.CurrentOperation.ProducedQuantity == FirstProducedQuantity)
            {
                StopOperationTimer();

                NavigationManager.NavigateTo("/halt-reasons");
            }

            InvokeAsync(StateHasChanged);
        }

        void StopOperationTimer()
        {
            operationStartTimer.Stop();
            operationStartTimer.Enabled = false;
        }

        public void OperationStartTimerDispose()
        {
            if (operationStartTimer != null)
            {
                operationStartTimer.Stop();
                operationStartTimer.Enabled = false;
                operationStartTimer.Dispose();
            }
        }

        #endregion

        #region System Idle Time

        public int TotalSystemIdleTime { get; set; }


        System.Timers.Timer _systemIdleTimer = new System.Timers.Timer(1000);

        DateTime SystemIdleStartTime = DateTime.Now;

        void StartSystemIdleTimer()
        {
            SystemIdleStartTime = DateTime.Now;
            _systemIdleTimer = new System.Timers.Timer(1000);
            _systemIdleTimer.Elapsed += SystemIdleOnTimedEvent;
            _systemIdleTimer.AutoReset = true;
            _systemIdleTimer.Enabled = true;
        }

        private async void SystemIdleOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalSystemIdleTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalSystemIdleTime);

            if (time.Minutes == (((AppService.ProgramParameters.HaltTriggerSecond) / 1000) / 60))
            {

                HaltReasonModalVisible = true;

                #region Sistem Genel Durum Update
                var generalStatus = (await SystemGeneralStatusLocalDbService.GetListAsync()).FirstOrDefault();

                if (generalStatus != null)
                {
                    generalStatus.GeneralStatus = 0;

                    await SystemGeneralStatusLocalDbService.UpdateAsync(generalStatus);
                }
                #endregion

                StartHaltReasonTimer();
                starthaltDate = GetSQLDateAppService.GetDateFromSQL();
                _systemIdleTimer.Stop();
                _systemIdleTimer.Enabled = false;
                await InvokeAsync(StateHasChanged);

            }

            await InvokeAsync(StateHasChanged);
        }

        void StopSystemIdleTimer()
        {
            _systemIdleTimer.Stop();
            _systemIdleTimer.Enabled = false;
        }


        #endregion

        #region Technical Drawings

        public async void OnTechnicalDrawingsButtonClicked()
        {
            TechnicalDrawingsModalVisible = true;

            await InvokeAsync(StateHasChanged);
        }

        public async void OnOperationalTechDrawingButtonClicked()
        {
            var operationalQualityPlanDataSource = (await OperationalQualityPlansAppService.GetbyOperationProductAsync(AppService.CurrentOperation.ProductsOperationID, AppService.CurrentOperation.ProductID)).Data;

            if (operationalQualityPlanDataSource != null && operationalQualityPlanDataSource.Id != Guid.Empty)
            {
                if (operationalQualityPlanDataSource.SelectOperationPictures != null && operationalQualityPlanDataSource.SelectOperationPictures.Count > 0)
                {
                    var lastLine = operationalQualityPlanDataSource.SelectOperationPictures.LastOrDefault();

                    if (lastLine != null && lastLine.Id != Guid.Empty && lastLine.IsApproved)
                    {
                        string filePath = lastLine.DrawingFilePath;
                        string fileName = lastLine.UploadedFileName;
                        string format = fileName.Split('.')[1];

                        string fileBrowserPath = ConfigurationRoot.GetSection("FileBrowserPath")["FilePath"].ToString();

                        if (!string.IsNullOrEmpty(fileBrowserPath))
                        {
                            DirectoryInfo operationPicture = new DirectoryInfo(fileBrowserPath + filePath);

                            if (operationPicture.Exists)
                            {
                                FileInfo[] exactFilesOperationPicture = operationPicture.GetFiles();

                                if (exactFilesOperationPicture.Length > 0)
                                {
                                    foreach (FileInfo fileinfo in exactFilesOperationPicture)
                                    {
                                        uploadedfiles.Add(fileinfo);
                                    }
                                }
                            }

                            if (format == "jpg" || format == "jpeg" || format == "png")
                            {
                                imageDataUri = uploadedfiles[0].FullName;

                                image = true;

                                pdf = false;

                                ImagePreviewPopup = true;
                            }

                            else if (format == "pdf")
                            {

                                PDFrootPath = uploadedfiles[0].FullName;

                                PDFFileName = fileName;

                                previewImagePopupTitle = fileName;

                                pdf = true;

                                image = false;

                                ImagePreviewPopup = true;

                            }
                        }


                    }


                }
            }

            await InvokeAsync(StateHasChanged);
        }

        public async void OnTechDrawingButtonClicked()
        {
            var productionOrder = (await ProductionOrdersAppService.GetAsync(AppService.CurrentOperation.ProductionOrderID)).Data;

            if (productionOrder != null && productionOrder.Id != Guid.Empty)
            {
                var technicalDrawingDataSource = (await TechnicalDrawingsAppService.GetSelectListAsync(productionOrder.FinishedProductID.GetValueOrDefault())).Data.Where(t => t.CustomerApproval && t.IsApproved && t.SampleApproval).FirstOrDefault();

                if (technicalDrawingDataSource != null && technicalDrawingDataSource.Id != Guid.Empty)
                {
                    string filePath = technicalDrawingDataSource.DrawingFilePath;
                    string fileName = technicalDrawingDataSource.UploadedFileName;
                    string format = fileName.Split('.')[1];

                    string fileBrowserPath = ConfigurationRoot.GetSection("FileBrowserPath")["FilePath"].ToString();

                    if (!string.IsNullOrEmpty(fileBrowserPath))
                    {
                        DirectoryInfo technicalDrawing = new DirectoryInfo(fileBrowserPath + filePath);

                        if (technicalDrawing.Exists)
                        {
                            FileInfo[] exactFilesTechnicalDrawing = technicalDrawing.GetFiles();

                            if (exactFilesTechnicalDrawing.Length > 0)
                            {
                                foreach (FileInfo fileinfo in exactFilesTechnicalDrawing)
                                {
                                    uploadedfiles.Add(fileinfo);
                                }
                            }
                        }

                        if (format == "jpg" || format == "jpeg" || format == "png")
                        {
                            imageDataUri = uploadedfiles[0].FullName;

                            image = true;

                            pdf = false;

                            ImagePreviewPopup = true;
                        }

                        else if (format == "pdf")
                        {

                            PDFrootPath = uploadedfiles[0].FullName;

                            PDFFileName = fileName;

                            previewImagePopupTitle = fileName;

                            pdf = true;

                            image = false;

                            ImagePreviewPopup = true;

                        }
                    }


                }
            }

            await InvokeAsync(StateHasChanged);
        }

        public void HidePreviewPopup()
        {
            ImagePreviewPopup = false;
        }

        #endregion

        #region Halt Reasons

        public bool HaltReasonModalVisible { get; set; }

        public bool EndHaltReasonButtonDisable { get; set; } = true;

        List<ListHaltReasonsDto> HaltReasonsList = new List<ListHaltReasonsDto>();

        ListHaltReasonsDto SelectedHaltReason = new ListHaltReasonsDto();

        private async Task GetHaltReasonsIsOperator()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsOperator == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsMachine()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsMachine == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsManagement()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsManagement == true && t.IsIncidentalHalt == false).ToList();

            await Task.CompletedTask;
        }

        private async Task GetHaltReasonsIsIncidental()
        {
            HaltReasonsList = (await HaltReasonsService.GetListAsync(new ListHaltReasonsParameterDto())).Data.Where(t => t.IsIncidentalHalt == true).ToList();

            await Task.CompletedTask;
        }

        private async void OnSelectHaltReason(ListHaltReasonsDto haltReason)
        {
            if (!string.IsNullOrEmpty(haltReason.Name))
            {
                SelectedHaltReason = haltReason;
                EndHaltReasonButtonDisable = false;
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                EndHaltReasonButtonDisable = true;
                await InvokeAsync(() => StateHasChanged());
            }
        }

        private void OnSelectIncidentalHaltReason(ListHaltReasonsDto haltReason)
        {
            isPasswordVisible = true;
            password = string.Empty;
            haltReasonIncidental = haltReason;
        }

        #region Halt Reason Timer

        public int TotalHaltReasonTime { get; set; }

        public string HaltReasonTime { get; set; } = "0:0:0";

        System.Timers.Timer _haltReasonTimer = new System.Timers.Timer(1000);

        DateTime HaltReasonStartTime = DateTime.Now;

        void StartHaltReasonTimer()
        {
            HaltReasonStartTime = DateTime.Now;
            _haltReasonTimer = new System.Timers.Timer(1000);
            _haltReasonTimer.Elapsed += HaltReasonOnTimedEvent;
            _haltReasonTimer.AutoReset = true;
            _haltReasonTimer.Enabled = true;
        }

        private void HaltReasonOnTimedEvent(object source, ElapsedEventArgs e)
        {
            TotalHaltReasonTime++;

            TimeSpan time = TimeSpan.FromSeconds(TotalHaltReasonTime);

            HaltReasonTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 time.Hours,
                 time.Minutes,
                 time.Seconds);

            InvokeAsync(StateHasChanged);
        }

        #endregion

        private async void EndHaltReasonButtonClick()
        {
            _haltReasonTimer.Enabled = false;
            _haltReasonTimer.Stop();
            StartSystemIdleTimer();

            var today = GetSQLDateAppService.GetDateFromSQL();

            #region Local Operation Halt Reason Table Insert
            OperationHaltReasonsTable haltReasonModel = new OperationHaltReasonsTable
            {
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                EmployeeName = AppService.CurrentOperation.EmployeeName,
                EndHaltDate = today,
                HaltReasonID = SelectedHaltReason.Id,
                HaltReasonName = SelectedHaltReason.Name,
                StartHaltDate = starthaltDate,
                StationID = AppService.CurrentOperation.StationID,
                StationCode = AppService.CurrentOperation.StationCode,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                WorkOrderNo = AppService.CurrentOperation.WorkOrderNo,
                TotalHaltReasonTime = TotalHaltReasonTime
            };

            await OperationHaltReasonsTableLocalDbService.InsertAsync(haltReasonModel);
            #endregion

            #region ERP Production Tracking Insert

            var workOrder = (await WorkOrdersAppService.GetAsync(AppService.CurrentOperation.WorkOrderID)).Data;

            Guid CurrentAccountID = Guid.Empty;

            if (workOrder != null && workOrder.Id != Guid.Empty)
            {
                CurrentAccountID = workOrder.CurrentAccountCardID.GetValueOrDefault();
            }

            CreateProductionTrackingsDto trackingModel = new CreateProductionTrackingsDto
            {
                AdjustmentTime = 0,
                Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                CurrentAccountCardID = CurrentAccountID,
                HaltReasonID = SelectedHaltReason.Id,
                EmployeeID = AppService.CurrentOperation.EmployeeID,
                Description_ = string.Empty,
                HaltTime = TotalHaltReasonTime,
                FaultyQuantity = AppService.CurrentOperation.ScrapQuantity,
                IsFinished = true,
                OperationEndDate = today.Date,
                OperationEndTime = today.TimeOfDay,
                OperationStartDate = starthaltDate.Date,
                OperationStartTime = starthaltDate.TimeOfDay,
                OperationTime = 0,
                PlannedQuantity = AppService.CurrentOperation.PlannedQuantity,
                ProducedQuantity = AppService.CurrentOperation.ProducedQuantity,
                ProductID = AppService.CurrentOperation.ProductID,
                ProductionTrackingTypes = 0,
                ProductionOrderID = AppService.CurrentOperation.ProductionOrderID,
                ProductsOperationID = AppService.CurrentOperation.ProductsOperationID,
                ShiftID = Guid.Empty,
                WorkOrderID = AppService.CurrentOperation.WorkOrderID,
                StationID = AppService.CurrentOperation.StationID,

            };

            await ProductionTrackingsAppService.CreateAsync(trackingModel);

            #endregion

            TotalHaltReasonTime = 0;

            HaltReasonTime = string.Format("{0:D2}:{1:D2}:{2:D2}",
                 0,
                 0,
                 0);
            SelectedHaltReason = new ListHaltReasonsDto();
            TotalSystemIdleTime = 0;


            HaltReasonModalVisible = false;

        }

        public async void OnPasswordSubmit()
        {
            if (!string.IsNullOrEmpty(haltReasonIncidental.Name))
            {
                SelectedHaltReason = haltReasonIncidental;
                EndHaltReasonButtonDisable = false;
                await InvokeAsync(() => StateHasChanged());
            }
            else
            {
                EndHaltReasonButtonDisable = true;
                await InvokeAsync(() => StateHasChanged());
            }

            HidePasswordModal();
        }

        public void HidePasswordModal()
        {
            isPasswordVisible = false;
            password = string.Empty;
            haltReasonIncidental = new ListHaltReasonsDto();
        }

        #endregion

        #region Start Adjustment

        public void StartAdjustment()
        {
            AppService.AdjustmentState = Utilities.EnumUtilities.States.AdjustmentState.FromOperation;
            NavigationManager.NavigateTo("/operation-adjustment");
        }

        #endregion

        #region Change Employee

        public async void OperatorChangeButtonClicked()
        {
            var res = await ModalManager.ConfirmationAsync("Onay", "Operatör değiştirmek istediğinizden emin misiniz?");

            if (res)
            {
                #region End Operation Codes

                var plannedQuantity = AppService.CurrentOperation.PlannedQuantity;
                var producedQuantity = AppService.CurrentOperation.ProducedQuantity;
                var scrapQuantity = AppService.CurrentOperation.ScrapQuantity;
                var currentWorkOrderID = AppService.CurrentOperation.WorkOrderID;

                if (plannedQuantity <= producedQuantity + scrapQuantity) // Work Order has completed
                {

                    #region Current Operation Delete

                    var currentOperationList = (await OperationDetailLocalDbService.GetListAsync()).ToList();

                    if (currentOperationList.Count > 0 && currentOperationList != null)
                    {
                        await OperationDetailLocalDbService.DeleteAsync(currentOperationList[0]);
                    }

                    #endregion

                    #region Unsuitable Table Delete

                    var scraplist = (await ScrapLocalDbService.GetListAsync()).ToList();

                    if (scraplist.Count > 0 && scraplist != null)
                    {
                        foreach (var item in scraplist)
                        {
                            await ScrapLocalDbService.DeleteAsync(item);
                        }
                    }

                    #endregion

                    #region Adjustment Table Delete

                    var adjustmentList = (await OperationAdjustmentLocalDbService.GetListAsync()).ToList();

                    if (adjustmentList.Count > 0 && adjustmentList != null)
                    {
                        foreach (var item in adjustmentList)
                        {
                            await OperationAdjustmentLocalDbService.DeleteAsync(item);
                        }
                    }

                    #endregion

                    #region Halt Reason Table Delete

                    var haltList = (await OperationHaltReasonsTableLocalDbService.GetListAsync()).ToList();

                    if (haltList.Count > 0 && haltList != null)
                    {
                        foreach (var item in haltList)
                        {
                            await OperationHaltReasonsTableLocalDbService.DeleteAsync(item);
                        }
                    }

                    #endregion

                    #region ERP DB WorkOrder - Operation Stock Movement - Production Tracking Status Update

                    var workOrderDataSource = (await WorkOrdersAppService.GetAsync(currentWorkOrderID)).Data;

                    if (workOrderDataSource.Id != Guid.Empty && workOrderDataSource != null)
                    {
                        string[] totalOprTime = TotalOperationTime.Split(':');
                        decimal oprTime = (Convert.ToDecimal(totalOprTime[0]) * 3600) + (Convert.ToDecimal(totalOprTime[1]) * 60) + Convert.ToDecimal(totalOprTime[2]);

                        decimal adjTime = Convert.ToDecimal(await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

                        CreateProductionTrackingsDto productionTrackingModel = new CreateProductionTrackingsDto
                        {
                            Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                            CurrentAccountCardID = workOrderDataSource.CurrentAccountCardID.GetValueOrDefault(),
                            EmployeeID = AppService.CurrentOperation.EmployeeID,
                            Description_ = string.Empty,
                            IsFinished = true,
                            IsDeleted = false,
                            WorkOrderID = workOrderDataSource.Id,
                            OperationStartDate = workOrderDataSource.OccuredStartDate.GetValueOrDefault(),
                            OperationEndDate = GetSQLDateAppService.GetDateFromSQL(),
                            OperationStartTime = OperationStartTime.TimeOfDay,
                            StationID = AppService.CurrentOperation.StationID,
                            ProductionOrderID = workOrderDataSource.ProductionOrderID.GetValueOrDefault(),
                            PlannedQuantity = plannedQuantity,
                            ProducedQuantity = producedQuantity,
                            ProductID = AppService.CurrentOperation.ProductID,
                            ProductsOperationID = workOrderDataSource.ProductsOperationID.GetValueOrDefault(),
                            OperationTime = oprTime,
                            AdjustmentTime = adjTime,
                            HaltTime = TotalHaltReasonTime,
                            FaultyQuantity = scrapQuantity,
                            OperationEndTime = GetSQLDateAppService.GetDateFromSQL().TimeOfDay,
                            ShiftID = Guid.Empty,
                        };

                        await ProductionTrackingsAppService.CreateAsync(productionTrackingModel);

                    }

                    #endregion
                }
                else // Work Order keeps going
                {

                    #region ERP DB WorkOrder - Operation Stock Movement - Production Tracking Status Update

                    var workOrderDataSource = (await WorkOrdersAppService.GetAsync(currentWorkOrderID)).Data;

                    if (workOrderDataSource.Id != Guid.Empty && workOrderDataSource != null)
                    {
                        string[] totalOprTime = TotalOperationTime.Split(':');
                        decimal oprTime = (Convert.ToDecimal(totalOprTime[0]) * 3600) + (Convert.ToDecimal(totalOprTime[1]) * 60) + Convert.ToDecimal(totalOprTime[2]);

                        decimal adjTime = Convert.ToDecimal(await OperationAdjustmentAppService.GetTotalAdjustmentTimeAsync(AppService.CurrentOperation.WorkOrderID));

                        CreateProductionTrackingsDto productionTrackingModel = new CreateProductionTrackingsDto
                        {
                            Code = FicheNumbersAppService.GetFicheNumberAsync("ProdTrackingsChildMenu"),
                            CurrentAccountCardID = workOrderDataSource.CurrentAccountCardID.GetValueOrDefault(),
                            EmployeeID = AppService.CurrentOperation.EmployeeID,
                            Description_ = string.Empty,
                            IsFinished = false,
                            IsDeleted = false,
                            WorkOrderID = workOrderDataSource.Id,
                            OperationStartDate = workOrderDataSource.OccuredStartDate.GetValueOrDefault(),
                            OperationEndDate = GetSQLDateAppService.GetDateFromSQL(),
                            OperationStartTime = OperationStartTime.TimeOfDay,
                            StationID = AppService.CurrentOperation.StationID,
                            ProductionOrderID = workOrderDataSource.ProductionOrderID.GetValueOrDefault(),
                            PlannedQuantity = plannedQuantity,
                            ProducedQuantity = producedQuantity,
                            ProductID = AppService.CurrentOperation.ProductID,
                            ProductsOperationID = workOrderDataSource.ProductsOperationID.GetValueOrDefault(),
                            OperationTime = oprTime,
                            AdjustmentTime = adjTime,
                            HaltTime = TotalHaltReasonTime,
                            FaultyQuantity = scrapQuantity,
                            OperationEndTime = GetSQLDateAppService.GetDateFromSQL().TimeOfDay,
                            ShiftID = Guid.Empty,
                        };

                        await ProductionTrackingsAppService.CreateAsync(productionTrackingModel);

                    }

                    #endregion
                }

                if (scrapQuantity > 0)
                {
                    ScrapQuantityEntryModalVisible = true;
                }

                StopOperationTimer();
                StopScrapTimer();

                TotalOperationTime = "0:0:0";

                ScrapQuantityButtonDisabled = true;
                PauseOperationButtonDisabled = true;
                EndOperationButtonDisabled = true;
                StartOperationButtonDisabled = false;
                IncreaseQuantityDisabled = true;
                ChangeOperationButtonDisabled = false;

                AppService.CurrentOperation.PlannedQuantity = 0;
                AppService.CurrentOperation.ProducedQuantity = 0;
                AppService.CurrentOperation.ScrapQuantity = 0;

                #endregion

                NavigationManager.NavigateTo("/");
            }

            await InvokeAsync(StateHasChanged);
        }


        #endregion

        #region Unsuitability Quantity Entries 

        public async void UnsuitabilityQuantityEntriesButtonClicked()
        {
            UnsuitabilityQuantityEntriesList = await ScrapLocalDbService.GetListbyEmployeeIDAsync(AppService.CurrentOperation.EmployeeID);

            scrapQuantityEntry = 0;

            UnsuitabilityQuantityEntryModalVisible = true;
        }

        public void HideUnsuitabilityQuantityEntriesModal()
        {
            UnsuitabilityQuantityEntryModalVisible = false;

            scrapQuantityEntry = 0;
        }

        #endregion

        #region Change Crate

        #region Kasa Değiştirme Değişkenleri

        public bool ChangeCratePopupVisible = false;

        public bool ApproveQuantityinCrateDisable = false;

        public bool TareDisable = false;

        public bool CloseCrateDisable = false;

        #endregion

        public async void ChangeCrateButtonClicked()
        {
            ChangeCratePopupVisible = true;

            ApproveQuantityinCrateDisable = false;

            TareDisable = true;

            CloseCrateDisable = true;

            await InvokeAsync(StateHasChanged);
        }

        public async void OnApproveQuantityinCrateButtonClicked()
        {
            var res = await ModalManager.ConfirmationAsync("Onay", "Kasadaki adedi onaylamak istediğinizden emin misiniz?");

            if (res)
            {
                ApproveQuantityinCrateDisable = true;
                TareDisable = false;
            }
        }

        public async void OnTareButtonClicked()
        {
            TareDisable = true;

            CloseCrateDisable = false;

            await InvokeAsync(StateHasChanged);
        }

        public async void OnCloseCrateButtonClicked()
        {
            HideChangeCrateModal();

            await InvokeAsync(StateHasChanged);
        }

        public void HideChangeCrateModal()
        {
            TareDisable = true;

            CloseCrateDisable = true;

            ChangeCratePopupVisible = false;
        }

        #endregion

        public void Dispose()
        {
            if (operationStartTimer != null)
            {
                operationStartTimer.Stop();
                operationStartTimer.Enabled = false;
                //operationStartTimer.Dispose();
            }
            if (_systemIdleTimer != null)
            {
                _systemIdleTimer.Stop();
                _systemIdleTimer.Enabled = false;
                //_systemIdleTimer.Dispose();
            }
        }
    }
}
