using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace App1.Models
{
    public class AssignmentModel:BaseModel       
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ExecutionDate { get; set; } = DateTime.Now;
        public string Tag { get; set; }
        
        //public DateTime BirthDate { get; set; }=DateTime.Now;
        public EnumPriority Priority { get; set; }
        public bool HasChilds { get; set; }
        public List<int>[] Childs { get; set; }
        public enum EnumPriority : int
        {
            Нет = 0,
            Низкий = 1,
            Средний = 2,
            Высокий = 3
        }
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    //public class ColorString
    //{
    //    public string Name;
    //    public Color Color;
    //}
}
