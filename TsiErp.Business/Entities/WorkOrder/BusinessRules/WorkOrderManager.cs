using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.Core.Utilities.ExceptionHandling.Exceptions;
using TsiErp.DataAccess.EntityFrameworkCore.Repositories.WorkOrder;
using TsiErp.Entities.Entities.WorkOrder;

namespace TsiErp.Business.Entities.WorkOrder.BusinessRules
{
    public class WorkOrderManager
    {
        public async Task CodeControl(IWorkOrdersRepository _repository, string code)
        {
            if (await _repository.AnyAsync(t => t.Code == code))
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task UpdateControl(IWorkOrdersRepository _repository, string code, Guid id, WorkOrders entity)
        {
            if (await _repository.AnyAsync(t => t.Id != id && t.Code == code) && entity.Code != code)
            {
                throw new DuplicateCodeException("Aynı kodlu bir kayıt bulunmaktadır.");
            }
        }

        public async Task DeleteControl(IWorkOrdersRepository _repository, Guid id)
        {
            
        }
    }
}
