using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FilterSelectPopupViewModel : BaseAssignmentViewModel
    {
        public Command SortByDateCommand { get; }
        public Command SortByPriorityCommand { get; }
        public Command DefaultSortCommand { get; }
        public INavigation Navigation { get; set; }

        public FilterSelectPopupViewModel(INavigation navigation)
        {
            SortByDateCommand = new Command(SortByDate);
            Navigation = navigation;
            SortByPriorityCommand = new Command(SortByPriority);
            DefaultSortCommand = new Command(DefaultSort);
        }
        
        private async void SortByDate()
        {
            MessagingCenter.Send(this, "FilterByDateSelected");
            await Navigation.PopAllPopupAsync();
        }
        private async void SortByPriority()
        {
            MessagingCenter.Send(this, "FilterByPrioritySelected");
            await Navigation.PopAllPopupAsync();
        }
        private async void DefaultSort()
        {
            MessagingCenter.Send(this, "DefaultFilterSelected");
            await Navigation.PopAllPopupAsync();
        }
    }
}

