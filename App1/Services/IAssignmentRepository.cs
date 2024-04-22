using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAssignmentRepository<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemsAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemtAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync();
    }
}
