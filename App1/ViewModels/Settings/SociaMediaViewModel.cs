using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels.Settings
{
    public class SociaMediaViewModel : BaseAssignmentViewModel
    {
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }

        public SociaMediaViewModel(INavigation navigation)
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