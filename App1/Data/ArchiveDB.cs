using System;
using System.Collections.Generic;
using SQLite;
using App1.Models;
using System.Threading.Tasks;
using App1.Services;
using System.Linq;

namespace App1.Data
{
    public class ArchiveDB : IAssignmentRepository<AssignmentModel>
    {
        public SQLiteAsyncConnection dbA;
        public ArchiveDB(string dbPath)
        {
            dbA = new SQLiteAsyncConnection(dbPath);
            dbA.CreateTableAsync<AssignmentModel>().Wait();
        }
        public async Task<bool> AddItemAsync(AssignmentModel assignment)
        {
            if (assignment.ID > 0)
            {
                await dbA.UpdateAsync(assignment);
            }
            else
                await dbA.InsertAsync(assignment);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            await dbA.DeleteAsync<AssignmentModel>(id);
            return await Task.FromResult(true);
        }

        public async Task<AssignmentModel> GetItemtAsync(int id)
        {
            return await dbA.Table<AssignmentModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AssignmentModel>> GetItemsAsync()
        {
            return await Task.FromResult(await dbA.Table<AssignmentModel>().ToListAsync());
        }
        public Task<bool> UpdateItemsAsync(AssignmentModel assignment)
        {
            throw new NotImplementedException();
        }
    }
}
