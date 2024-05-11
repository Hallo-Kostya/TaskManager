using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    internal class TagSelectPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public TagSelectPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }
    }
}
