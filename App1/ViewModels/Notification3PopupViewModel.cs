using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class Notification3PopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public Notification3PopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
