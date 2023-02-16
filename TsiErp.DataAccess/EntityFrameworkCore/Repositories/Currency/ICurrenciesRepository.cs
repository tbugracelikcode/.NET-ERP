using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.Entities.Entities.Currency;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories.Currency
{
    public interface ICurrenciesRepository : IEfCoreRepository<Currencies>
    {
    }
}
