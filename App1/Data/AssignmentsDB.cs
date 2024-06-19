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
            db.CreateTableAsync<UserModel>().Wait();
            db.CreateTableAsync<TagModel>().Wait();
            db.CreateTableAsync<ListModel>().Wait();

        }
        public async Task<bool> AddUserAsync(UserModel user)
        {
            if (user.ID > 0)
            {
                await db.UpdateAsync(user);
            }
            else
                await db.InsertAsync(user);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            await db.DeleteAsync<UserModel>(id);
            return await Task.FromResult(true);
        }
        public async Task<UserModel> GetUserAsync(int id)
        {
            return await db.Table<UserModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<UserModel>> GetUsersAsync()
        {
            return await Task.FromResult(await db.Table<UserModel>().ToListAsync());
        }
        public async Task<bool> AddListAsync(ListModel list)
        {
            if (list.ID > 0)
            {
                await db.UpdateAsync(list);
            }
            else
                await db.InsertAsync(list);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteListAsync(int id)
        {
            await db.DeleteAsync<ListModel>(id);
            return await Task.FromResult(true);
        }
        public async Task<ListModel> GetListAsync(int id)
        {
            return await db.Table<ListModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<ListModel>> GetListsAsync()
        {
            return await Task.FromResult(await db.Table<ListModel>().ToListAsync());
        }
        public async Task<bool> AddTagAsync(TagModel tag)
        {
            if (tag.ID > 0)
            {
                await db.UpdateAsync(tag);
            }
            else
                await db.InsertAsync(tag);
            return await Task.FromResult(true);
        }
        public async Task<bool> DeleteTagAsync(int id)
        {
            await db.DeleteAsync<TagModel>(id);
            return await Task.FromResult(true);
        }
        public async Task<TagModel> GetTagAsync(int id)
        {
            return await db.Table<TagModel>().Where(x => x.ID == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<TagModel>> GetTagsAsync()
        {
            return await Task.FromResult(await db.Table<TagModel>().ToListAsync());
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
    }
}
