
using App1.Views;
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
        public Command SearchCommand { get; }

        public MeetBallsPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FilterSelectPopupCommand = new Command(ExecuteFilterSelectPopup);
            SearchCommand = new Command(OnSearch);
        }

        private async void ExecuteFilterSelectPopup()
        {
            await Navigation.PushPopupAsync(new FilterSelectPopupPage());
        }
        private async void OnSearch(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SearchPage));
            await Navigation.PopPopupAsync(false);
        }
    }
}
