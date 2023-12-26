using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _connection.CreateTableAsync<OperationDetailDto>();
        }

        public async Task InsertAsync(OperationDetailDto operationDetail)
        {
            await _connection.InsertAsync(operationDetail);
        }

        public async Task<OperationDetailDto> GetAsync(int id)
        {
            return await _connection.Table<OperationDetailDto>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<OperationDetailDto>> GetListAsync()
        {
            return await _connection.Table<OperationDetailDto>().ToListAsync();
        }

        public async Task UpdateAsync(OperationDetailDto operationDetail)
        {
            await _connection.UpdateAsync(operationDetail);
        }

        public async Task DeleteAsync(OperationDetailDto operationDetail)
        {
            await _connection.DeleteAsync(operationDetail);
        }

        public async Task<int> WorkOrderControl(Guid workOrderId)
        {
            return await _connection.Table<OperationDetailDto>().CountAsync() > 0 ? (await _connection.Table<OperationDetailDto>().Where(t => t.WorkOrderID == workOrderId).FirstOrDefaultAsync()).Id : 0;
        }

        public async Task<string> GetCurrentTimeStamp()
        {
            return (await _connection.ExecuteScalarAsync<string>("select datetime(CURRENT_TIMESTAMP, 'localtime') as CurrentTime",new object[] { }));
        }
    }
}
