using App1.Data;
using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class SearchPageViewModel:BaseAssignmentViewModel
    {
        public Command SearchBarTextChangedCommand { get; }
        public ObservableCollection<AssignmentModel> SearchedAssignments { get; }
        private string _searchedText;
        public string SearchedText
        {
            get { return _searchedText; }
            set { _searchedText = value; OnPropertyChanged(); }
        }

        public SearchPageViewModel()
        {
            SearchedAssignments = new ObservableCollection<AssignmentModel>();
            SearchBarTextChangedCommand = new Command(OnSearchBarTextChanged);
        }

       
        private async void OnSearchBarTextChanged()
        {   if (SearchedText.Length > 0)
            {
                var archives = (await App.AssignmentsDB.GetItemsAsync()).Where(x => x.Name.ToLower().Contains(SearchedText.Trim().ToLower())).OrderBy(t => t.IsCompleted);
                SearchedAssignments.Clear();
                foreach (var item in archives)
                {
                    SearchedAssignments.Add(item);
                }
            }
            else { SearchedAssignments.Clear(); }
            
        }
    }
}
