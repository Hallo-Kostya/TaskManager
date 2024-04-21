using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace App1.Models
{
    public class AssignmentModel:BaseModel       
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ExecutionDate { get; set; } 
        public string Priority { get; set; } 
        public bool IsCompleted { get; set; }
        public void Update(AssignmentModel t)
        {
            IsCompleted = t.IsCompleted;
            Name = t.Name;
        }
    }
}
