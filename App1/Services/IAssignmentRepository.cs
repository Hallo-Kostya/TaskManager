using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App1.Services
{
    public interface IAssignmentRepository
    {
        Task<bool> AddAssignmentAsync(Assignment assignment);
        Task<bool> UpdateAssignmentsAsync(Assignment assignment);
        Task<bool> DeleteAssignmentAsync(int id);
        Task<Assignment> GetAssignmentAsync(int id);
        Task<IEnumerable<Assignment>> GetAssignmentsAsync();
    }
}
