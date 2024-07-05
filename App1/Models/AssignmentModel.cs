using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App1.Services.Notifications;
using SQLite;
using Xamarin.Forms;

namespace App1.Models
{
    public class AssignmentModel : BaseModel
    {
        private bool _isOverdue = false;
        private List<TagModel> _tags = new List<TagModel>();
        private List<AssignmentModel> _childs = new List<AssignmentModel>();
        private DateTime _executionDate = DateTime.Now.AddDays(1);
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public DateTime ExecutionDate
        {
            get => _executionDate;
            set
            {
                if (_executionDate != value)
                {
                    _executionDate = value;
                    OnPropertyChanged(nameof(ExecutionDate));
                }
            }
        }

        [Ignore]
        public List<AssignmentModel> Childs
        {
            get => _childs;
            set
            {
                _childs = value;
                OnPropertyChanged(nameof(Childs));
                OnPropertyChanged(nameof(ChildsString));
            }
        }
        public string ChildsString
        {
            get => string.Join(",", _childs.Select(t => t.ID));
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _childs = new List<AssignmentModel>();
                }
                else
                {
                    var childIds = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse).ToList();

                    _childs = childIds.Select(id => new AssignmentModel { ID = id }).ToList();
                }
                OnPropertyChanged(nameof(Childs));
            }
        }
        [Ignore]
        public List<TagModel> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString));
            }
        }

        public string TagsString
        {
            get => string.Join(",", _tags.Select(t => t.ID));
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _tags = new List<TagModel>();
                }
                else
                {
                    var tagIds = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                      .Select(int.Parse).ToList();

                    _tags = tagIds.Select(id => new TagModel { ID = id }).ToList();
                }
                OnPropertyChanged(nameof(Tags));
            }
        }
        private bool _hasNotification { get; set; }
        public bool HasNotification
        {
            get => _hasNotification;
            set
            {
                if (_hasNotification != value)
                {
                    _hasNotification = value;
                    OnPropertyChanged(nameof(HasNotification));

                }
            }
        }
        public bool IsRepeatable { get; set; }
        public int RepeatitionAdditional { get; set; }
        public DateTime RepeatitionReturnTime { get; set; }
        public DateTime NotificationTime { get; set; }
        public EnumPriority Priority { get; set; }

        public enum EnumPriority : int
        {
            Нет = 0,
            Низкий = 1,
            Средний = 2,
            Высокий = 3
        }
        public bool WasDone { get; set; } = false;
        public bool WasOverdue { get; set; } = false;
        public bool HasChild { get; set; } = false;
        public bool IsChild { get; set; } = false;
        public bool IsCompleted { get; set; }
        public bool IsDeleted { get; set; } = false;

        public bool IsOverdue
        {
            get => _isOverdue;
            set
            {
                if (_isOverdue != value)
                {
                    _isOverdue = value;
                    OnPropertyChanged(nameof(IsOverdue));
                    if (_isOverdue == true)
                    {
                        HasNotification = false;
                    }
                }
            }
        }
        public string FolderName { get; set; } = "Мои дела";
        public int NotificationTimeMultiplier { get; set; } = 1;
       

        

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
