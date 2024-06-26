using App1.Models;
using System;

namespace App1.Models
{
    public class WeekModel : BaseModel
    {
        public DateTime LastDay { get; set; }
        public DateTime StartDay { get; set; }
        public string WeekString { get; set; }
    }
}