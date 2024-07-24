using Microsoft.Extensions.Localization;
using System.Reflection;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using Tsi.Core.Utilities.VersionUtilities;
using TSI.QueryBuilder.BaseClasses;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.DataAccess.DatabaseSchemeHistories;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Version;
using TsiErp.Entities.Entities.GeneralSystemIdentifications.Version.Dtos;
using TsiErp.Entities.TableConstant;
using TsiErp.Localizations.Resources.Periods.Page;

namespace TsiErp.Business.Entities.ProgVersion.Services
{
    [ServiceRegistration(typeof(IProgVersionsAppService), DependencyInjectionType.Scoped)]
    public class ProgVersionsAppService : ApplicationService<PeriodsResource>, IProgVersionsAppService
    {
        QueryFactory queryFactory { get; set; } = new QueryFactory();

        public ProgVersionsAppService(IStringLocalizer<PeriodsResource> l) : base(l)
        {
        }

        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectProgVersionsDto>> CreateAsync(CreateProgVersionsDto input)
        {
            await Task.CompletedTask;
            throw new NotImplementedException("Ekleme işlemi bu servis için yapılamaz.");
        }

        public Task<IResult> DeleteAsync(Guid id)
        {
            throw new NotImplementedException("Silme işlemi bu servis için yapılamaz.");
        }

        public async Task<IDataResult<SelectProgVersionsDto>> GetAsync(Guid id)
        {
            id = LoginedUserService.VersionTableId;

            var query = queryFactory.Query().From(Tables.ProgVersions).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "").UseIsDelete(false);

            var version = queryFactory.Get<SelectProgVersionsDto>(query);


            LogsAppService.InsertLogToDatabase(version, version, LoginedUserService.UserId, Tables.ProgVersions, LogType.Get, id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProgVersionsDto>(version);
        }

        public Task<IDataResult<IList<ListProgVersionsDto>>> GetListAsync(ListProgVersionsParameterDto input)
        {
            throw new NotImplementedException("Listeleme işlemi bu servis için yapılamaz.");
        }

        public async Task<IDataResult<SelectProgVersionsDto>> UpdateAsync(UpdateProgVersionsDto input)
        {
            input.Id = LoginedUserService.VersionTableId;

            var entityQuery = queryFactory.Query().From(Tables.ProgVersions).Select("*").Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);
            var entity = queryFactory.Get<ProgVersions>(entityQuery);

            var query = queryFactory.Query().From(Tables.ProgVersions).Update(new UpdateProgVersionsDto
            {

                Id = input.Id,
                MinDbVersion = input.MinDbVersion,
                BuildVersion = input.BuildVersion,
                MajDbVersion = input.MajDbVersion,
                IsUpdating = input.IsUpdating

            }).Where(new { Id = input.Id }, false, false, "").UseIsDelete(false);

            var version = queryFactory.Update<SelectProgVersionsDto>(query, "Id", true);

            LogsAppService.InsertLogToDatabase(entity, version, LoginedUserService.UserId, Tables.ProgVersions, LogType.Update, entity.Id);

            await Task.CompletedTask;
            return new SuccessDataResult<SelectProgVersionsDto>(version);
        }

        public Task<IDataResult<SelectProgVersionsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckVersion(string progVersion)
        {
            Guid id = LoginedUserService.VersionTableId;

            var query = queryFactory.Query().From(Tables.ProgVersions).Select("*").Where(
            new
            {
                Id = id
            }, false, false, "").UseIsDelete(false);

            var version = queryFactory.Get<SelectProgVersionsDto>(query);

            string dbVersion = version.MajDbVersion + "." + version.MinDbVersion + "." + version.BuildVersion;

            if (dbVersion != progVersion)
            {
                var updatedVersionIsRuning = new UpdateProgVersionsDto
                {
                    Id = id,
                    BuildVersion = version.BuildVersion,
                    IsUpdating = true,
                    MajDbVersion = version.MajDbVersion,
                    MinDbVersion = version.MinDbVersion
                };

                await UpdateAsync(updatedVersionIsRuning);

                await UpdateDatabase(progVersion);

                var updatedVersion = new UpdateProgVersionsDto
                {
                    Id = id,
                    BuildVersion = progVersion.Split('.')[2],
                    IsUpdating = false,
                    MajDbVersion = progVersion.Split(".")[0],
                    MinDbVersion = progVersion.Split(".")[1]
                };

                await UpdateAsync(updatedVersion);

                return updatedVersion.BuildVersion == progVersion.Split('.')[2] ? true : false;


            }

            return true;
        }

        public async Task<bool> UpdateDatabase(string versionToBeUpdated)
        {
            DatabaseSchemeUpdater databaseSchemeUpdater = new DatabaseSchemeUpdater();

            var methods = databaseSchemeUpdater.GetType().GetMethods();

            foreach (var item in methods)
            {
                if (item.GetCustomAttributes<VersionAttribute>().Count() > 0)
                {
                    var methodVersionNumber = item.GetCustomAttribute<VersionAttribute>().VersiyonNumber;

                    if (methodVersionNumber == versionToBeUpdated)
                    {
                        var executeMethod = (bool)item.Invoke(databaseSchemeUpdater, null);
                    }
                }
            }

            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> TableAddColumn()
        {
            DatabaseSchemeUpdater databaseSchemeUpdater = new DatabaseSchemeUpdater();

            databaseSchemeUpdater.TableAddColumn();
            await Task.CompletedTask;
            return true;
        }
    }
}
