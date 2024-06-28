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
    public class AssignmentModel:BaseModel       
    {
        INotificationManager notificationManager = DependencyService.Get<INotificationManager>();
        private bool _isOverdue = false;
        private bool _wasOverdue = false;
        private bool _wasDone = false;
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
                    if (HasNotification==true)
                        UpdateNotificationTime();
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
                    if (_hasNotification == false)
                    {
                        notificationManager.CancelNotification(ID);
                    }
                    if (_hasNotification)
                    {
                        SendNotification();
                    }

                }
            }
        }
        public bool IsRepeatable { get; set; }

        public bool WasOverdue
        {
            get => _wasOverdue;
            set
            {
                if (_wasOverdue != value)
                {
                    _wasOverdue = value;
                    OnPropertyChanged(nameof(WasOverdue));
                    if (_wasOverdue == true)
                    {
                        MessagingCenter.Send<object>(this, "UpdateOverdue");
                    }
                }
            }
        }
        public bool WasDone
        {
            get => _wasDone;
            set
            {
                if (_wasDone != value)
                {
                    _wasDone = value;
                    OnPropertyChanged(nameof(WasDone));
                    if (_wasDone == true)
                    {
                        MessagingCenter.Send<object>(this, "UpdateDone");
                        MessagingCenter.Send<object>(this, "UpdateExp");
                    }
                }
            }
        }
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
                       OnPropertyChanged(nameof(HasNotification));
                    }
                    if (_isOverdue==true && WasOverdue == false)
                    {
                        WasOverdue = true;
                        OnPropertyChanged(nameof(WasOverdue));
                    }
                }
            }
        }
        public string FolderName { get; set; } = "Мои дела";
        public int NotificationTimeMultiplier { get; set; } = 1;
        private void UpdateNotificationTime()
        {
            if (HasNotification==true && NotificationTimeMultiplier==2 && NotificationTime > ExecutionDate)
            {
                NotificationTime = ExecutionDate;
                OnPropertyChanged(nameof(NotificationTime));
                return;
            }
            var newTime = ExecutionDate.AddMinutes(NotificationTimeMultiplier);
            if (newTime != null && newTime >= DateTime.Now && newTime <= ExecutionDate)
            {
                NotificationTime = newTime;
                OnPropertyChanged(nameof(NotificationTime));
            }
            else
            {
                HasNotification = false;
                OnPropertyChanged(nameof(HasNotification));
            }
                
        }
        public void CheckIfOverdue()
        {

            if (IsRepeatable == true && DateTime.Today >= RepeatitionReturnTime && IsDeleted == false)
            {
                IsCompleted = false;
                OnPropertyChanged(nameof(IsCompleted));
                ExecutionDate = ExecutionDate.AddDays(RepeatitionAdditional);
                OnPropertyChanged(nameof(ExecutionDate));
                RepeatitionReturnTime = RepeatitionReturnTime.AddDays(RepeatitionAdditional);
                OnPropertyChanged(nameof(RepeatitionReturnTime));

                if (HasNotification)
                {
                    SendNotification();
                }
            }
            if (!IsCompleted)
            {
                bool newIsOverdue = (!IsDeleted && ExecutionDate < DateTime.Now);
                if (_isOverdue != newIsOverdue)
                {
                    _isOverdue = newIsOverdue;
                    OnPropertyChanged(nameof(IsOverdue));
                }
            }
            
        }
        public void AddChild(AssignmentModel assignment)
        {
            HasChild = true;
            if (Childs.Count<10 && !Childs.Any(t=> t.ID == assignment.ID))
            {
                Childs.Add(assignment);
                OnPropertyChanged(nameof(Childs));
                OnPropertyChanged(nameof(ChildsString));
            }
            
        }
        
        public void ChangeIsCompleted()
        {
            IsCompleted = !IsCompleted;
            
            OnPropertyChanged(nameof(IsCompleted));

            if (IsCompleted==true && IsRepeatable==true && IsDeleted == false)
            {
                RepeatitionReturnTime = DateTime.Today.AddDays(RepeatitionAdditional);
                OnPropertyChanged(nameof(RepeatitionReturnTime));
            }
            if (IsCompleted && HasNotification)
            {
                notificationManager.CancelNotification(ID);
            }
            else if (!IsCompleted && HasNotification && NotificationTime <= ExecutionDate && NotificationTime >= DateTime.Now)
            {
                SendNotification();
            }

        }

        private void SendNotification()
        {
            string tags = string.Join(", ", Tags.Select(tag => $"#{tag.Name}"));
            string title = $"Уведомление! {tags}";
            string message = $"Ваш дедлайн по задаче:{Name} приближается!\nОписание:{Description}\nНе забудьте сделать её до:{ExecutionDate}";
            if (HasChild)
            {
                message += "\nТакже не забудьте про подзадачи!";
            }
            notificationManager.CancelNotification(ID);
            notificationManager.SendExtendedNotification(title, message, NotificationTime, ID);
        }

        public void RemoveChild(AssignmentModel assignment)
        {
            var existingChild = Childs.FirstOrDefault(t => t.ID == assignment.ID);
            if (existingChild != null)
            {
                Childs.Remove(existingChild);
                OnPropertyChanged(nameof(Childs));
                OnPropertyChanged(nameof(ChildsString));
            }
            if (Childs.Count == 0)
            {
                HasChild = false;
                OnPropertyChanged(nameof(HasChild));
            }
        }
        public void AddTag(TagModel tag)
        {
            if (Tags.Count < 5 && !Tags.Any(t => t.ID == tag.ID))
            {
                Tags.Add(tag);
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString)); 
            }
        }

        public void RemoveTag(TagModel tag)
        {
            var existingTag = Tags.FirstOrDefault(t => t.ID == tag.ID);
            if (existingTag != null)
            {
                Tags.Remove(existingTag);
                OnPropertyChanged(nameof(Tags));
                OnPropertyChanged(nameof(TagsString)); 
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
        public async Task LoadChildsAsync()
        {
            var childIds = ChildsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var childs = new List<AssignmentModel>();
            foreach (var childId in childIds)
            {
                var child = await App.AssignmentsDB.GetItemtAsync(childId);
                if (child != null)
                {
                    childs.Add(child);
                }
            }
            Childs = childs;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
}
