using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class Notification2PopupViewModel : BaseAssignmentViewModel
    {
        public Command SetRepeatitionCommand { get; }
        public INavigation Navigation { get; set; }

        public Notification2PopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Assignment = new AssignmentModel();
            SetRepeatitionCommand = new Command<string>(SetRepeatition);
        }
        private async void SetRepeatition(string repeatTime)
        {
            if (repeatTime == "0")
            {
                Assignment.IsRepeatable = false;
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(Assignment, "RepeatitionSetted");
            }
            if (int.TryParse(repeatTime, out int days))
            {
                Assignment.IsRepeatable = true;
                Assignment.RepeatitionAdditional = days;
                await Navigation.PopPopupAsync();
                MessagingCenter.Send(Assignment, "RepeatitionSetted");
            }
        }
    }
}
