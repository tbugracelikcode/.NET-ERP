﻿using Microsoft.Extensions.Localization;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.Other.GetSQLDate.Services;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.MachineAndWorkforceManagementParameter.Page;

namespace TsiErp.Business.Entities.GeneralSystemIdentifications.MachineAndWorkforceManagementParameter.Services
{
    [ServiceRegistration(typeof(IMachineAndWorkforceManagementParametersAppService), DependencyInjectionType.Scoped)]
    public class MachineAndWorkforceManagementParametersAppService : ApplicationService<MachineAndWorkforceManagementParametersResource>, IMachineAndWorkforceManagementParametersAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();
        private readonly IGetSQLDateAppService _GetSQLDateAppService;

        public MachineAndWorkforceManagementParametersAppService(IStringLocalizer<MachineAndWorkforceManagementParametersResource> l, IGetSQLDateAppService getSQLDateAppService) : base(l)
        {
            _GetSQLDateAppService = getSQLDateAppService;
        }

        public async Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> GetAsync(Guid id)
        {
            var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(
             new
             {
                 Id = id
             }, "");

            var MachineAndWorkforceManagementParameter = queryFactory.Get<SelectMachineAndWorkforceManagementParametersDto>(query);

            //LogsAppService.InsertLogToDatabase(MachineAndWorkforceManagementParameter, MachineAndWorkforceManagementParameter, LoginedUserService.UserId, Tables.MachineAndWorkforceManagementParameters, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectMachineAndWorkforceManagementParametersDto>(MachineAndWorkforceManagementParameter);

        }

        public async Task<IDataResult<IList<ListMachineAndWorkforceManagementParametersDto>>> GetListAsync(ListMachineAndWorkforceManagementParametersParameterDto input)
        {
            var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(null, "");

            var MachineAndWorkforceManagementParameters = queryFactory.GetList<ListMachineAndWorkforceManagementParametersDto>(query).ToList();

            await Task.CompletedTask;
            return new SuccessDataResult<IList<ListMachineAndWorkforceManagementParametersDto>>(MachineAndWorkforceManagementParameters);
        }

        public async Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> UpdateAsync(UpdateMachineAndWorkforceManagementParametersDto input)
        {
            var entityQuery = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Select("*").Where(new { Id = input.Id },  "");
            var entity = queryFactory.Get<MachineAndWorkforceManagementParameters>(entityQuery);

            var query = queryFactory.Query().From(Tables.MachineAndWorkforceManagementParameters).Update(new UpdateMachineAndWorkforceManagementParametersDto
            {
                FutureDateParameter = input.FutureDateParameter,
                Id = input.Id
            }).Where(new { Id = input.Id }, "");

            var MachineAndWorkforceManagementParameters = queryFactory.Update<SelectMachineAndWorkforceManagementParametersDto>(query, "Id", true);


            //LogsAppService.InsertLogToDatabase(entity, MachineAndWorkforceManagementParameters, LoginedUserService.UserId, Tables.MachineAndWorkforceManagementParameters, LogType.Update, entity.Id);


            await Task.CompletedTask;
            return new SuccessDataResult<SelectMachineAndWorkforceManagementParametersDto>(MachineAndWorkforceManagementParameters);

        }

        #region Unused Implemented Methods

        public Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> CreateAsync(CreateMachineAndWorkforceManagementParametersDto input)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResult<SelectMachineAndWorkforceManagementParametersDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
