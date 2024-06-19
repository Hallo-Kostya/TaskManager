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
        private string _selectedGrouping { get; set;}
        public string SelectedGrouping
        {
            get => _selectedGrouping;
            set
            {
                if (_selectedGrouping != value)
                {
                    _selectedGrouping = value;
                    OnPropertyChanged(nameof(SelectedGrouping));
                }
            }
        }
        private string _selectedSorting { get; set; }
        public string SelectedSorting
        {
            get => _selectedSorting;
            set
            {
                if (_selectedSorting != value)
                {
                    _selectedSorting = value;
                    OnPropertyChanged(nameof(_selectedSorting));
                }
            }
        }
        public Command SortByDateCommand { get; }
        public Command GroupByCommand { get; }
        public Command SortByPriorityCommand { get; }
        public Command DefaultSortCommand { get; }
        public INavigation Navigation { get; set; }

        public FilterSelectPopupViewModel(INavigation navigation)
        {
            SortByDateCommand = new Command(SortByDate);
            Navigation = navigation;
            SelectedGrouping = "None";
            SelectedSorting="None";
            SortByPriorityCommand = new Command(SortByPriority);
            DefaultSortCommand = new Command(DefaultSort);
            GroupByCommand = new Command<string>(GroupBy);
        }
        private  void GroupBy(string filter)
        {
            SelectedGrouping = filter;
            MessagingCenter.Send(this, "GroupSelected", filter);
            
        }
        private  void SortByDate()
        {
            SelectedSorting = "Date";
            MessagingCenter.Send(this, "FilterByDateSelected");
        }
        private void SortByPriority()
        {
            SelectedSorting = "Priority";
            MessagingCenter.Send(this, "FilterByPrioritySelected");

        }
        private  void DefaultSort()
        {
            SelectedSorting = "None";
            MessagingCenter.Send(this, "DefaultFilterSelected");

        }
    }
}

