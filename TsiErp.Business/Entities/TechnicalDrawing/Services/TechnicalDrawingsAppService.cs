using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Application.Contract.Services.EntityFrameworkCore;
using Tsi.Core.Aspects.Autofac.Caching;
using Tsi.Core.Aspects.Autofac.Validation;
using Tsi.Core.Utilities.Results;
using Tsi.Core.Utilities.Services.Business.ServiceRegistrations;
using TsiErp.Business.Entities.Branch.Services;
using TsiErp.Business.Entities.Branch.Validations;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.Branch;
using TsiErp.Entities.Entities.Branch.Dtos;
using TsiErp.Entities.Entities.Branch;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing;
using TsiErp.Business.Entities.TechnicalDrawing.Validations;
using TsiErp.Entities.Entities.TechnicalDrawing.Dtos;
using TsiErp.Business.Extensions.ObjectMapping;
using TsiErp.Entities.Entities.TechnicalDrawing;
using TsiErp.Business.Entities.TechnicalDrawing.BusinessRules;
using TsiErp.DataAccess.EntityFrameworkCore.EfUnitOfWork;

namespace TsiErp.Business.Entities.TechnicalDrawing.Services
{
    [ServiceRegistration(typeof(ITechnicalDrawingsAppService), DependencyInjectionType.Scoped)]
    public class TechnicalDrawingsAppService : ApplicationService, ITechnicalDrawingsAppService
    {
        TechnicalDrawingManager _manager { get; set; } = new TechnicalDrawingManager();

        [ValidationAspect(typeof(CreateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> CreateAsync(CreateTechnicalDrawingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.CodeControl(_uow.TechnicalDrawingsRepository, input.RevisionNo);

                var entity = ObjectMapper.Map<CreateTechnicalDrawingsDto, TechnicalDrawings>(input);

                var addedEntity = await _uow.TechnicalDrawingsRepository.InsertAsync(entity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectTechnicalDrawingsDto>(ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(addedEntity));
            }
        }


        [CacheRemoveAspect("Get")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                await _manager.DeleteControl(_uow.TechnicalDrawingsRepository, id);
                await _uow.TechnicalDrawingsRepository.DeleteAsync(id);
                await _uow.SaveChanges();
                return new SuccessResult("Silme işlemi başarılı.");
            }
        }


        public async Task<IDataResult<SelectTechnicalDrawingsDto>> GetAsync(Guid id)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.TechnicalDrawingsRepository.GetAsync(t => t.Id == id, t => t.Products);
                var mappedEntity = ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(entity);
                return new SuccessDataResult<SelectTechnicalDrawingsDto>(mappedEntity);
            }
        }


        [CacheAspect(duration: 60)]
        public async Task<IDataResult<IList<ListTechnicalDrawingsDto>>> GetListAsync(ListTechnicalDrawingsParameterDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                IList<TechnicalDrawings> list;

                if (input.ProductId == null)
                {
                    list = await _uow.TechnicalDrawingsRepository.GetListAsync(null, t => t.Products);
                }
                else
                {
                    list = await _uow.TechnicalDrawingsRepository.GetListAsync(t => t.ProductID == input.ProductId, t => t.Products);
                }


                var mappedEntity = ObjectMapper.Map<List<TechnicalDrawings>, List<ListTechnicalDrawingsDto>>(list.ToList());

                return new SuccessDataResult<IList<ListTechnicalDrawingsDto>>(mappedEntity);
            }
        }


        public async Task<IDataResult<IList<SelectTechnicalDrawingsDto>>> GetSelectListAsync(Guid productId)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var list = await _uow.TechnicalDrawingsRepository.GetListAsync(t => t.ProductID == productId, t => t.Products);

                var mappedEntity = ObjectMapper.Map<List<TechnicalDrawings>, List<SelectTechnicalDrawingsDto>>(list.ToList());

                return new SuccessDataResult<IList<SelectTechnicalDrawingsDto>>(mappedEntity);
            }
        }


        [ValidationAspect(typeof(UpdateTechnicalDrawingsValidator), Priority = 1)]
        [CacheRemoveAspect("Get")]
        public async Task<IDataResult<SelectTechnicalDrawingsDto>> UpdateAsync(UpdateTechnicalDrawingsDto input)
        {
            using (UnitOfWork _uow = new UnitOfWork())
            {
                var entity = await _uow.TechnicalDrawingsRepository.GetAsync(x => x.Id == input.Id);

                await _manager.UpdateControl(_uow.TechnicalDrawingsRepository, input.RevisionNo, input.Id, entity);

                var mappedEntity = ObjectMapper.Map<UpdateTechnicalDrawingsDto, TechnicalDrawings>(input);

                await _uow.TechnicalDrawingsRepository.UpdateAsync(mappedEntity);
                await _uow.SaveChanges();

                return new SuccessDataResult<SelectTechnicalDrawingsDto>(ObjectMapper.Map<TechnicalDrawings, SelectTechnicalDrawingsDto>(mappedEntity));
            }
        }
    }
}
