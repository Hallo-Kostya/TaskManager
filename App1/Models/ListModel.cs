
using System.Linq;
using SQLite;

namespace App1.Models
{
    public class ListModel : BaseModel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Icon { get; set; }
        public string Color { get;set; }

        public async void UpdateCount()
        {
            Count = ((await App.AssignmentsDB.GetItemsAsync()).Where(x => x.FolderName == Name).Count());
        }
    }
}