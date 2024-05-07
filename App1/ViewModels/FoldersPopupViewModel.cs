using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FoldersPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }

        public FoldersPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;

        }
    }
}
