using System;
using System.Collections.Generic;
using SQLite;
using App1.Models;
using System.Threading.Tasks;
using App1.Services;

namespace App1.Data
{
    public class AssignmentsDB : IAssignmentRepository
    {
        public SQLiteAsyncConnection db;
        public AssignmentsDB(string dbPath)
        {
            db = new SQLiteAsyncConnection(dbPath);
            db.CreateTableAsync<Assignment>().Wait();
        }
        public async Task<bool> AddAssignmentAsync(Assignment assignment)
        {
            if (assignment.ID > 0)
            {
                await db.UpdateAsync(assignment);
            }
            else 
                await db.InsertAsync(assignment);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteAssignmentAsync(int id)
        {
            await db.DeleteAsync<Assignment>(id);
            return await Task.FromResult(true);
        }

        public async Task<Assignment> GetAssignmentAsync(int id)
        {
            return await db.Table<Assignment>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Assignment>> GetAssignmentsAsync()
        {
            return await  Task.FromResult(await db.Table<Assignment>().ToListAsync());
        }

        public Task<bool> UpdateAssignmentsAsync(Assignment assignment)
        {
            throw new NotImplementedException();
        }
    }
}
