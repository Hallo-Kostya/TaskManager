using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace App1.Models
{
    public class AssignmentModel:BaseModel       
    {
        private bool _isOverdue = false;
        private DateTime _executionDate = DateTime.Now;
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //public DateTime ExecutionDate { get; set; }
       
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
        public string Tag { get; set; }
        public string TagColor { get; set; }
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
