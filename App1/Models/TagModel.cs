using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Models
{
    public class TagModel:BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string TagColor { get; set; } = "#6C6C6C";
    }
}
