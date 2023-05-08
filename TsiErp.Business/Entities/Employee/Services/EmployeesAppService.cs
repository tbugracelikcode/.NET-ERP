using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using TsiErp.Localizations.Resources.Employees.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Employee.BusinessRules;
using TsiErp.Business.Entities.Employee.Validations;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Employee;
using TsiErp.Entities.Entities.Employee.Dtos;
using Microsoft.Extensions.Localization;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Entities.TableConstant;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.Entities.Entities.Department;
using TSI.QueryBuilder.Constants.Join;

namespace TsiErp.Business.Entities.Employee.Services
{
    [ServiceRegistration(typeof(IEmployeesAppService), DependencyInjectionType.Scoped)]
    public class EmployeesAppService : ApplicationService<EmployeesResource>, IEmployeesAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public EmployeesAppService(IStringLocalizer<EmployeesResource> l) : base(l)
        {
        }


        [ValidationAspect(typeof(CreateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> CreateAsync(CreateEmployeesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var listQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Code = input.Code }, false, false, "");

                var list = queryFactory.ControlList<Employees>(listQuery).ToList();

                #region Code Control 

                if (list.Count > 0)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["CodeControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Employees).Insert(new CreateEmployeesDto
                {
                    Code = input.Code,
                    Address = input.Address,
                    Birthday = input.Birthday,
                    BloodType = input.BloodType,
                    CellPhone = input.CellPhone,
                    City = input.City,
                    Department = input.Department,
                    DepartmentID = input.DepartmentID,
                    District = input.District,
                    Email = input.Email,
                    HomePhone = input.HomePhone,
                    IDnumber = input.IDnumber,
                    Image = input.Image,
                    Surname = input.Surname,
                    CreationTime = DateTime.Now,
                    CreatorId = LoginedUserService.UserId,
                    DataOpenStatus = false,
                    DataOpenStatusUserId = Guid.Empty,
                    DeleterId = Guid.Empty,
                    DeletionTime = null,
                    Id = GuidGenerator.CreateGuid(),
                    IsActive = true,
                    IsDeleted = false,
                    LastModificationTime = null,
                    LastModifierId = Guid.Empty,
                    Name = input.Name
                });

                var employees = queryFactory.Insert<SelectEmployeesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, Tables.Employees, LogType.Insert, employees.Id);

                return new SuccessDataResult<SelectEmployeesDto>(employees);
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory.Query().From(Tables.Employees).Delete(LoginedUserService.UserId).Where(new { Id = id }, true, true, "");

                var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

                LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, Tables.Employees, LogType.Delete, id);

                return new SuccessDataResult<SelectEmployeesDto>(employees);
            }
        }


        public async Task<IDataResult<SelectEmployeesDto>> GetAsync(Guid id)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var query = queryFactory
                        .Query().From(Tables.Employees).Select<Employees>(e => new { e.Surname, e.Birthday, e.BloodType, e.CellPhone, e.HomePhone, e.City, e.Name, e.Address, e.Code, e.DataOpenStatus, e.DataOpenStatusUserId, e.DepartmentID, e.District, e.Email, e.Id, e.IDnumber, e.IsActive, e.Image })
                            .Join<Departments>
                            (
                                d => new { Department = d.Name, DepartmentID = d.Id },
                                nameof(Employees.DepartmentID),
                                nameof(Departments.Id),
                                JoinType.Left
                            )
                            .Where(new { Id = id }, true, true, Tables.Employees);

                var employee = queryFactory.Get<SelectEmployeesDto>(query);

                LogsAppService.InsertLogToDatabase(employee, employee, LoginedUserService.UserId, Tables.Employees, LogType.Get, id);

                return new SuccessDataResult<SelectEmployeesDto>(employee);

            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListEmployeesDto>>> GetListAsync(ListEmployeesParameterDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {

                var query = queryFactory
                   .Query()
                   .From(Tables.Employees)
                   .Select<Employees>(e => new { e.Surname, e.Birthday, e.BloodType, e.CellPhone, e.HomePhone, e.City, e.Name, e.Address, e.Code, e.DataOpenStatus, e.DataOpenStatusUserId, e.DepartmentID, e.District, e.Email, e.Id, e.IDnumber, e.IsActive, e.Image })
                       .Join<Departments>
                       (
                           d => new { Department = d.Name },
                             nameof(Employees.DepartmentID),
                             nameof(Departments.Id),
                             JoinType.Left
                       ).Where(null, true, true, Tables.Employees);

                var employees = queryFactory.GetList<ListEmployeesDto>(query).ToList();

                return new SuccessDataResult<IList<ListEmployeesDto>>(employees);
            }
        }


        [ValidationAspect(typeof(UpdateEmployeesValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectEmployeesDto>> UpdateAsync(UpdateEmployeesDto input)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = input.Id }, true, true, "");
                var entity = queryFactory.Get<Employees>(entityQuery);

                #region Update Control

                var listQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Code = input.Code }, false, false, "");
                var list = queryFactory.GetList<Employees>(listQuery).ToList();

                if (list.Count > 0 && entity.Code != input.Code)
                {
                    connection.Close();
                    connection.Dispose();
                    throw new DuplicateCodeException(L["UpdateControlManager"]);
                }

                #endregion

                var query = queryFactory.Query().From(Tables.Employees).Update(new UpdateEmployeesDto
                {
                    Code = input.Code,
                    Name = input.Name,
                    Id = input.Id,
                    Address = input.Address,
                    Birthday = input.Birthday,
                    Image = input.Image,
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
                    DeleterId = entity.DeleterId.Value,
                    DeletionTime = entity.DeletionTime.Value,
                    IsDeleted = entity.IsDeleted,
                    LastModificationTime = DateTime.Now,
                    LastModifierId = LoginedUserService.UserId
                }).Where(new { Id = input.Id }, true, true, "");

                var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);


                LogsAppService.InsertLogToDatabase(entity, employees, LoginedUserService.UserId, Tables.Employees, LogType.Update, entity.Id);


                return new SuccessDataResult<SelectEmployeesDto>(employees);
            }
        }

        public async Task<IDataResult<SelectEmployeesDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (var connection = queryFactory.ConnectToDatabase())
            {
                var entityQuery = queryFactory.Query().From(Tables.Employees).Select("*").Where(new { Id = id }, true, true, "");
                var entity = queryFactory.Get<Employees>(entityQuery);

                var query = queryFactory.Query().From(Tables.Periods).Update(new UpdateEmployeesDto
                {
                    Code = entity.Code,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    HomePhone = entity.HomePhone,
                    Email = entity.Email,
                    District = entity.District,
                    DepartmentID = entity.DepartmentID,
                    Address = entity.Address,
                    Birthday = entity.Birthday,
                    BloodType = entity.BloodType,
                    CellPhone = entity.CellPhone,
                    City = entity.City,
                    IDnumber = entity.IDnumber,
                    Image = entity.Image,
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

                }).Where(new { Id = id }, true, true, "");

                var employees = queryFactory.Update<SelectEmployeesDto>(query, "Id", true);

                return new SuccessDataResult<SelectEmployeesDto>(employees);

            }
        }
    }
}
