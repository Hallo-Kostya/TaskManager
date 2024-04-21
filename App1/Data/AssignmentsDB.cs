using System;
using System.Collections.Generic;
using SQLite;
using App1.Models;
using System.Threading.Tasks;
using App1.Services;
using System.Linq;

namespace App1.Data
{
    public class AssignmentsDB : IAssignmentRepository<AssignmentModel>
    {
        public SQLiteAsyncConnection db;
        public AssignmentsDB(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<AssignmentModel>().Wait();
        }
        public async Task<bool> AddItemAsync(AssignmentModel assignment)
        {
            if (assignment.ID > 0)
            {
                await db.UpdateAsync(assignment);
            }
            else 
                 await db.InsertAsync(assignment);
            return  await Task.FromResult(true);
        }

        public async  Task<bool> DeleteItemAsync(int id)
        {
            await db.DeleteAsync<AssignmentModel>(id);
            return await Task.FromResult(true);
        }

        public async Task<AssignmentModel> GetItemtAsync(int id)
        {
            return await db.Table<AssignmentModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AssignmentModel>> GetItemsAsync()
        {
            return await  Task.FromResult(await db.Table<AssignmentModel>().ToListAsync());
        }

        public  Task<bool> UpdateItemsAsync(AssignmentModel assignment)
        {
            throw new NotImplementedException();
        }
       
        public async Task<bool> ChangeItemIsCompleted(AssignmentModel assignment)
        {
            assignment.IsCompleted = !assignment.IsCompleted;
            await db.UpdateAsync(assignment);
            return await Task.FromResult(true);
        }
    }
}
