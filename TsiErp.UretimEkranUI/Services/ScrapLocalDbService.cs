using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class ScrapLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;

        public ScrapLocalDbService()
        {

            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<ScrapTable>();
        }

        public async Task InsertAsync(ScrapTable loggedUser)
        {
            await _connection.InsertAsync(loggedUser);
        }

        public async Task<ScrapTable> GetAsync(int id)
        {
            return await _connection.Table<ScrapTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ScrapTable>> GetListAsync()
        {
            return await _connection.Table<ScrapTable>().ToListAsync();
        }

        public async Task UpdateAsync(ScrapTable loggedUser)
        {
            await _connection.UpdateAsync(loggedUser);
        }

        public async Task DeleteAsync(ScrapTable loggedUser)
        {
            await _connection.DeleteAsync(loggedUser);
        }
    }
}
