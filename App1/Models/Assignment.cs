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
        public DateTime ExecutionDate { get; set; }
        public int Priority { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsRepetitive { get; set; }


    }
}
