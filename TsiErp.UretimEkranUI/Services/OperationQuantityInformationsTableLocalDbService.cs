using SQLite;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class OperationQuantityInformationsTableLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public OperationQuantityInformationsTableLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<OperationQuantityInformationsTable>();
        }

        public async Task InsertAsync(OperationQuantityInformationsTable operationQuantityInformations)
        {
            await _connection.InsertAsync(operationQuantityInformations);
        }

        public async Task<OperationQuantityInformationsTable> GetAsync(int id)
        {
            return await _connection.Table<OperationQuantityInformationsTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<OperationQuantityInformationsTable>> GetListAsync()
        {
            return await _connection.Table<OperationQuantityInformationsTable>().ToListAsync();
        }

        public async Task UpdateAsync(OperationQuantityInformationsTable operationQuantityInformations)
        {
            await _connection.UpdateAsync(operationQuantityInformations);
        }

        public async Task DeleteAsync(OperationQuantityInformationsTable operationQuantityInformations)
        {
            await _connection.DeleteAsync(operationQuantityInformations);
        }
    }
}
