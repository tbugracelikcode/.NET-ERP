using AutoMapper.Internal.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.HaltReason.BusinessRules;
using TsiErp.Business.Entities.HaltReason.Validations;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.HaltReason;
using TsiErp.Entities.Entities.HaltReason;
using TsiErp.Entities.Entities.HaltReason.Dtos;
using TsiErp.Entities.Entities.WorkOrder.Dtos;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.HaltReason.Services
{
    [ServiceRegistration(typeof(IHaltReasonsAppService), DependencyInjectionType.Scoped)]
    public class HaltReasonsAppService : IHaltReasonsAppService
    {
        HaltReasonManager _manager { get; set; } = new HaltReasonManager();

        [ValidationAspect(typeof(CreateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> CreateAsync(CreateHaltReasonsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.HaltReasonsRepository, input.Code);

                var entity = ObjectMapper.Map<CreateHaltReasonsDto, HaltReasons>(input);

                var addedEntity = await _uow.HaltReasonsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectHaltReasonsDto>(ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(addedEntity));
            }
        }

        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _uow.HaltReasonsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }

        public async Task<IDataResult<SelectHaltReasonsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.HaltReasonsRepository.GetAsync(t => t.Id == id);
                var mappedEntity = ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(entity);
                return new SuccessDataResult<SelectHaltReasonsDto>(mappedEntity);
            }
        }

        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListHaltReasonsDto>>> GetListAsync(ListHaltReasonsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.HaltReasonsRepository.GetListAsync();

                var mappedEntity = ObjectMapper.Map<List<HaltReasons>, List<ListHaltReasonsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListHaltReasonsDto>>(mappedEntity);
            }
        }

        [ValidationAspect(typeof(UpdateHaltReasonsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateAsync(UpdateHaltReasonsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.HaltReasonsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.HaltReasonsRepository, input.Code, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateHaltReasonsDto, HaltReasons>(input);

                await _uow.HaltReasonsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();
                return new SuccessDataResult<SelectHaltReasonsDto>(ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(mappedEntity));
            }
        }

        public async Task<IDataResult<SelectHaltReasonsDto>> UpdateConcurrencyFieldsAsync(Guid id, bool lockRow, Guid userId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.HaltReasonsRepository.GetAsync(x => x.Id == id);

                var updatedEntity = await _uow.HaltReasonsRepository.LockRow(entity.Id, lockRow, userId);

                await _uow.SaveChanges();

                var mappedEntity = ObjectMapper.Map<HaltReasons, SelectHaltReasonsDto>(updatedEntity);

                return new SuccessDataResult<SelectHaltReasonsDto>(mappedEntity);
            }
        }
    }
}
