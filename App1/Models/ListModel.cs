
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using SQLite;

namespace App1.Models
{
    public class ListModel : BaseModel
    {
        private int _count;
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        public string Icon { get; set; }
        public string Color { get;set; }

        public async Task UpdateCount()
        {
            var items = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == false && t.IsChild == false);
            Count = items.Count(x => x.FolderName == Name);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}