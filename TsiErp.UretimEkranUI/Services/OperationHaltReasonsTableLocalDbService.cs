using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class OperationHaltReasonsTableLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public OperationHaltReasonsTableLocalDbService()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<OperationHaltReasonsTable>();
        }

        public async Task InsertAsync(OperationHaltReasonsTable haltReasons)
        {
            await _connection.InsertAsync(haltReasons);
        }

        public async Task<OperationHaltReasonsTable> GetAsync(int id)
        {
            return await _connection.Table<OperationHaltReasonsTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<OperationHaltReasonsTable>> GetListAsync()
        {
            return await _connection.Table<OperationHaltReasonsTable>().ToListAsync();
        }

        public async Task UpdateAsync(OperationHaltReasonsTable haltReasons)
        {
            await _connection.UpdateAsync(haltReasons);
        }

        public async Task DeleteAsync(OperationHaltReasonsTable haltReasons)
        {
            await _connection.DeleteAsync(haltReasons);
        }

    }
}
