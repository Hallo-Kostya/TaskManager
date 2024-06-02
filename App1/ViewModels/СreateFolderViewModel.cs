using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class СreateFolderViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public СreateFolderViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
