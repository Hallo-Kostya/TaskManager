using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace App1.Models
{
    public class AssignmentModel:BaseModel       
    {
        private bool _isOverdue = false;
        private List<TagModel> _tags = new List<TagModel>();
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
        public List<TagModel> Tags
        {
            get => _tags;
            set
            {
                _tags = value;
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString)); // Update TagsString when Tags changes
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
            if (Tags.Count < 5 && !Tags.Any(t => t.ID == tag.ID))
            {
                Tags.Add(tag);
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString)); // Update TagsString when a tag is added
            }
        }

        public void RemoveTag(TagModel tag)
        {
            var existingTag = Tags.FirstOrDefault(t => t.ID == tag.ID);
            if (existingTag != null)
            {
                Tags.Remove(existingTag);
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString)); // Update TagsString when a tag is removed
            }
        }

        public async Task LoadTagsAsync()
        {
            var tagIds = TagsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var tags = new List<TagModel>();
            foreach (var tagId in tagIds)
            {
                var tag = await App.AssignmentsDB.GetTagAsync(tagId);
                if (tag != null)
                {
                    tags.Add(tag);
                }
            }
            Tags = tags;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
