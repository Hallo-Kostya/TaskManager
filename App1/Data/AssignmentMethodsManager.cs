using App1.Models;
using App1.Services.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.Data
{
    public class AssignmentMethodsManager
    {
        INotificationManager notificationManager;
        public AssignmentMethodsManager()
        {
            notificationManager = DependencyService.Get<INotificationManager>();
        }
        public async Task  ChangeIsCompleted(AssignmentModel assign)
        {
            assign.IsCompleted = !assign.IsCompleted;
            if (assign.WasDone==false && assign.IsCompleted == true)
            {
                assign.WasDone = true;
                MessagingCenter.Send<object>(this, "UpdateDone");
                MessagingCenter.Send<object>(this, "UpdateExp");
            }
            if (assign.IsCompleted == true && assign.IsRepeatable == true && assign.IsDeleted == false)
            {
                assign.RepeatitionReturnTime = DateTime.Today.AddDays(assign.RepeatitionAdditional);
            }
            if (assign.IsCompleted && assign.HasNotification)
            {
                notificationManager.CancelNotification(assign.ID);
            }
            else if (!assign.IsCompleted && assign.HasNotification && assign.NotificationTime <= assign.ExecutionDate && assign.NotificationTime >= DateTime.Now)
            {
                SendNotification(assign);
            }
            await App.AssignmentsDB.AddItemAsync(assign);
        }
        public async Task CheckIfOverdue(AssignmentModel assign)
        {
            if (assign.IsRepeatable == true && DateTime.Today >= assign.RepeatitionReturnTime && assign.IsDeleted == false)
            {
                assign.IsCompleted = false;
                assign.ExecutionDate = assign.ExecutionDate.AddDays(assign.RepeatitionAdditional);
                assign.RepeatitionReturnTime = assign.RepeatitionReturnTime.AddDays(assign.RepeatitionAdditional);

                if (assign.HasNotification)
                {
                    SendNotification(assign);
                }
            }
            else if (!assign.IsCompleted)
            {
                assign.IsOverdue= assign.ExecutionDate < DateTime.Now;
                if (assign.WasOverdue==false && assign.IsOverdue==true)
                {
                    assign.WasOverdue = true;
                    MessagingCenter.Send<object>(this, "UpdateOverdue");
                }
            }
            await App.AssignmentsDB.AddItemAsync(assign);
        }
        private void SendNotification(AssignmentModel assign)
        {
            string tags = string.Join(", ", assign.Tags.Select(tag => $"#{tag.Name}"));
            string title = $"Уведомление! {tags}";
            string message = $"Ваш дедлайн по задаче:{assign.Name} приближается!\nОписание:{assign.Description}\nНе забудьте сделать её до:{assign.ExecutionDate}";
            if (assign.HasChild)
            {
                message += "\nТакже не забудьте про подзадачи!";
            }
            notificationManager.CancelNotification(assign.ID);
            notificationManager.SendExtendedNotification(title, message, assign.NotificationTime, assign.ID);
        }
        public async Task LoadTagsAsync(AssignmentModel assign)
        {
            var tagIds = assign.TagsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var tags = new List<TagModel>();
            foreach (var tagId in tagIds)
            {
                var tag = await App.AssignmentsDB.GetTagAsync(tagId);
                if (tag != null)
                {
                    tags.Add(tag);
                }
            }
            assign.Tags = tags;
            await App.AssignmentsDB.AddItemAsync(assign);
        }
        public async Task LoadChildsAsync(AssignmentModel assign)
        {
            var childIds = assign.ChildsString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            var childs = new List<AssignmentModel>();
            foreach (var childId in childIds)
            {
                var child = await App.AssignmentsDB.GetItemtAsync(childId);
                if (child != null)
                {
                    childs.Add(child);
                }
            }
            assign.Childs = childs;
            await App.AssignmentsDB.AddItemAsync(assign);
        }
    }
}
