using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class СreateTagViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public СreateTagViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
