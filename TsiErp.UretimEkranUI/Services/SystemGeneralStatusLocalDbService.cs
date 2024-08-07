using SQLite;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class SystemGeneralStatusLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public SystemGeneralStatusLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<SystemGeneralStatusTable>();
        }

        public async Task InsertAsync(SystemGeneralStatusTable loggedUser)
        {
            await _connection.InsertAsync(loggedUser);
        }

        public async Task<SystemGeneralStatusTable> GetAsync(int id)
        {
            return await _connection.Table<SystemGeneralStatusTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SystemGeneralStatusTable>> GetListAsync()
        {
            return await _connection.Table<SystemGeneralStatusTable>().ToListAsync();
        }

        public async Task UpdateAsync(SystemGeneralStatusTable loggedUser)
        {
            await _connection.UpdateAsync(loggedUser);
        }

        public async Task DeleteAsync(SystemGeneralStatusTable loggedUser)
        {
            await _connection.DeleteAsync(loggedUser);
        }
    }
}
