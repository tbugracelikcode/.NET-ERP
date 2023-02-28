using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results; using TsiErp.Localizations.Resources.Branches.Page;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.BusinessCoreServices;
using TsiErp.Business.Entities.Logging.Services;
using TsiErp.Business.Entities.TemplateOperation.BusinessRules;
using TsiErp.Business.Entities.TemplateOperation.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.TemplateOperation;
using TsiErp.Entities.Entities.TemplateOperation.Dtos;
using TsiErp.Entities.Entities.TemplateOperationLine;
using TsiErp.Entities.Entities.TemplateOperationLine.Dtos;
using Microsoft.Extensions.Localization;

namespace TsiErp.Business.Entities.TemplateOperation.Services
{
    [ServiceRegistration(typeof(ITemplateOperationsAppService), DependencyInjectionType.Scoped)]
    public class TemplateOperationsAppService : ApplicationService<BranchesResource>, ITemplateOperationsAppService
    {
        public TemplateOperationsAppService(IStringLocalizer<BranchesResource> l) : base(l)
        {
        }

        TemplateOperationManager _manager { get; set; } = new TemplateOperationManager();

        [ValidationAspect(typeof(CreateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> CreateAsync(CreateTemplateOperationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.TemplateOperationsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateTemplateOperationsDto, TemplateOperations>(input);

                var addedEntity = await _uow.TemplateOperationsRepository.InsertAsync(entity);

                foreach (var item in input.SelectTemplateOperationLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectTemplateOperationLinesDto, TemplateOperationLines>(item);
                    lineEntity.TemplateOperationID = addedEntity.Id;
                    await _uow.TemplateOperationLinesRepository.InsertAsync(lineEntity);
                }
                input.Id = addedEntity.Id;
                var log = LogsAppService.InsertLogToDatabase(input, input, LoginedUserService.UserId, "TemplateOperations", LogType.Insert, addedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectTemplateOperationsDto>(ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var lines = (await _uow.TemplateOperationLinesRepository.GetAsync(t => t.Id == id));

                if (lines != null)
                {
                    await _manager.DeleteControl(_uow.TemplateOperationsRepository, lines.TemplateOperationID, lines.Id, true);
                    await _uow.TemplateOperationLinesRepository.DeleteAsync(id);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
                else
                {
                    await _manager.DeleteControl(_uow.TemplateOperationsRepository, id, Guid.Empty, false);

                    var list = (await _uow.TemplateOperationLinesRepository.GetListAsync(t => t.TemplateOperationID == id));
                    foreach (var line in list)
                    {
                        await _uow.TemplateOperationLinesRepository.DeleteAsync(line.Id);


                    }
                    await _uow.TemplateOperationsRepository.DeleteAsync(id);
                    var log = LogsAppService.InsertLogToDatabase(id, id, LoginedUserService.UserId, "TemplateOperations", LogType.Delete, id);
                    await _uow.LogsRepository.InsertAsync(log);
                    await _uow.SaveChanges();
                    return new SuccessResult("Silme işlemi başarılı.");
                }
            }
        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.TemplateOperationsRepository.GetAsync(t => t.Id == id,
                t => t.TemplateOperationLines);

                var mappedEntity = ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(entity);

                mappedEntity.SelectTemplateOperationLines = ObjectMapper.Map<List<TemplateOperationLines>, List<SelectTemplateOperationLinesDto>>(entity.TemplateOperationLines.ToList());

                foreach (var item in mappedEntity.SelectTemplateOperationLines)
                {
                    item.StationCode = (await _uow.StationsRepository.GetAsync(t => t.Id == item.StationID)).Code;
                    item.StationName = (await _uow.StationsRepository.GetAsync(t => t.Id == item.StationID)).Name;
                }
                var log = LogsAppService.InsertLogToDatabase(mappedEntity, mappedEntity, LoginedUserService.UserId, "TemplateOperations", LogType.Get, id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectTemplateOperationsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTemplateOperationsDto>>> GetListAsync(ListTemplateOperationsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.TemplateOperationsRepository.GetListAsync(null,
                t => t.TemplateOperationLines);

                var mappedEntity = ObjectMapper.Map<List<TemplateOperations>, List<ListTemplateOperationsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListTemplateOperationsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateTemplateOperationsValidatorDto), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateAsync(UpdateTemplateOperationsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.TemplateOperationsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.TemplateOperationsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateTemplateOperationsDto, TemplateOperations>(input);

                await _uow.TemplateOperationsRepository.UpdateAsync(mappedEntity);

                foreach (var item in input.SelectTemplateOperationLines)
                {
                    var lineEntity = ObjectMapper.Map<SelectTemplateOperationLinesDto, TemplateOperationLines>(item);
                    lineEntity.TemplateOperationID = mappedEntity.Id;

                    if (lineEntity.Id == Guid.Empty)
                    {
                        await _uow.TemplateOperationLinesRepository.InsertAsync(lineEntity);
                    }
                    else
                    {
                        await _uow.TemplateOperationLinesRepository.UpdateAsync(lineEntity);
                    }
                }
                var before = ObjectMapper.Map<TemplateOperations, UpdateTemplateOperationsDto>(entity);
                before.SelectTemplateOperationLines = ObjectMapper.Map<List<TemplateOperationLines>, List<SelectTemplateOperationLinesDto>>(entity.TemplateOperationLines.ToList());
                var log = LogsAppService.InsertLogToDatabase(before, input, LoginedUserService.UserId, "TemplateOperations", LogType.Update, mappedEntity.Id);
                await _uow.LogsRepository.InsertAsync(log);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectTemplateOperationsDto>(ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectTemplateOperationsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.TemplateOperationsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.TemplateOperationsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<TemplateOperations, SelectTemplateOperationsDto>(updatedEntity);

                return new SuccessDataResult<SelectTemplateOperationsDto>(mappedEntity);
            }
        }
    }
}
