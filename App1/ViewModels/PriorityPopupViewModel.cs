using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class PriorityPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public List<EnumPriority> Priority { get; set; }
        public AssignmentModel Assignmentic { get; }
        public Command SetHighPriority { get; }
        public Command SetMediumPriority { get; }
        public Command SetLowPriority { get; }
        public Command SetNoPriority { get; }


        public PriorityPopupViewModel(INavigation navigation)
        {
            Priority = new List<EnumPriority> { EnumPriority.Нет, EnumPriority.Низкий, EnumPriority.Средний, EnumPriority.Высокий };
            Navigation = navigation;
            Assignmentic = new AssignmentModel();
            SetHighPriority = new Command(SetHigh);
            SetMediumPriority= new Command(SetMedium);
            SetLowPriority= new Command(SetLow);
            SetNoPriority = new Command(SetNo);
        }
        private async void SetHigh()
        {
            Assignmentic.Priority = EnumPriority.Высокий;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(Assignmentic, "PriorityChanged");
        }
        private async void SetMedium()
        {
            Assignmentic.Priority = EnumPriority.Средний;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(Assignmentic, "PriorityChanged");
        }
        private async void SetLow()
        {
            Assignmentic.Priority = EnumPriority.Низкий;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(Assignmentic, "PriorityChanged");
        }
        private async void SetNo()
        {
            Assignmentic.Priority = EnumPriority.Нет;
            await Navigation.PopPopupAsync();
            MessagingCenter.Send(Assignmentic, "PriorityChanged");
        }
    }
}
