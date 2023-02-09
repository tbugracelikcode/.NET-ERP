using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using TsiErp.Entities.Entities.TechnicalDrawing;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.TechnicalDrawing
{
    public interface ITechnicalDrawingsRepository : IEfCoreRepository<TechnicalDrawings>
    {
    }
}
