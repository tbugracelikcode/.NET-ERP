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
using TsiErp.Business.Entities.Employee.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.GeneralSystemIdentifications.NotificationTemplate.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.Business.Entities.Other.Notification.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Branch;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.Entities.Other.Notification.Dtos;
using TsiErp.Entities.Entities.StockManagement.TechnicalDrawing.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Employees.Page;

namespace TsiErp.Business.Entities.Employee.Services
{
    [ServiceRegistration(typeof(IEmployeesAppService), DependencyInjectionType.Scoped)]
    public class EmployeesAppService : ApplicationService<EmployeesResource>, IEmployeesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }
        private readonly IGetSQLDateAppService _GetSQLDateAppService;
        private readonly INotificationsAppService _NotificationsAppService;
        private readonly INotificationTemplatesAppService _NotificationTemplatesAppService;

        public EmployeesAppService(IStringLocalizer<EmployeesResource> l, IFicheNumbersAppService ficheNumbersAppService, IGetSQLDateAppService getSQLDateAppService, INotificationTemplatesAppService notificationTemplatesAppService, INotificationsAppService notificationsAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
            _GetSQLDateAppService = getSQLDateAppService;
            _NotificationsAppService = notificationsAppService;
            _NotificationTemplatesAppService = notificationTemplatesAppService;
        }


        [ValidationAspect(typeof(CreateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> CreateAsync(CreateEmployeesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Employees).Select("Code").Where(new { Code = input.Code }, "");

            var list = queryFactory.ControlList<Employees>(listQuery).ToList();

            #region Code Control 

            if (list.Count > 0)
            {
                throw new DuplicateCodeException(L["CodeControlManager"]);
            }

            #endregion

            Guid addedEntityId = GuidGenerator.CreateGuid();


            var query = queryFactory.Query().From(Tables.Employees).Insert(new CreateEmployeesDto
            {
                Code = input.Code,
                Address = input.Address,
                Birthday = input.Birthday,
                CurrentSalary = input.CurrentSalary,
                EducationLevelID = input.EducationLevelID.GetValueOrDefault(),
                HiringDate = input.HiringDate,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                TaskDefinition = input.TaskDefinition,
                BloodType = (int)input.BloodType,
                CellPhone = input.CellPhone,
                City = input.City,
                DepartmentID = input.DepartmentID.GetValueOrDefault(),
                District = input.District,
                Email = input.Email,
                HomePhone = input.HomePhone,
                IDnumber = input.IDnumber,
                Surname = input.Surname,
                CreationTime = _GetSQLDateAppService.GetDateFromSQL(),
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsDeleted = false,
                LastModificationTime = null,
                LastModifierId = Guid.Empty,
                Name = input.Name,
                IsProductionScreenUser = input.IsProductionScreenUser,
                ProductionScreenPassword = input.ProductionScreenPassword,
                IsProductionScreenSettingUser = input.IsProductionScreenSettingUser
            });

            var employees = queryFactory.Insert<SelectEmployeesDto>(query, "Id", true);

            await FicheNumbersAppService.UpdateFicheNumberAsync("EmployeesChildMenu", input.Code);

            LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Employees, LogType.Insert, addedEntityId);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeesChildMenu"], L["ProcessAdd"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
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
                            Message_ = notTemplate.Message_,
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
            return new SuccessDataResult<SelectEmployeesDto>(employees);
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            DeleteControl.ControlList.Clear();

            DeleteControl.ControlList.Add("AdjustmentUserId", new List<string>
            {
                Tables.OperationAdjustments
            });

            DeleteControl.ControlList.Add("EmployeeID", new List<string>
            {
                Tables.ContractProductionTrackings,
                Tables.EmployeeOperations,
                Tables.EmployeeScoringLines,
                Tables.FinalControlUnsuitabilityReports,
                Tables.FirstProductApprovals,
                Tables.OperationUnsuitabilityReports,
                Tables.ProductionTrackings
            });

            bool control = DeleteControl.Control(queryFactory, id);

            if (!control)
            {
                throw new Exception(L["DeleteControlManager"]);
            }
            else
            {
                var entity = (await GetAsync(id)).Data;
                var query = queryFactory.Query().From(Tables.Employees).Delete(LoginedUserService.UserId).Where(new { Id = id }, "");

                var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Employees, LogType.Delete, id);
                #region Notification

                var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeesChildMenu"], L["ProcessDelete"])).Data.FirstOrDefault();

                if (notTemplate != null && notTemplate.Id != Guid.Empty)
                {
                    if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                    {
                        if (notTemplate.TargetUsersId.Contains(","))
                        {
                            string[] usersNot = notTemplate.TargetUsersId.Split(',');

                            foreach (string user in usersNot)
                            {
                                CreateNotificationsDto createInput = new CreateNotificationsDto
                                {
                                    ContextMenuName_ = notTemplate.ContextMenuName_,
                                    IsViewed = false,
                                    Message_ = notTemplate.Message_,
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
                                Message_ = notTemplate.Message_,
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
                return new SuccessDataResult<SelectEmployeesDto>(employees);
            }
        }


        public async Task<IDataResult<SelectEmployeesDto>> GetAsync(Guid id)
        {
            var query = queryFactory
                    .Query().From(Tables.Employees).Select<Employees>(null)
                        .Join<Departments>
                        (
                            d => new { Department = d.Name, DepartmentID = d.Id },
                            nameof(Employees.DepartmentID),
                            nameof(Departments.Id),
                            JoinType.Left
                        )
                          .Join<EmployeeSeniorities>
                        (
                            d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                            nameof(Employees.SeniorityID),
                            nameof(EmployeeSeniorities.Id),
                            JoinType.Left
                        )
                            .Join<EducationLevelScores>
                        (
                            d => new { EducationLevelName = d.Name, EducationLevelID = d.Id },
                            nameof(Employees.EducationLevelID),
                            nameof(EducationLevelScores.Id),
                            JoinType.Left
                        )
                        .Where(new { Id = id }, Tables.Employees);

            var employee = queryFactory.Get<SelectEmployeesDto>(query);

            LogsAppService.InsertLogToDatabase(employee, employee, LoginedUserService.UserId, Tables.Employees, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeesDto>(employee);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListAsync(ListEmployeesParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Employees)
               .Select<Employees>(s => new { s.Code, s.Name, s.Email, s.Id })
                   .Join<Departments>
                   (
                       d => new { Department = d.Name },
                         nameof(Employees.DepartmentID),
                         nameof(Departments.Id),
                         JoinType.Left
                   )

                     .Join<EmployeeSeniorities>
                   (
                       d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                       nameof(Employees.SeniorityID),
                       nameof(EmployeeSeniorities.Id),
                       JoinType.Left
                   )
                       .Join<EducationLevelScores>
                   (
                       d => new { EducationLevelName = d.Name, EducationLevelID = d.Id },
                       nameof(Employees.EducationLevelID),
                       nameof(EducationLevelScores.Id),
                       JoinType.Left
                   ).Where(null, Tables.Employees);

            var employees = queryFactory.GetList<ListEmployeesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEmployeesDto>>(employees);
        }


        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListbyDepartmentAsync(Guid departmentID)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Employees)
               .Select<Employees>(s => new { s.Code, s.Name, s.Id })
                   .Join<Departments>
                   (
                       d => new { Department = d.Name },
                         nameof(Employees.DepartmentID),
                         nameof(Departments.Id),
                         JoinType.Left
                   )

                     .Join<EmployeeSeniorities>
                   (
                       d => new { SeniorityName = d.Name, SeniorityID = d.Id },
                       nameof(Employees.SeniorityID),
                       nameof(EmployeeSeniorities.Id),
                       JoinType.Left
                   )
                       .Join<EducationLevelScores>
                   (
                       d => new { EducationLevelName = d.Name, EducationLevelID = d.Id },
                       nameof(Employees.EducationLevelID),
                       nameof(EducationLevelScores.Id),
                       JoinType.Left
                   ).Where(new { DepartmentID = departmentID }, Tables.Employees);

            var employees = queryFactory.GetList<ListEmployeesDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListEmployeesDto>>(employees);
        }


        [ValidationAspect(typeof(UpdateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> UpdateAsync(UpdateEmployeesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = input.Id }, "");
            var entity = queryFactory.Get<Employees>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Code = input.Code }, "");
            var list = queryFactory.GetList<Employees>(listQuery).ToList();

            if (list.Count > 0 && entity.Code != input.Code)
            {
                throw new DuplicateCodeException(L["UpdateControlManager"]);
            }

            #endregion

            var query = queryFactory.Query().From(Tables.Employees).Update(new UpdateEmployeesDto
            {
                Code = input.Code,
                Name = input.Name,
                Id = input.Id,
                TaskDefinition = input.TaskDefinition,
                SeniorityID = input.SeniorityID.GetValueOrDefault(),
                HiringDate = input.HiringDate,
                EducationLevelID = input.EducationLevelID.GetValueOrDefault(),
                CurrentSalary = input.CurrentSalary,
                Address = input.Address,
                Birthday = input.Birthday,
                BloodType = input.BloodType,
                IDnumber = input.IDnumber,
                CellPhone = input.CellPhone,
                City = input.City,
                DepartmentID = input.DepartmentID.GetValueOrDefault(),
                District = input.District,
                Email = input.Email,
                HomePhone = input.HomePhone,
                Surname = input.Surname,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = _GetSQLDateAppService.GetDateFromSQL(),
                LastModifierId = LoginedUserService.UserId,
                IsProductionScreenUser = input.IsProductionScreenUser,
                ProductionScreenPassword = input.ProductionScreenPassword,
                IsProductionScreenSettingUser = input.IsProductionScreenSettingUser
            }).Where(new { Id = input.Id }, "");

            var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, employees, LoginedUserService.UserId, Tables.Employees, LogType.Update, entity.Id);
            #region Notification

            var notTemplate = (await _NotificationTemplatesAppService.GetListbyModuleProcessAsync(L["EmployeesChildMenu"], L["ProcessRefresh"])).Data.FirstOrDefault();

            if (notTemplate != null && notTemplate.Id != Guid.Empty)
            {
                if (!string.IsNullOrEmpty(notTemplate.TargetUsersId))
                {
                    if (notTemplate.TargetUsersId.Contains(","))
                    {
                        string[] usersNot = notTemplate.TargetUsersId.Split(',');

                        foreach (string user in usersNot)
                        {
                            CreateNotificationsDto createInput = new CreateNotificationsDto
                            {
                                ContextMenuName_ = notTemplate.ContextMenuName_,
                                IsViewed = false,
                                Message_ = notTemplate.Message_,
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
                            Message_ = notTemplate.Message_,
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

            return new SuccessDataResult<SelectEmployeesDto>(employees);
        }

        public async Task<IDataResult<SelectEmployeesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = id }, "");
            var entity = queryFactory.Get<Employees>(entityQuery);

            var query = queryFactory.Query().From(Tables.Periods).Update(new UpdateEmployeesDto
            {
                Code = entity.Code,
                Name = entity.Name,
                Surname = entity.Surname,
                HomePhone = entity.HomePhone,
                CurrentSalary = entity.CurrentSalary,
                EducationLevelID = entity.EducationLevelID,
                HiringDate = entity.HiringDate,
                SeniorityID = entity.SeniorityID,
                TaskDefinition = entity.TaskDefinition,
                Email = entity.Email,
                District = entity.District,
                DepartmentID = entity.DepartmentID,
                Address = entity.Address,
                Birthday = entity.Birthday,
                BloodType = (int)entity.BloodType,
                CellPhone = entity.CellPhone,
                City = entity.City,
                IDnumber = entity.IDnumber,
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
                IsProductionScreenUser = entity.IsProductionScreenUser,
                ProductionScreenPassword = entity.ProductionScreenPassword,
                IsProductionScreenSettingUser = entity.IsProductionScreenSettingUser
            }, UpdateType.ConcurrencyUpdate).Where(new { Id = id }, "");

            var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectEmployeesDto>(employees);
        }
    }
}
