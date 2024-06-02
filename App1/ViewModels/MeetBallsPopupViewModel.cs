
using App1.Views.Popups;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class MeetBallsPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public Command FilterSelectPopupCommand { get; }

        public MeetBallsPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FilterSelectPopupCommand = new Command(ExecuteFilterSelectPopup);
        }

        private async void ExecuteFilterSelectPopup()
        {
            await Navigation.PushPopupAsync(new FilterSelectPopupPage());
        }
    }
}
