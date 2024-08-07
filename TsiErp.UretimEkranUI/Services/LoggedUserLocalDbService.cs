using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class LoggedUserLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public LoggedUserLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<LoggedUserTable>();
        }

        public async Task InsertAsync(LoggedUserTable loggedUser)
        {
            await _connection.InsertAsync(loggedUser);
        }

        public async Task<LoggedUserTable> GetAsync(int id)
        {
            return await _connection.Table<LoggedUserTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<LoggedUserTable>> GetListAsync()
        {
            return await _connection.Table<LoggedUserTable>().ToListAsync();
        }

        public async Task UpdateAsync(LoggedUserTable loggedUser)
        {
            await _connection.UpdateAsync(loggedUser);
        }

        public async Task DeleteAsync(LoggedUserTable loggedUser)
        {
            await _connection.DeleteAsync(loggedUser);
        }
    }
}
