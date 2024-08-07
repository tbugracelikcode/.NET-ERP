using SQLite;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class OperationDetailLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public OperationDetailLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<OperationDetailTable>();
        }

        public async Task InsertAsync(OperationDetailTable operationDetail)
        {
            await _connection.InsertAsync(operationDetail);
        }

        public async Task<OperationDetailTable> GetAsync(int id)
        {
            return await _connection.Table<OperationDetailTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<OperationDetailTable>> GetListAsync()
        {
            return await _connection.Table<OperationDetailTable>().ToListAsync();
        }

        public async Task UpdateAsync(OperationDetailTable operationDetail)
        {
            await _connection.UpdateAsync(operationDetail);
        }

        public async Task DeleteAsync(OperationDetailTable operationDetail)
        {
            await _connection.DeleteAsync(operationDetail);
        }

        public async Task<int> WorkOrderControl(Guid workOrderId)
        {
            return await _connection.Table<OperationDetailTable>().CountAsync() > 0 ? (await _connection.Table<OperationDetailTable>().Where(t => t.WorkOrderID == workOrderId).FirstOrDefaultAsync()).Id : 0;
        }

        public async Task<string> GetCurrentTimeStamp()
        {
            return (await _connection.ExecuteScalarAsync<string>("select datetime(CURRENT_TIMESTAMP, 'localtime') as CurrentTime",new object[] { }));
        }
    }
}
