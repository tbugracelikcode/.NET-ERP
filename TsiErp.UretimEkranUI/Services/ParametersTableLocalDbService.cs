using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsiErp.UretimEkranUI.Models;

namespace TsiErp.UretimEkranUI.Services
{
    public class ParametersTableLocalDbService
    {
        private const string DB_NAME = "TsiERP_Production_TrackingDb.db3";
        private SQLiteAsyncConnection _connection;


        public ParametersTableLocalDbService()
        {
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase")))
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase"));
            }

            _connection = new SQLiteAsyncConnection(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LocalDatabase", DB_NAME));
            _connection.CreateTableAsync<ParametersTable>();
        }


        public async Task InsertAsync(ParametersTable parameters)
        {
            await _connection.InsertAsync(parameters);
        }

        public async Task<ParametersTable> GetAsync(int id)
        {
            return await _connection.Table<ParametersTable>().Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ParametersTable>> GetListAsync()
        {
            return await _connection.Table<ParametersTable>().ToListAsync();
        }

        public async Task UpdateAsync(ParametersTable parameters)
        {
            await _connection.UpdateAsync(parameters);
        }

        public async Task DeleteAsync(ParametersTable parameters)
        {
            await _connection.DeleteAsync(parameters);
        }

        public async Task<int> ParameterControl()
        {
            return await _connection.Table<ParametersTable>().CountAsync() > 0 ? (await _connection.Table<ParametersTable>().FirstOrDefaultAsync()).Id : 0;
        }
    }
}
