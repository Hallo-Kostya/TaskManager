using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels.Settings
{
    public class HelpViewModel : BaseAssignmentViewModel
    {
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }

        public HelpViewModel(INavigation navigation)
        {
            OnCancelCommand = new Command(OnCancel);
            Navigation = navigation;
        }

        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }
    }
}
