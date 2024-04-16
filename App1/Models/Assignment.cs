using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace App1.Models
{
    public class Assignment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; }
        public EnumPriority Priority { get; set; }

        public enum EnumPriority
        {
            Without = 0,
            LowPriority,
            MediumPriority,
            HighPriority
        }

        public bool IsCompleted { get; set; }
    }
}
