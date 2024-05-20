using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class Notification2PopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public Notification2PopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
