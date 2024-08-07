using SQLite;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class OperationAdjustmentLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public OperationAdjustmentLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<OperationAdjustmentTable>();
        }

        public async Task InsertAsync(OperationAdjustmentTable loggedUser)
        {
            await _connection.InsertAsync(loggedUser);
        }

        public async Task<OperationAdjustmentTable> GetAsync(int id)
        {
            return await _connection.Table<OperationAdjustmentTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<OperationAdjustmentTable>> GetListAsync()
        {
            return await _connection.Table<OperationAdjustmentTable>().ToListAsync();
        }

        public async Task UpdateAsync(OperationAdjustmentTable loggedUser)
        {
            await _connection.UpdateAsync(loggedUser);
        }

        public async Task DeleteAsync(OperationAdjustmentTable loggedUser)
        {
            await _connection.DeleteAsync(loggedUser);
        }
    }
}
