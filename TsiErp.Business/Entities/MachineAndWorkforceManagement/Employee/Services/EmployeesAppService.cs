using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TSI.QueryBuilder.Constants.Join;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Employee.Validations;
using TsiErp.Business.Entities.GeneralSystemIdentifications.FicheNumber.Services;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.DeleteControlExtension;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Department;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EducationLevelScore;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.Employee.Dtos;
using TsiErp.Entities.Entities.MachineAndWorkforceManagement.EmployeeSeniority;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Employees.Page;

namespace TsiErp.Business.Entities.Employee.Services
{
    [ServiceRegistration(typeof(IEmployeesAppService), DependencyInjectionType.Scoped)]
    public class EmployeesAppService : ApplicationService<EmployeesResource>, IEmployeesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        private IFicheNumbersAppService FicheNumbersAppService { get; set; }

        public EmployeesAppService(IStringLocalizer<EmployeesResource> l, IFicheNumbersAppService ficheNumbersAppService) : base(l)
        {
            FicheNumbersAppService = ficheNumbersAppService;
        }


        [ValidationAspect(typeof(CreateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> CreateAsync(CreateEmployeesDto input)
        {
            var listQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Code = input.Code }, false, false, "");

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
                EducationLevelID = input.EducationLevelID,
                HiringDate = input.HiringDate,
                SeniorityID = input.SeniorityID,
                TaskDefinition = input.TaskDefinition,
                BloodType = (int)input.BloodType,
                CellPhone = input.CellPhone,
                City = input.City,
                DepartmentID = input.DepartmentID,
                District = input.District,
                Email = input.Email,
                HomePhone = input.HomePhone,
                IDnumber = input.IDnumber,
                Surname = input.Surname,
                CreationTime = DateTime.Now,
                CreatorId = LoginedUserService.UserId,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = Guid.Empty,
                DeletionTime = null,
                Id = addedEntityId,
                IsActive = true,
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
                var query = queryFactory.Query().From(Tables.Employees).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Employees, LogType.Delete, id);

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
                        .Where(new { Id = id }, true, true, Tables.Employees);

            var employee = queryFactory.Get<SelectEmployeesDto>(query);

            LogsAppService.InsertLogToDatabase(employee, employee, LoginedUserService.UserId, Tables.Employees, LogType.Get, id);

            return new SuccessDataResult<SelectEmployeesDto>(employee);
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListAsync(ListEmployeesParameterDto input)
        {
            var query = queryFactory
               .Query()
               .From(Tables.Employees)
               .Select<Employees>(null)
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
                   ).Where(null, true, true, Tables.Employees);

            var employees = queryFactory.GetList<ListEmployeesDto>(query).ToList();

            return new SuccessDataResult<IList<ListEmployeesDto>>(employees);
        }


        [ValidationAspect(typeof(UpdateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> UpdateAsync(UpdateEmployeesDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = input.Id }, true, true, "");
            var entity = queryFactory.Get<Employees>(entityQuery);

            #region Update Control

            var listQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Code = input.Code }, false, false, "");
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
                SeniorityID = input.SeniorityID,
                HiringDate = input.HiringDate,
                EducationLevelID = input.EducationLevelID,
                CurrentSalary = input.CurrentSalary,
                Address = input.Address,
                Birthday = input.Birthday,
                BloodType = input.BloodType,
                IDnumber = input.IDnumber,
                CellPhone = input.CellPhone,
                City = input.City,
                DepartmentID = input.DepartmentID,
                District = input.District,
                Email = input.Email,
                HomePhone = input.HomePhone,
                Surname = input.Surname,
                IsActive = input.IsActive,
                CreationTime = entity.CreationTime.Value,
                CreatorId = entity.CreatorId.Value,
                DataOpenStatus = false,
                DataOpenStatusUserId = Guid.Empty,
                DeleterId = entity.DeleterId.GetValueOrDefault(),
                DeletionTime = entity.DeletionTime.GetValueOrDefault(),
                IsDeleted = entity.IsDeleted,
                LastModificationTime = DateTime.Now,
                LastModifierId = LoginedUserService.UserId,
                IsProductionScreenUser = input.IsProductionScreenUser,
                ProductionScreenPassword = input.ProductionScreenPassword,
                IsProductionScreenSettingUser = input.IsProductionScreenSettingUser
            }).Where(new { Id = input.Id }, true, true, "");

            var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);


            LogsAppService.InsertLogToDatabase(entity, employees, LoginedUserService.UserId, Tables.Employees, LogType.Update, entity.Id);


            return new SuccessDataResult<SelectEmployeesDto>(employees);
        }

        public async Task<IDataResult<SelectEmployeesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = id }, true, true, "");
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
                IsActive = entity.IsActive,
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
            }).Where(new { Id = id }, true, true, "");

            var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

            return new SuccessDataResult<SelectEmployeesDto>(employees);
        }
    }
}
