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
        public DateTime ExecutionDate { get; set; } = DateTime.Now; 
        public EnumPriority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; } = false;
        public enum EnumPriority
        {
            Нет=0,
            Низкий=1,
            Средний=2,
            Высокий=3
        }
    }
}
