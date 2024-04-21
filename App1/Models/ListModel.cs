

namespace App1.Models
{
    public class ListModel : BaseModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public string UserId { get; set; } = "";
    }
}