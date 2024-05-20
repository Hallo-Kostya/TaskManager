using App1.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class Notification1PopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public Notification1PopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
