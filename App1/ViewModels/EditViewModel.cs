using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class EditViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; }

        public EditViewModel(INavigation navigation)
        {
            Navigation = navigation;
            Assignment = new AssignmentModel();
        }
        //private async OnSave()
        //{
        //    MessagingCenter.Send(this, "PopupClosed");
        //}
    }
}
