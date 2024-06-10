using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace App1.Models
{
    public class AssignmentModel:BaseModel       
    {
        private bool _isOverdue = false;
        private List<int> _tags = new List<int>();
        private DateTime _executionDate = DateTime.Now;
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
                    if (HasNotification==true)
                        UpdateNotificationTime();
                }
            }
        }
        [Ignore]
        public List<int> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
            }
        }

        public string TagsString
        {
            get => string.Join(",", _tags);
            set
            {
                _tags = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
                OnPropertyChanged(nameof(Tags));
            }
        }
        public bool HasNotification { get; set; }
        public DateTime NotificationTime { get; set; }
        public EnumPriority Priority { get; set; }
        //public bool HasChilds { get; set; }
        //public List<int>[] Childs { get; set; }
        public enum EnumPriority : int
        {
            Нет = 0,
            Низкий = 1,
            Средний = 2,
            Высокий = 3
        }
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
                }
            }
        }
        public string FolderName { get; set; } = "Мои дела";
        public int NotificationTimeMultiplier { get; set; }
        private void UpdateNotificationTime()
        {
            var newTime = ExecutionDate.AddMinutes(NotificationTimeMultiplier);
            if (newTime != null && newTime >= DateTime.Now && newTime <= ExecutionDate)
                NotificationTime = newTime;
            else
                HasNotification = false;
        }
        public void CheckIfOverdue()
        {
            IsOverdue = !IsDeleted && !IsCompleted && ExecutionDate < DateTime.Now;
        }

        public void AddTag(TagModel tag)
        {
            if (Tags.Count < 5 && !Tags.Contains(tag.ID))
            {
                Tags.Add(tag.ID);
                OnPropertyChanged(nameof(Tags));
            }
        }

        public void RemoveTag(TagModel tag)
        {
            if (Tags.Contains(tag.ID))
            {
                Tags.Remove(tag.ID);
                OnPropertyChanged(nameof(Tags));
            }
        }
 


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
