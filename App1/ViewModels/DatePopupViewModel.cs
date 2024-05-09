using App1.Models;
using App1.Views.Popups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class DataPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public AssignmentModel AssData { get; }

        public DataPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            AssData = new AssignmentModel();
        }
    }
}