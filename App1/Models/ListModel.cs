

using SQLite;

namespace App1.Models
{
    public class ListModel : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}