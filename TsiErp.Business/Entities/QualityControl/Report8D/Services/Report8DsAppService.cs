using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TSI.QueryBuilder.Models;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Entities.Report8D.Validations;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.FinanceManagement.CurrentAccountCard;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.QualityControl.Report8D;
using TsiErp.Entities.Entities.QualityControl.Report8D.Dtos;
using TsiErp.Entities.Entities.StockManagement.Product;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Report8Ds.Page;

namespace TsiErp.Business.Entities.Report8D.Services
{
    [ServiceRegistration(typeof(IReport8DsAppService), DependencyInjectionType.Scoped)]
    public class Report8DsAppService : ApplicationService<Report8DsResource>, IReport8DsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public Report8DsAppService(IStringLocalizer<Report8DsResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }




        [ValidationAspect(typeof(CreateReport8DsValidator), Priority = 1)]
        public async Task<IDataResult<SelectReport8DsDto>> CreateAsync(CreateReport8DsDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Report8Ds).Select("Code").Where(new { Code = input.Code },  "");

            var list = queryFactory.ControlList<Report8Ds>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();
            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Report8Ds).Insert(new CreateReport8DsDto
            {
                Code = input.Code,
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                SupplierID = input.SupplierID.GetValueOrDefault(),
                CustomerID = input.CustomerID.GetValueOrDefault(),
                AtCustomerBlocked = input.AtCustomerBlocked,
                AtCustomerChecked = input.AtCustomerChecked,
                AtCustomerDefect = input.AtCustomerDefect,
                AtSupplierBlocked = input.AtSupplierBlocked,
                AtSupplierChecked = input.AtSupplierChecked,
                AtSupplierDefect = input.AtSupplierDefect,
                PartNumber = input.PartNumber,
                State_ = input.State_,
                CA1ContainmentAction = input.CA1ContainmentAction,
                CA1ContainmentActionD6 = input.CA1ContainmentActionD6,
                CA1ImplementationDate = input.CA1ImplementationDate,
                CA1PotentialRisk = input.CA1PotentialRisk,
                CA1RemovalDateD6 = input.CA1RemovalDateD6,
                CA1Responsible = input.CA1Responsible,
                CA1ResponsibleD6 = input.CA1ResponsibleD6,
                CA2ContainmentAction = input.CA2ContainmentAction,
                CA2ContainmentActionD6 = input.CA2ContainmentActionD6,
                CA2ImplementationDate = input.CA2ImplementationDate,
                CA2PotentialRisk = input.CA2PotentialRisk,
                CA2RemovalDateD6 = input.CA2RemovalDateD6,
                CA2Responsible = input.CA2Responsible,
                CA2ResponsibleD6 = input.CA2ResponsibleD6,
                CA3ContainmentAction = input.CA3ContainmentAction,
                CA3ContainmentActionD6 = input.CA3ContainmentActionD6,
                CA3ImplementationDate = input.CA3ImplementationDate,
                CA3PotentialRisk = input.CA3PotentialRisk,
                CA3RemovalDateD6 = input.CA3RemovalDateD6,
                CA3Responsible = input.CA3Responsible,
                CA3ResponsibleD6 = input.CA3ResponsibleD6,
                ClaimedQuantity = input.ClaimedQuantity,
                ClaimingPlants = input.ClaimingPlants,
                ClaimOpeningDate = input.ClaimOpeningDate,
                ComplaintJustified = input.ComplaintJustified,
                ContainmentActionDate = input.ContainmentActionDate,
                ControlPlanRevisionCompletionDate = input.ControlPlanRevisionCompletionDate,
                ControlPlanRevisionDocumentNumber = input.ControlPlanRevisionDocumentNumber,
                ControlPlanRevisionProofAttached = input.ControlPlanRevisionProofAttached,
                ControlPlanRevisionRelevant = input.ControlPlanRevisionRelevant,
                ControlPlanRevisionResponsible = input.ControlPlanRevisionResponsible,
                ControlPlanRevisionVersion = input.ControlPlanRevisionVersion,
                Customer8DClosureDate = input.Customer8DClosureDate,
                Customer8DClosureFunctionDepartment = input.Customer8DClosureFunctionDepartment,
                Customer8DClosureName = input.Customer8DClosureName,
                DateFinalRelease = input.DateFinalRelease,
                DateInterimReportD3 = input.DateInterimReportD3,
                DateInterimReportD5 = input.DateInterimReportD5,
                DeliveredQuantity = input.DeliveredQuantity,
                DeviationsProblems = input.DeviationsProblems,
                DeviationsSymptoms = input.DeviationsSymptoms,
                DFMEARevisionCompletionDate = input.DFMEARevisionCompletionDate,
                DFMEARevisionDocumentNumber = input.DFMEARevisionDocumentNumber,
                DFMEARevisionProofAttached = input.DFMEARevisionProofAttached,
                DFMEARevisionRelevant = input.DFMEARevisionRelevant,
                DFMEARevisionResponsible = input.DFMEARevisionResponsible,
                DFMEARevisionVersion = input.DFMEARevisionVersion,
                DrawingIndex = input.DrawingIndex,
                FailureOccurance = input.FailureOccurance,
                IA1CorrectiveAction = input.IA1CorrectiveAction,
                IA1EffectiveFromDate = input.IA1EffectiveFromDate,
                IA1ImplementationDate = input.IA1ImplementationDate,
                IA1ProofAttached = input.IA1ProofAttached,
                IA1RootCause = input.IA1RootCause,
                IA1ValidatedDate = input.IA1ValidatedDate,
                IA2CorrectiveAction = input.IA2CorrectiveAction,
                IA2EffectiveFromDate = input.IA2EffectiveFromDate,
                IA2ImplementationDate = input.IA2ImplementationDate,
                IA2ProofAttached = input.IA2ProofAttached,
                IA2RootCause = input.IA2RootCause,
                IA2ValidatedDate = input.IA2ValidatedDate,
                IA3CorrectiveAction = input.IA3CorrectiveAction,
                IA3EffectiveFromDate = input.IA3EffectiveFromDate,
                IA3ImplementationDate = input.IA3ImplementationDate,
                IA3ProofAttached = input.IA3ProofAttached,
                IA3RootCause = input.IA3RootCause,
                IA3ValidatedDate = input.IA3ValidatedDate,
                IA4CorrectiveAction = input.IA4CorrectiveAction,
                IA4EffectiveFromDate = input.IA4EffectiveFromDate,
                IA4ImplementationDate = input.IA4ImplementationDate,
                IA4ProofAttached = input.IA4ProofAttached,
                IA4RootCause = input.IA4RootCause,
                IA4ValidatedDate = input.IA4ValidatedDate,
                IA5CorrectiveAction = input.IA5CorrectiveAction,
                IA5EffectiveFromDate = input.IA5EffectiveFromDate,
                IA5ImplementationDate = input.IA5ImplementationDate,
                IA5ProofAttached = input.IA5ProofAttached,
                IA5RootCause = input.IA5RootCause,
                IA5ValidatedDate = input.IA5ValidatedDate,
                InTransitBlocked = input.InTransitBlocked,
                InTransitChecked = input.InTransitChecked,
                InTransitDefect = input.InTransitDefect,
                LessonsLearnedDate = input.LessonsLearnedDate,
                LessonsLearnedFunctionDepartment = input.LessonsLearnedFunctionDepartment,
                LessonsLearnedProofAttached = input.LessonsLearnedProofAttached,
                LessonsLearnedRelevant = input.LessonsLearnedRelevant,
                LessonsLearnedResponsible = input.LessonsLearnedResponsible,
                OtherAffectedPlants = input.OtherAffectedPlants,
                PA1PlannedImplementationDate = input.PA1PlannedImplementationDate,
                PA1PotentialCorrectiveAction = input.PA1PotentialCorrectiveAction,
                PA1Responsible = input.PA1Responsible,
                PA1RootCause = input.PA1RootCause,
                PA1ToBeImplemented = input.PA1ToBeImplemented,
                PA2PlannedImplementationDate = input.PA2PlannedImplementationDate,
                PA2PotentialCorrectiveAction = input.PA2PotentialCorrectiveAction,
                PA2Responsible = input.PA2Responsible,
                PA2RootCause = input.PA2RootCause,
                PA2ToBeImplemented = input.PA2ToBeImplemented,
                PA3PlannedImplementationDate = input.PA3PlannedImplementationDate,
                PA3PotentialCorrectiveAction = input.PA3PotentialCorrectiveAction,
                PA3Responsible = input.PA3Responsible,
                PA3RootCause = input.PA3RootCause,
                PA3ToBeImplemented = input.PA3ToBeImplemented,
                PA4PlannedImplementationDate = input.PA4PlannedImplementationDate,
                PA4PotentialCorrectiveAction = input.PA4PotentialCorrectiveAction,
                PA4Responsible = input.PA4Responsible,
                PA4RootCause = input.PA4RootCause,
                PA4ToBeImplemented = input.PA4ToBeImplemented,
                PA5PlannedImplementationDate = input.PA5PlannedImplementationDate,
                PA5PotentialCorrectiveAction = input.PA5PotentialCorrectiveAction,
                PA5Responsible = input.PA5Responsible,
                PA5RootCause = input.PA5RootCause,
                PA5ToBeImplemented = input.PA5ToBeImplemented,
                PFMEARevisionCompletionDate = input.PFMEARevisionCompletionDate,
                PFMEARevisionDocumentNumber = input.PFMEARevisionDocumentNumber,
                PFMEARevisionProofAttached = input.PFMEARevisionProofAttached,
                PFMEARevisionRelevant = input.PFMEARevisionRelevant,
                PFMEARevisionResponsible = input.PFMEARevisionResponsible,
                PFMEARevisionVersion = input.PFMEARevisionVersion,
                ProductionPlant = input.ProductionPlant,
                Report8DAccepted = input.Report8DAccepted,
                Report8DRevision = input.Report8DRevision,
                Revision1Action = input.Revision1Action,
                Revision1CompletionDate = input.Revision1CompletionDate,
                Revision1DocumentNumber = input.Revision1DocumentNumber,
                Revision1ProofAttached = input.Revision1ProofAttached,
                Revision1Relevant = input.Revision1Relevant,
                Revision1Responsible = input.Revision1Responsible,
                Revision1Version = input.Revision1Version,
                Revision2Action = input.Revision2Action,
                Revision2CompletionDate = input.Revision2CompletionDate,
                Revision2DocumentNumber = input.Revision2DocumentNumber,
                Revision2ProofAttached = input.Revision2ProofAttached,
                Revision2Relevant = input.Revision2Relevant,
                Revision2Responsible = input.Revision2Responsible,
                Revision2Version = input.Revision2Version,
                Revision3Action = input.Revision3Action,
                Revision3CompletionDate = input.Revision3CompletionDate,
                Revision3DocumentNumber = input.Revision3DocumentNumber,
                Revision3ProofAttached = input.Revision3ProofAttached,
                Revision3Relevant = input.Revision3Relevant,
                Revision3Responsible = input.Revision3Responsible,
                Revision3Version = input.Revision3Version,
                RN1AnalysisMethod = input.RN1AnalysisMethod,
                RN1NonDetectionReason = input.RN1NonDetectionReason,
                RN1Share = input.RN1Share,
                RN2AnalysisMethod = input.RN2AnalysisMethod,
                RN2NonDetectionReason = input.RN2NonDetectionReason,
                RN2Share = input.RN2Share,
                RO1AnalysisMethod = input.RO1AnalysisMethod,
                RO1OccuranceReason = input.RO1OccuranceReason,
                RO1Share = input.RO1Share,
                RO2AnalysisMethod = input.RO2AnalysisMethod,
                RO2OccuranceReason = input.RO2OccuranceReason,
                RO2Share = input.RO2Share,
                Sponsor = input.Sponsor,
                SponsorD8 = input.SponsorD8,
                SponsorDateD8 = input.SponsorDateD8,
                SponsorEMail = input.SponsorEMail,
                SponsorFunctionDepartment = input.SponsorFunctionDepartment,
                SponsorPhone = input.SponsorPhone,
                TeamLeader = input.TeamLeader,
                TeamLeaderD8 = input.TeamLeaderD8,
                TeamLeaderDateD8 = input.TeamLeaderDateD8,
                TeamLeaderEMail = input.TeamLeaderEMail,
                TeamLeaderFunctionDepartment = input.TeamLeaderFunctionDepartment,
                TeamLeaderPhone = input.TeamLeaderPhone,
                TeamMember1 = input.TeamMember1,
                TeamMember1EMail = input.TeamMember1EMail,
                TeamMember1FunctionDepartment = input.TeamMember1FunctionDepartment,
                TeamMember1Phone = input.TeamMember1Phone,
                TeamMember2 = input.TeamMember2,
                TeamMember2EMail = input.TeamMember2EMail,
                TeamMember2FunctionDepartment = input.TeamMember2FunctionDepartment,
                TeamMember2Phone = input.TeamMember2Phone,
                TeamMember3 = input.TeamMember3,
                TeamMember3EMail = input.TeamMember3EMail,
                TeamMember3FunctionDepartment = input.TeamMember3FunctionDepartment,
                TeamMember3Phone = input.TeamMember3Phone,
                TopicTitle = input.TopicTitle,
                UpdateRequiredUntilDate = input.UpdateRequiredUntilDate,

                CreationTime = now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
            });

            var Report8Ds = queryFactory.Insert<SelectReport8DsDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("Report8DChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Report8Ds, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["Report8DChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                             
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectReport8DsDto>(Report8Ds);
        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var entity = (await GetAsync(id)).Data;
           
                var query = queryFactory.Query().From(Tables.Report8Ds).Delete(LoginedUserService.UserId).Where(new { Id = id },  "");

                var Report8Ds = queryFactory.Update<SelectReport8DsDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Report8Ds, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["Report8DChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains("*Not*"))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                     
                                    ModuleName_ = notTemplate.ModuleName_,
                                    ProcessName_ = notTemplate.ProcessName_,
                                    RecordNumber = entity.Code,
                                    NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                    UserId = new Guid(user),
                                    ViewDate = null,
                                };

                                await _NotificationsAppService.CreateAsync(createInput);
                            }
                        }
                        else
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = entity.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(notTemplate.TargetUsersId),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }

                }

                #endregion

                await Task.CompletedTask;
                return new SuccessDataResult<SelectReport8DsDto>(Report8Ds);
            
        }

        public async Task<IDataResult<SelectReport8DsDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.Report8Ds).Select<Report8Ds>(null)
                        .Join<CurrentAccountCards>
                        (
                            e => new { SupplierID = e.Id, SupplierName = e.Name, SupplierCode = e.Code, SupplierNo = e.SupplierNo },
                            nameof(Report8Ds.SupplierID),
                            nameof(CurrentAccountCards.Id),
                            "Supplier",
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            cac => new { CustomerName = cac.Name, CustomerCode = cac.Code, CustomerID = cac.Id },
                            nameof(Report8Ds.CustomerID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(Report8Ds.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<TechnicalDrawings>
                        (
                            p => new { TechnicalDrawingID = p.Id, TechnicalDrawingNo = p.DrawingNo },
                            nameof(Report8Ds.TechnicalDrawingID),
                            nameof(TechnicalDrawings.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.Report8Ds);

            var Report8D = queryFactory.Get<SelectReport8DsDto>(query);

            LogsAppService.InsertLogToDatabase(Report8D, Report8D, LoginedUserService.UserId, Tables.Report8Ds, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectReport8DsDto>(Report8D);

        }

        public async Task<IDataResult<IList<ListReport8DsDto>>> GetListAsync(ListReport8DsParameterDto input)
        {
            var query = queryFactory
                    .Query()
                    .From(Tables.Report8Ds)
                    .Select<Report8Ds>(s => new { s.Code, s.ClaimOpeningDate, s.State_, s.Id })
                        .Join<CurrentAccountCards>
                        (
                            e => new { SupplierID = e.Id, SupplierName = e.Name, SupplierCode = e.Code, SupplierNo = e.SupplierNo },
                            nameof(Report8Ds.SupplierID),
                            nameof(CurrentAccountCards.Id),
                            "Supplier",
                            JoinType.Left
                        )
                          .Join<CurrentAccountCards>
                        (
                            cac => new { CustomerName = cac.Name, CustomerCode = cac.Code, CustomerID = cac.Id },
                            nameof(Report8Ds.CustomerID),
                            nameof(CurrentAccountCards.Id),
                            JoinType.Left
                        )
                         .Join<Products>
                        (
                            p => new { ProductID = p.Id, ProductCode = p.Code, ProductName = p.Name },
                            nameof(Report8Ds.ProductID),
                            nameof(Products.Id),
                            JoinType.Left
                        )
                          .Join<TechnicalDrawings>
                        (
                            p => new { TechnicalDrawingID = p.Id, TechnicalDrawingNo = p.DrawingNo },
                            nameof(Report8Ds.TechnicalDrawingID),
                            nameof(TechnicalDrawings.Id),
                            JoinType.Left
                        ).Where(null,  Tables.Report8Ds);


            var Report8D = queryFactory.GetList<ListReport8DsDto>(query).ToList();
            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListReport8DsDto>>(Report8D);
        }

        [ValidationAspect(typeof(UpdateReport8DsValidator), Priority = 1)]
        public async Task<IDataResult<SelectReport8DsDto>> UpdateAsync(UpdateReport8DsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Report8Ds).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<Report8Ds>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Report8Ds).Select("*").Where(new { Code = input.Code },  "");
            var list = queryFactory.GetList<Report8Ds>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Report8Ds).Update(new UpdateReport8DsDto
            {
                Code = input.Code,
                State_ = input.State_,
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                SupplierID = input.SupplierID.GetValueOrDefault(),
                CustomerID = input.CustomerID.GetValueOrDefault(),
                AtCustomerBlocked = input.AtCustomerBlocked,
                AtCustomerChecked = input.AtCustomerChecked,
                AtCustomerDefect = input.AtCustomerDefect,
                AtSupplierBlocked = input.AtSupplierBlocked,
                AtSupplierChecked = input.AtSupplierChecked,
                AtSupplierDefect = input.AtSupplierDefect,
                PartNumber = input.PartNumber,
                CA1ContainmentAction = input.CA1ContainmentAction,
                CA1ContainmentActionD6 = input.CA1ContainmentActionD6,
                CA1ImplementationDate = input.CA1ImplementationDate,
                CA1PotentialRisk = input.CA1PotentialRisk,
                CA1RemovalDateD6 = input.CA1RemovalDateD6,
                CA1Responsible = input.CA1Responsible,
                CA1ResponsibleD6 = input.CA1ResponsibleD6,
                CA2ContainmentAction = input.CA2ContainmentAction,
                CA2ContainmentActionD6 = input.CA2ContainmentActionD6,
                CA2ImplementationDate = input.CA2ImplementationDate,
                CA2PotentialRisk = input.CA2PotentialRisk,
                CA2RemovalDateD6 = input.CA2RemovalDateD6,
                CA2Responsible = input.CA2Responsible,
                CA2ResponsibleD6 = input.CA2ResponsibleD6,
                CA3ContainmentAction = input.CA3ContainmentAction,
                CA3ContainmentActionD6 = input.CA3ContainmentActionD6,
                CA3ImplementationDate = input.CA3ImplementationDate,
                CA3PotentialRisk = input.CA3PotentialRisk,
                CA3RemovalDateD6 = input.CA3RemovalDateD6,
                CA3Responsible = input.CA3Responsible,
                CA3ResponsibleD6 = input.CA3ResponsibleD6,
                ClaimedQuantity = input.ClaimedQuantity,
                ClaimingPlants = input.ClaimingPlants,
                ClaimOpeningDate = input.ClaimOpeningDate,
                ComplaintJustified = input.ComplaintJustified,
                ContainmentActionDate = input.ContainmentActionDate,
                ControlPlanRevisionCompletionDate = input.ControlPlanRevisionCompletionDate,
                ControlPlanRevisionDocumentNumber = input.ControlPlanRevisionDocumentNumber,
                ControlPlanRevisionProofAttached = input.ControlPlanRevisionProofAttached,
                ControlPlanRevisionRelevant = input.ControlPlanRevisionRelevant,
                ControlPlanRevisionResponsible = input.ControlPlanRevisionResponsible,
                ControlPlanRevisionVersion = input.ControlPlanRevisionVersion,
                Customer8DClosureDate = input.Customer8DClosureDate,
                Customer8DClosureFunctionDepartment = input.Customer8DClosureFunctionDepartment,
                Customer8DClosureName = input.Customer8DClosureName,
                DateFinalRelease = input.DateFinalRelease,
                DateInterimReportD3 = input.DateInterimReportD3,
                DateInterimReportD5 = input.DateInterimReportD5,
                DeliveredQuantity = input.DeliveredQuantity,
                DeviationsProblems = input.DeviationsProblems,
                DeviationsSymptoms = input.DeviationsSymptoms,
                DFMEARevisionCompletionDate = input.DFMEARevisionCompletionDate,
                DFMEARevisionDocumentNumber = input.DFMEARevisionDocumentNumber,
                DFMEARevisionProofAttached = input.DFMEARevisionProofAttached,
                DFMEARevisionRelevant = input.DFMEARevisionRelevant,
                DFMEARevisionResponsible = input.DFMEARevisionResponsible,
                DFMEARevisionVersion = input.DFMEARevisionVersion,
                DrawingIndex = input.DrawingIndex,
                FailureOccurance = input.FailureOccurance,
                IA1CorrectiveAction = input.IA1CorrectiveAction,
                IA1EffectiveFromDate = input.IA1EffectiveFromDate,
                IA1ImplementationDate = input.IA1ImplementationDate,
                IA1ProofAttached = input.IA1ProofAttached,
                IA1RootCause = input.IA1RootCause,
                IA1ValidatedDate = input.IA1ValidatedDate,
                IA2CorrectiveAction = input.IA2CorrectiveAction,
                IA2EffectiveFromDate = input.IA2EffectiveFromDate,
                IA2ImplementationDate = input.IA2ImplementationDate,
                IA2ProofAttached = input.IA2ProofAttached,
                IA2RootCause = input.IA2RootCause,
                IA2ValidatedDate = input.IA2ValidatedDate,
                IA3CorrectiveAction = input.IA3CorrectiveAction,
                IA3EffectiveFromDate = input.IA3EffectiveFromDate,
                IA3ImplementationDate = input.IA3ImplementationDate,
                IA3ProofAttached = input.IA3ProofAttached,
                IA3RootCause = input.IA3RootCause,
                IA3ValidatedDate = input.IA3ValidatedDate,
                IA4CorrectiveAction = input.IA4CorrectiveAction,
                IA4EffectiveFromDate = input.IA4EffectiveFromDate,
                IA4ImplementationDate = input.IA4ImplementationDate,
                IA4ProofAttached = input.IA4ProofAttached,
                IA4RootCause = input.IA4RootCause,
                IA4ValidatedDate = input.IA4ValidatedDate,
                IA5CorrectiveAction = input.IA5CorrectiveAction,
                IA5EffectiveFromDate = input.IA5EffectiveFromDate,
                IA5ImplementationDate = input.IA5ImplementationDate,
                IA5ProofAttached = input.IA5ProofAttached,
                IA5RootCause = input.IA5RootCause,
                IA5ValidatedDate = input.IA5ValidatedDate,
                InTransitBlocked = input.InTransitBlocked,
                InTransitChecked = input.InTransitChecked,
                InTransitDefect = input.InTransitDefect,
                LessonsLearnedDate = input.LessonsLearnedDate,
                LessonsLearnedFunctionDepartment = input.LessonsLearnedFunctionDepartment,
                LessonsLearnedProofAttached = input.LessonsLearnedProofAttached,
                LessonsLearnedRelevant = input.LessonsLearnedRelevant,
                LessonsLearnedResponsible = input.LessonsLearnedResponsible,
                OtherAffectedPlants = input.OtherAffectedPlants,
                PA1PlannedImplementationDate = input.PA1PlannedImplementationDate,
                PA1PotentialCorrectiveAction = input.PA1PotentialCorrectiveAction,
                PA1Responsible = input.PA1Responsible,
                PA1RootCause = input.PA1RootCause,
                PA1ToBeImplemented = input.PA1ToBeImplemented,
                PA2PlannedImplementationDate = input.PA2PlannedImplementationDate,
                PA2PotentialCorrectiveAction = input.PA2PotentialCorrectiveAction,
                PA2Responsible = input.PA2Responsible,
                PA2RootCause = input.PA2RootCause,
                PA2ToBeImplemented = input.PA2ToBeImplemented,
                PA3PlannedImplementationDate = input.PA3PlannedImplementationDate,
                PA3PotentialCorrectiveAction = input.PA3PotentialCorrectiveAction,
                PA3Responsible = input.PA3Responsible,
                PA3RootCause = input.PA3RootCause,
                PA3ToBeImplemented = input.PA3ToBeImplemented,
                PA4PlannedImplementationDate = input.PA4PlannedImplementationDate,
                PA4PotentialCorrectiveAction = input.PA4PotentialCorrectiveAction,
                PA4Responsible = input.PA4Responsible,
                PA4RootCause = input.PA4RootCause,
                PA4ToBeImplemented = input.PA4ToBeImplemented,
                PA5PlannedImplementationDate = input.PA5PlannedImplementationDate,
                PA5PotentialCorrectiveAction = input.PA5PotentialCorrectiveAction,
                PA5Responsible = input.PA5Responsible,
                PA5RootCause = input.PA5RootCause,
                PA5ToBeImplemented = input.PA5ToBeImplemented,
                PFMEARevisionCompletionDate = input.PFMEARevisionCompletionDate,
                PFMEARevisionDocumentNumber = input.PFMEARevisionDocumentNumber,
                PFMEARevisionProofAttached = input.PFMEARevisionProofAttached,
                PFMEARevisionRelevant = input.PFMEARevisionRelevant,
                PFMEARevisionResponsible = input.PFMEARevisionResponsible,
                PFMEARevisionVersion = input.PFMEARevisionVersion,
                ProductionPlant = input.ProductionPlant,
                Report8DAccepted = input.Report8DAccepted,
                Report8DRevision = input.Report8DRevision,
                Revision1Action = input.Revision1Action,
                Revision1CompletionDate = input.Revision1CompletionDate,
                Revision1DocumentNumber = input.Revision1DocumentNumber,
                Revision1ProofAttached = input.Revision1ProofAttached,
                Revision1Relevant = input.Revision1Relevant,
                Revision1Responsible = input.Revision1Responsible,
                Revision1Version = input.Revision1Version,
                Revision2Action = input.Revision2Action,
                Revision2CompletionDate = input.Revision2CompletionDate,
                Revision2DocumentNumber = input.Revision2DocumentNumber,
                Revision2ProofAttached = input.Revision2ProofAttached,
                Revision2Relevant = input.Revision2Relevant,
                Revision2Responsible = input.Revision2Responsible,
                Revision2Version = input.Revision2Version,
                Revision3Action = input.Revision3Action,
                Revision3CompletionDate = input.Revision3CompletionDate,
                Revision3DocumentNumber = input.Revision3DocumentNumber,
                Revision3ProofAttached = input.Revision3ProofAttached,
                Revision3Relevant = input.Revision3Relevant,
                Revision3Responsible = input.Revision3Responsible,
                Revision3Version = input.Revision3Version,
                RN1AnalysisMethod = input.RN1AnalysisMethod,
                RN1NonDetectionReason = input.RN1NonDetectionReason,
                RN1Share = input.RN1Share,
                RN2AnalysisMethod = input.RN2AnalysisMethod,
                RN2NonDetectionReason = input.RN2NonDetectionReason,
                RN2Share = input.RN2Share,
                RO1AnalysisMethod = input.RO1AnalysisMethod,
                RO1OccuranceReason = input.RO1OccuranceReason,
                RO1Share = input.RO1Share,
                RO2AnalysisMethod = input.RO2AnalysisMethod,
                RO2OccuranceReason = input.RO2OccuranceReason,
                RO2Share = input.RO2Share,
                Sponsor = input.Sponsor,
                SponsorD8 = input.SponsorD8,
                SponsorDateD8 = input.SponsorDateD8,
                SponsorEMail = input.SponsorEMail,
                SponsorFunctionDepartment = input.SponsorFunctionDepartment,
                SponsorPhone = input.SponsorPhone,
                TeamLeader = input.TeamLeader,
                TeamLeaderD8 = input.TeamLeaderD8,
                TeamLeaderDateD8 = input.TeamLeaderDateD8,
                TeamLeaderEMail = input.TeamLeaderEMail,
                TeamLeaderFunctionDepartment = input.TeamLeaderFunctionDepartment,
                TeamLeaderPhone = input.TeamLeaderPhone,
                TeamMember1 = input.TeamMember1,
                TeamMember1EMail = input.TeamMember1EMail,
                TeamMember1FunctionDepartment = input.TeamMember1FunctionDepartment,
                TeamMember1Phone = input.TeamMember1Phone,
                TeamMember2 = input.TeamMember2,
                TeamMember2EMail = input.TeamMember2EMail,
                TeamMember2FunctionDepartment = input.TeamMember2FunctionDepartment,
                TeamMember2Phone = input.TeamMember2Phone,
                TeamMember3 = input.TeamMember3,
                TeamMember3EMail = input.TeamMember3EMail,
                TeamMember3FunctionDepartment = input.TeamMember3FunctionDepartment,
                TeamMember3Phone = input.TeamMember3Phone,
                TopicTitle = input.TopicTitle,
                UpdateRequiredUntilDate = input.UpdateRequiredUntilDate,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id },  "");

            var Report8Ds = queryFactory.Update<SelectReport8DsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, Report8Ds, LoginedUserService.UserId, Tables.Report8Ds, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["Report8DChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                 
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = notTemplate.ContextMenuName_,
                            IsViewed = false,
                             
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectReport8DsDto>(Report8Ds);
        }

        public async Task<IDataResult<SelectReport8DsDto>> UpdateStateAsync(UpdateReport8DsDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Report8Ds).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<Report8Ds>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Report8Ds).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<Report8Ds>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            DateTime now = _GetSQLDateAppService.GetDateFromSQL();

            var query = queryFactory.Query().From(Tables.Report8Ds).Update(new UpdateReport8DsDto
            {
                Code = input.Code,
                State_ = input.State_,
                TechnicalDrawingID = input.TechnicalDrawingID.GetValueOrDefault(),
                ProductID = input.ProductID.GetValueOrDefault(),
                SupplierID = input.SupplierID.GetValueOrDefault(),
                CustomerID = input.CustomerID.GetValueOrDefault(),
                AtCustomerBlocked = input.AtCustomerBlocked,
                AtCustomerChecked = input.AtCustomerChecked,
                AtCustomerDefect = input.AtCustomerDefect,
                AtSupplierBlocked = input.AtSupplierBlocked,
                AtSupplierChecked = input.AtSupplierChecked,
                AtSupplierDefect = input.AtSupplierDefect,
                PartNumber = input.PartNumber,
                CA1ContainmentAction = input.CA1ContainmentAction,
                CA1ContainmentActionD6 = input.CA1ContainmentActionD6,
                CA1ImplementationDate = input.CA1ImplementationDate,
                CA1PotentialRisk = input.CA1PotentialRisk,
                CA1RemovalDateD6 = input.CA1RemovalDateD6,
                CA1Responsible = input.CA1Responsible,
                CA1ResponsibleD6 = input.CA1ResponsibleD6,
                CA2ContainmentAction = input.CA2ContainmentAction,
                CA2ContainmentActionD6 = input.CA2ContainmentActionD6,
                CA2ImplementationDate = input.CA2ImplementationDate,
                CA2PotentialRisk = input.CA2PotentialRisk,
                CA2RemovalDateD6 = input.CA2RemovalDateD6,
                CA2Responsible = input.CA2Responsible,
                CA2ResponsibleD6 = input.CA2ResponsibleD6,
                CA3ContainmentAction = input.CA3ContainmentAction,
                CA3ContainmentActionD6 = input.CA3ContainmentActionD6,
                CA3ImplementationDate = input.CA3ImplementationDate,
                CA3PotentialRisk = input.CA3PotentialRisk,
                CA3RemovalDateD6 = input.CA3RemovalDateD6,
                CA3Responsible = input.CA3Responsible,
                CA3ResponsibleD6 = input.CA3ResponsibleD6,
                ClaimedQuantity = input.ClaimedQuantity,
                ClaimingPlants = input.ClaimingPlants,
                ClaimOpeningDate = input.ClaimOpeningDate,
                ComplaintJustified = input.ComplaintJustified,
                ContainmentActionDate = input.ContainmentActionDate,
                ControlPlanRevisionCompletionDate = input.ControlPlanRevisionCompletionDate,
                ControlPlanRevisionDocumentNumber = input.ControlPlanRevisionDocumentNumber,
                ControlPlanRevisionProofAttached = input.ControlPlanRevisionProofAttached,
                ControlPlanRevisionRelevant = input.ControlPlanRevisionRelevant,
                ControlPlanRevisionResponsible = input.ControlPlanRevisionResponsible,
                ControlPlanRevisionVersion = input.ControlPlanRevisionVersion,
                Customer8DClosureDate = input.Customer8DClosureDate,
                Customer8DClosureFunctionDepartment = input.Customer8DClosureFunctionDepartment,
                Customer8DClosureName = input.Customer8DClosureName,
                DateFinalRelease = input.DateFinalRelease,
                DateInterimReportD3 = input.DateInterimReportD3,
                DateInterimReportD5 = input.DateInterimReportD5,
                DeliveredQuantity = input.DeliveredQuantity,
                DeviationsProblems = input.DeviationsProblems,
                DeviationsSymptoms = input.DeviationsSymptoms,
                DFMEARevisionCompletionDate = input.DFMEARevisionCompletionDate,
                DFMEARevisionDocumentNumber = input.DFMEARevisionDocumentNumber,
                DFMEARevisionProofAttached = input.DFMEARevisionProofAttached,
                DFMEARevisionRelevant = input.DFMEARevisionRelevant,
                DFMEARevisionResponsible = input.DFMEARevisionResponsible,
                DFMEARevisionVersion = input.DFMEARevisionVersion,
                DrawingIndex = input.DrawingIndex,
                FailureOccurance = input.FailureOccurance,
                IA1CorrectiveAction = input.IA1CorrectiveAction,
                IA1EffectiveFromDate = input.IA1EffectiveFromDate,
                IA1ImplementationDate = input.IA1ImplementationDate,
                IA1ProofAttached = input.IA1ProofAttached,
                IA1RootCause = input.IA1RootCause,
                IA1ValidatedDate = input.IA1ValidatedDate,
                IA2CorrectiveAction = input.IA2CorrectiveAction,
                IA2EffectiveFromDate = input.IA2EffectiveFromDate,
                IA2ImplementationDate = input.IA2ImplementationDate,
                IA2ProofAttached = input.IA2ProofAttached,
                IA2RootCause = input.IA2RootCause,
                IA2ValidatedDate = input.IA2ValidatedDate,
                IA3CorrectiveAction = input.IA3CorrectiveAction,
                IA3EffectiveFromDate = input.IA3EffectiveFromDate,
                IA3ImplementationDate = input.IA3ImplementationDate,
                IA3ProofAttached = input.IA3ProofAttached,
                IA3RootCause = input.IA3RootCause,
                IA3ValidatedDate = input.IA3ValidatedDate,
                IA4CorrectiveAction = input.IA4CorrectiveAction,
                IA4EffectiveFromDate = input.IA4EffectiveFromDate,
                IA4ImplementationDate = input.IA4ImplementationDate,
                IA4ProofAttached = input.IA4ProofAttached,
                IA4RootCause = input.IA4RootCause,
                IA4ValidatedDate = input.IA4ValidatedDate,
                IA5CorrectiveAction = input.IA5CorrectiveAction,
                IA5EffectiveFromDate = input.IA5EffectiveFromDate,
                IA5ImplementationDate = input.IA5ImplementationDate,
                IA5ProofAttached = input.IA5ProofAttached,
                IA5RootCause = input.IA5RootCause,
                IA5ValidatedDate = input.IA5ValidatedDate,
                InTransitBlocked = input.InTransitBlocked,
                InTransitChecked = input.InTransitChecked,
                InTransitDefect = input.InTransitDefect,
                LessonsLearnedDate = input.LessonsLearnedDate,
                LessonsLearnedFunctionDepartment = input.LessonsLearnedFunctionDepartment,
                LessonsLearnedProofAttached = input.LessonsLearnedProofAttached,
                LessonsLearnedRelevant = input.LessonsLearnedRelevant,
                LessonsLearnedResponsible = input.LessonsLearnedResponsible,
                OtherAffectedPlants = input.OtherAffectedPlants,
                PA1PlannedImplementationDate = input.PA1PlannedImplementationDate,
                PA1PotentialCorrectiveAction = input.PA1PotentialCorrectiveAction,
                PA1Responsible = input.PA1Responsible,
                PA1RootCause = input.PA1RootCause,
                PA1ToBeImplemented = input.PA1ToBeImplemented,
                PA2PlannedImplementationDate = input.PA2PlannedImplementationDate,
                PA2PotentialCorrectiveAction = input.PA2PotentialCorrectiveAction,
                PA2Responsible = input.PA2Responsible,
                PA2RootCause = input.PA2RootCause,
                PA2ToBeImplemented = input.PA2ToBeImplemented,
                PA3PlannedImplementationDate = input.PA3PlannedImplementationDate,
                PA3PotentialCorrectiveAction = input.PA3PotentialCorrectiveAction,
                PA3Responsible = input.PA3Responsible,
                PA3RootCause = input.PA3RootCause,
                PA3ToBeImplemented = input.PA3ToBeImplemented,
                PA4PlannedImplementationDate = input.PA4PlannedImplementationDate,
                PA4PotentialCorrectiveAction = input.PA4PotentialCorrectiveAction,
                PA4Responsible = input.PA4Responsible,
                PA4RootCause = input.PA4RootCause,
                PA4ToBeImplemented = input.PA4ToBeImplemented,
                PA5PlannedImplementationDate = input.PA5PlannedImplementationDate,
                PA5PotentialCorrectiveAction = input.PA5PotentialCorrectiveAction,
                PA5Responsible = input.PA5Responsible,
                PA5RootCause = input.PA5RootCause,
                PA5ToBeImplemented = input.PA5ToBeImplemented,
                PFMEARevisionCompletionDate = input.PFMEARevisionCompletionDate,
                PFMEARevisionDocumentNumber = input.PFMEARevisionDocumentNumber,
                PFMEARevisionProofAttached = input.PFMEARevisionProofAttached,
                PFMEARevisionRelevant = input.PFMEARevisionRelevant,
                PFMEARevisionResponsible = input.PFMEARevisionResponsible,
                PFMEARevisionVersion = input.PFMEARevisionVersion,
                ProductionPlant = input.ProductionPlant,
                Report8DAccepted = input.Report8DAccepted,
                Report8DRevision = input.Report8DRevision,
                Revision1Action = input.Revision1Action,
                Revision1CompletionDate = input.Revision1CompletionDate,
                Revision1DocumentNumber = input.Revision1DocumentNumber,
                Revision1ProofAttached = input.Revision1ProofAttached,
                Revision1Relevant = input.Revision1Relevant,
                Revision1Responsible = input.Revision1Responsible,
                Revision1Version = input.Revision1Version,
                Revision2Action = input.Revision2Action,
                Revision2CompletionDate = input.Revision2CompletionDate,
                Revision2DocumentNumber = input.Revision2DocumentNumber,
                Revision2ProofAttached = input.Revision2ProofAttached,
                Revision2Relevant = input.Revision2Relevant,
                Revision2Responsible = input.Revision2Responsible,
                Revision2Version = input.Revision2Version,
                Revision3Action = input.Revision3Action,
                Revision3CompletionDate = input.Revision3CompletionDate,
                Revision3DocumentNumber = input.Revision3DocumentNumber,
                Revision3ProofAttached = input.Revision3ProofAttached,
                Revision3Relevant = input.Revision3Relevant,
                Revision3Responsible = input.Revision3Responsible,
                Revision3Version = input.Revision3Version,
                RN1AnalysisMethod = input.RN1AnalysisMethod,
                RN1NonDetectionReason = input.RN1NonDetectionReason,
                RN1Share = input.RN1Share,
                RN2AnalysisMethod = input.RN2AnalysisMethod,
                RN2NonDetectionReason = input.RN2NonDetectionReason,
                RN2Share = input.RN2Share,
                RO1AnalysisMethod = input.RO1AnalysisMethod,
                RO1OccuranceReason = input.RO1OccuranceReason,
                RO1Share = input.RO1Share,
                RO2AnalysisMethod = input.RO2AnalysisMethod,
                RO2OccuranceReason = input.RO2OccuranceReason,
                RO2Share = input.RO2Share,
                Sponsor = input.Sponsor,
                SponsorD8 = input.SponsorD8,
                SponsorDateD8 = input.SponsorDateD8,
                SponsorEMail = input.SponsorEMail,
                SponsorFunctionDepartment = input.SponsorFunctionDepartment,
                SponsorPhone = input.SponsorPhone,
                TeamLeader = input.TeamLeader,
                TeamLeaderD8 = input.TeamLeaderD8,
                TeamLeaderDateD8 = input.TeamLeaderDateD8,
                TeamLeaderEMail = input.TeamLeaderEMail,
                TeamLeaderFunctionDepartment = input.TeamLeaderFunctionDepartment,
                TeamLeaderPhone = input.TeamLeaderPhone,
                TeamMember1 = input.TeamMember1,
                TeamMember1EMail = input.TeamMember1EMail,
                TeamMember1FunctionDepartment = input.TeamMember1FunctionDepartment,
                TeamMember1Phone = input.TeamMember1Phone,
                TeamMember2 = input.TeamMember2,
                TeamMember2EMail = input.TeamMember2EMail,
                TeamMember2FunctionDepartment = input.TeamMember2FunctionDepartment,
                TeamMember2Phone = input.TeamMember2Phone,
                TeamMember3 = input.TeamMember3,
                TeamMember3EMail = input.TeamMember3EMail,
                TeamMember3FunctionDepartment = input.TeamMember3FunctionDepartment,
                TeamMember3Phone = input.TeamMember3Phone,
                TopicTitle = input.TopicTitle,
                UpdateRequiredUntilDate = input.UpdateRequiredUntilDate,
                Id = input.Id,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = now,
                LastModifierId = LoginedUserService.UserId
            }).Where(new { Id = input.Id }, "");

            var Report8Ds = queryFactory.Update<SelectReport8DsDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, Report8Ds, LoginedUserService.UserId, Tables.Report8Ds, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessContextAsync(L["Report8DChildMenu"], L["8DContextState"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains("*Not*"))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split("*Not*");

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = L["8DContextState"],
                                IsViewed = false,
                                 
                                ModuleName_ = notTemplate.ModuleName_,
                                ProcessName_ = notTemplate.ProcessName_,
                                RecordNumber = input.Code,
                                NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                                UserId = new Guid(user),
                                ViewDate = null,
                            };

                            await _NotificationsAppService.CreateAsync(createInput);
                        }
                    }
                    else
                    {
                        CreateNotificationsDto createInput = new CreateNotificationsDto
                        {
                            ContextMenuName_ = L["8DContextState"],
                            IsViewed = false,
                             
                            ModuleName_ = notTemplate.ModuleName_,
                            ProcessName_ = notTemplate.ProcessName_,
                            RecordNumber = input.Code,
                            NotificationDate = _GetSQLDateAppService.GetDateFromSQL(),
                            UserId = new Guid(notTemplate.TargetUsersId),
                            ViewDate = null,
                        };

                        await _NotificationsAppService.CreateAsync(createInput);
                    }
                }

            }

            #endregion

            await Task.CompletedTask;
            return new SuccessDataResult<SelectReport8DsDto>(Report8Ds);
        }

        public async Task<IDataResult<SelectReport8DsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Report8Ds).Select("*").Where(new { Id = id },  "");
            var entity = queryFactory.Get<Report8Ds>(entityQuery);

            var query = queryFactory.Query().From(Tables.Report8Ds).Update(new UpdateReport8DsDto
            {
                Code = entity.Code,
                State_ = entity.State_,
                TechnicalDrawingID = entity.TechnicalDrawingID,
                ProductID = entity.ProductID,
                SupplierID = entity.SupplierID,
                CustomerID = entity.CustomerID,
                AtCustomerBlocked = entity.AtCustomerBlocked,
                AtCustomerChecked = entity.AtCustomerChecked,
                AtCustomerDefect = entity.AtCustomerDefect,
                AtSupplierBlocked = entity.AtSupplierBlocked,
                AtSupplierChecked = entity.AtSupplierChecked,
                AtSupplierDefect = entity.AtSupplierDefect,
                CA1ContainmentAction = entity.CA1ContainmentAction,
                CA1ContainmentActionD6 = entity.CA1ContainmentActionD6,
                CA1ImplementationDate = entity.CA1ImplementationDate,
                CA1PotentialRisk = entity.CA1PotentialRisk,
                CA1RemovalDateD6 = entity.CA1RemovalDateD6,
                CA1Responsible = entity.CA1Responsible,
                CA1ResponsibleD6 = entity.CA1ResponsibleD6,
                CA2ContainmentAction = entity.CA2ContainmentAction,
                CA2ContainmentActionD6 = entity.CA2ContainmentActionD6,
                CA2ImplementationDate = entity.CA2ImplementationDate,
                CA2PotentialRisk = entity.CA2PotentialRisk,
                CA2RemovalDateD6 = entity.CA2RemovalDateD6,
                PartNumber = entity.PartNumber,
                CA2Responsible = entity.CA2Responsible,
                CA2ResponsibleD6 = entity.CA2ResponsibleD6,
                CA3ContainmentAction = entity.CA3ContainmentAction,
                CA3ContainmentActionD6 = entity.CA3ContainmentActionD6,
                CA3ImplementationDate = entity.CA3ImplementationDate,
                CA3PotentialRisk = entity.CA3PotentialRisk,
                CA3RemovalDateD6 = entity.CA3RemovalDateD6,
                CA3Responsible = entity.CA3Responsible,
                CA3ResponsibleD6 = entity.CA3ResponsibleD6,
                ClaimedQuantity = entity.ClaimedQuantity,
                ClaimingPlants = entity.ClaimingPlants,
                ClaimOpeningDate = entity.ClaimOpeningDate,
                ComplaintJustified = entity.ComplaintJustified,
                ContainmentActionDate = entity.ContainmentActionDate,
                ControlPlanRevisionCompletionDate = entity.ControlPlanRevisionCompletionDate,
                ControlPlanRevisionDocumentNumber = entity.ControlPlanRevisionDocumentNumber,
                ControlPlanRevisionProofAttached = entity.ControlPlanRevisionProofAttached,
                ControlPlanRevisionRelevant = entity.ControlPlanRevisionRelevant,
                ControlPlanRevisionResponsible = entity.ControlPlanRevisionResponsible,
                ControlPlanRevisionVersion = entity.ControlPlanRevisionVersion,
                Customer8DClosureDate = entity.Customer8DClosureDate,
                Customer8DClosureFunctionDepartment = entity.Customer8DClosureFunctionDepartment,
                Customer8DClosureName = entity.Customer8DClosureName,
                DateFinalRelease = entity.DateFinalRelease,
                DateInterimReportD3 = entity.DateInterimReportD3,
                DateInterimReportD5 = entity.DateInterimReportD5,
                DeliveredQuantity = entity.DeliveredQuantity,
                DeviationsProblems = entity.DeviationsProblems,
                DeviationsSymptoms = entity.DeviationsSymptoms,
                DFMEARevisionCompletionDate = entity.DFMEARevisionCompletionDate,
                DFMEARevisionDocumentNumber = entity.DFMEARevisionDocumentNumber,
                DFMEARevisionProofAttached = entity.DFMEARevisionProofAttached,
                DFMEARevisionRelevant = entity.DFMEARevisionRelevant,
                DFMEARevisionResponsible = entity.DFMEARevisionResponsible,
                DFMEARevisionVersion = entity.DFMEARevisionVersion,
                DrawingIndex = entity.DrawingIndex,
                FailureOccurance = entity.FailureOccurance,
                IA1CorrectiveAction = entity.IA1CorrectiveAction,
                IA1EffectiveFromDate = entity.IA1EffectiveFromDate,
                IA1ImplementationDate = entity.IA1ImplementationDate,
                IA1ProofAttached = entity.IA1ProofAttached,
                IA1RootCause = entity.IA1RootCause,
                IA1ValidatedDate = entity.IA1ValidatedDate,
                IA2CorrectiveAction = entity.IA2CorrectiveAction,
                IA2EffectiveFromDate = entity.IA2EffectiveFromDate,
                IA2ImplementationDate = entity.IA2ImplementationDate,
                IA2ProofAttached = entity.IA2ProofAttached,
                IA2RootCause = entity.IA2RootCause,
                IA2ValidatedDate = entity.IA2ValidatedDate,
                IA3CorrectiveAction = entity.IA3CorrectiveAction,
                IA3EffectiveFromDate = entity.IA3EffectiveFromDate,
                IA3ImplementationDate = entity.IA3ImplementationDate,
                IA3ProofAttached = entity.IA3ProofAttached,
                IA3RootCause = entity.IA3RootCause,
                IA3ValidatedDate = entity.IA3ValidatedDate,
                IA4CorrectiveAction = entity.IA4CorrectiveAction,
                IA4EffectiveFromDate = entity.IA4EffectiveFromDate,
                IA4ImplementationDate = entity.IA4ImplementationDate,
                IA4ProofAttached = entity.IA4ProofAttached,
                IA4RootCause = entity.IA4RootCause,
                IA4ValidatedDate = entity.IA4ValidatedDate,
                IA5CorrectiveAction = entity.IA5CorrectiveAction,
                IA5EffectiveFromDate = entity.IA5EffectiveFromDate,
                IA5ImplementationDate = entity.IA5ImplementationDate,
                IA5ProofAttached = entity.IA5ProofAttached,
                IA5RootCause = entity.IA5RootCause,
                IA5ValidatedDate = entity.IA5ValidatedDate,
                InTransitBlocked = entity.InTransitBlocked,
                InTransitChecked = entity.InTransitChecked,
                InTransitDefect = entity.InTransitDefect,
                LessonsLearnedDate = entity.LessonsLearnedDate,
                LessonsLearnedFunctionDepartment = entity.LessonsLearnedFunctionDepartment,
                LessonsLearnedProofAttached = entity.LessonsLearnedProofAttached,
                LessonsLearnedRelevant = entity.LessonsLearnedRelevant,
                LessonsLearnedResponsible = entity.LessonsLearnedResponsible,
                OtherAffectedPlants = entity.OtherAffectedPlants,
                PA1PlannedImplementationDate = entity.PA1PlannedImplementationDate,
                PA1PotentialCorrectiveAction = entity.PA1PotentialCorrectiveAction,
                PA1Responsible = entity.PA1Responsible,
                PA1RootCause = entity.PA1RootCause,
                PA1ToBeImplemented = entity.PA1ToBeImplemented,
                PA2PlannedImplementationDate = entity.PA2PlannedImplementationDate,
                PA2PotentialCorrectiveAction = entity.PA2PotentialCorrectiveAction,
                PA2Responsible = entity.PA2Responsible,
                PA2RootCause = entity.PA2RootCause,
                PA2ToBeImplemented = entity.PA2ToBeImplemented,
                PA3PlannedImplementationDate = entity.PA3PlannedImplementationDate,
                PA3PotentialCorrectiveAction = entity.PA3PotentialCorrectiveAction,
                PA3Responsible = entity.PA3Responsible,
                PA3RootCause = entity.PA3RootCause,
                PA3ToBeImplemented = entity.PA3ToBeImplemented,
                PA4PlannedImplementationDate = entity.PA4PlannedImplementationDate,
                PA4PotentialCorrectiveAction = entity.PA4PotentialCorrectiveAction,
                PA4Responsible = entity.PA4Responsible,
                PA4RootCause = entity.PA4RootCause,
                PA4ToBeImplemented = entity.PA4ToBeImplemented,
                PA5PlannedImplementationDate = entity.PA5PlannedImplementationDate,
                PA5PotentialCorrectiveAction = entity.PA5PotentialCorrectiveAction,
                PA5Responsible = entity.PA5Responsible,
                PA5RootCause = entity.PA5RootCause,
                PA5ToBeImplemented = entity.PA5ToBeImplemented,
                PFMEARevisionCompletionDate = entity.PFMEARevisionCompletionDate,
                PFMEARevisionDocumentNumber = entity.PFMEARevisionDocumentNumber,
                PFMEARevisionProofAttached = entity.PFMEARevisionProofAttached,
                PFMEARevisionRelevant = entity.PFMEARevisionRelevant,
                PFMEARevisionResponsible = entity.PFMEARevisionResponsible,
                PFMEARevisionVersion = entity.PFMEARevisionVersion,
                ProductionPlant = entity.ProductionPlant,
                Report8DAccepted = entity.Report8DAccepted,
                Report8DRevision = entity.Report8DRevision,
                Revision1Action = entity.Revision1Action,
                Revision1CompletionDate = entity.Revision1CompletionDate,
                Revision1DocumentNumber = entity.Revision1DocumentNumber,
                Revision1ProofAttached = entity.Revision1ProofAttached,
                Revision1Relevant = entity.Revision1Relevant,
                Revision1Responsible = entity.Revision1Responsible,
                Revision1Version = entity.Revision1Version,
                Revision2Action = entity.Revision2Action,
                Revision2CompletionDate = entity.Revision2CompletionDate,
                Revision2DocumentNumber = entity.Revision2DocumentNumber,
                Revision2ProofAttached = entity.Revision2ProofAttached,
                Revision2Relevant = entity.Revision2Relevant,
                Revision2Responsible = entity.Revision2Responsible,
                Revision2Version = entity.Revision2Version,
                Revision3Action = entity.Revision3Action,
                Revision3CompletionDate = entity.Revision3CompletionDate,
                Revision3DocumentNumber = entity.Revision3DocumentNumber,
                Revision3ProofAttached = entity.Revision3ProofAttached,
                Revision3Relevant = entity.Revision3Relevant,
                Revision3Responsible = entity.Revision3Responsible,
                Revision3Version = entity.Revision3Version,
                RN1AnalysisMethod = entity.RN1AnalysisMethod,
                RN1NonDetectionReason = entity.RN1NonDetectionReason,
                RN1Share = entity.RN1Share,
                RN2AnalysisMethod = entity.RN2AnalysisMethod,
                RN2NonDetectionReason = entity.RN2NonDetectionReason,
                RN2Share = entity.RN2Share,
                RO1AnalysisMethod = entity.RO1AnalysisMethod,
                RO1OccuranceReason = entity.RO1OccuranceReason,
                RO1Share = entity.RO1Share,
                RO2AnalysisMethod = entity.RO2AnalysisMethod,
                RO2OccuranceReason = entity.RO2OccuranceReason,
                RO2Share = entity.RO2Share,
                Sponsor = entity.Sponsor,
                SponsorD8 = entity.SponsorD8,
                SponsorDateD8 = entity.SponsorDateD8,
                SponsorEMail = entity.SponsorEMail,
                SponsorFunctionDepartment = entity.SponsorFunctionDepartment,
                SponsorPhone = entity.SponsorPhone,
                TeamLeader = entity.TeamLeader,
                TeamLeaderD8 = entity.TeamLeaderD8,
                TeamLeaderDateD8 = entity.TeamLeaderDateD8,
                TeamLeaderEMail = entity.TeamLeaderEMail,
                TeamLeaderFunctionDepartment = entity.TeamLeaderFunctionDepartment,
                TeamLeaderPhone = entity.TeamLeaderPhone,
                TeamMember1 = entity.TeamMember1,
                TeamMember1EMail = entity.TeamMember1EMail,
                TeamMember1FunctionDepartment = entity.TeamMember1FunctionDepartment,
                TeamMember1Phone = entity.TeamMember1Phone,
                TeamMember2 = entity.TeamMember2,
                TeamMember2EMail = entity.TeamMember2EMail,
                TeamMember2FunctionDepartment = entity.TeamMember2FunctionDepartment,
                TeamMember2Phone = entity.TeamMember2Phone,
                TeamMember3 = entity.TeamMember3,
                TeamMember3EMail = entity.TeamMember3EMail,
                TeamMember3FunctionDepartment = entity.TeamMember3FunctionDepartment,
                TeamMember3Phone = entity.TeamMember3Phone,
                TopicTitle = entity.TopicTitle,
                UpdateRequiredUntilDate = entity.UpdateRequiredUntilDate,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime.GetValueOrDefault(),
                LastModifierId = entity.LastModifierId.GetValueOrDefault(),
                Id = id,
                DataOpenStatus = lockRow,
                DataOpenStatusUserId = userId,

            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var Report8Ds = queryFactory.Update<SelectReport8DsDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectReport8DsDto>(Report8Ds);

        }
    }
}
