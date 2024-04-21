using System;
using System.Collections.Generic;
using SQLite;
using App1.Models;
using System.Threading.Tasks;
using App1.Services;
using System.Linq;

namespace App1.Data
{
    public class ListsDB : IAssignmentRepository<ListModel>
    {
        public SQLiteAsyncConnection db;
        public ListsDB(string dbPath)
        {
            db.CreateTableAsync<ListModel>().Wait();
        }
        public Task<bool> AddItemAsync(ListModel list)
        {
            if (list.ID > 0)
            {
                db.UpdateAsync(list);
            }
            else
                db.InsertAsync(list);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteItemAsync(int id)
        {
            db.DeleteAsync<ListModel>(id);
            return Task.FromResult(true);
        }

        public Task<ListModel> GetItemtAsync(int id)
        {
            return db.Table<ListModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ListModel>> GetItemsAsync()
        {
            return await Task.FromResult(await db.Table<ListModel>().ToListAsync());
        }

        public Task<bool> UpdateItemsAsync(ListModel list)
        {
            throw new NotImplementedException();
        }
    }
}
