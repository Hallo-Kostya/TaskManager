using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FilterSelectPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public FilterSelectPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}

