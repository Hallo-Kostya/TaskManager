using App1.Data;
using App1.Models;
using App1.Views;
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
        public INavigation Navigation { get; set; }
        public Command SearchBarTextChangedCommand { get; }
        public Command EditCommand { get; }
        public ObservableCollection<AssignmentModel> SearchedAssignments { get; }
        private string _searchedText;
        public string SearchedText
        {
            get { return _searchedText; }
            set { _searchedText = value; OnPropertyChanged(); }
        }

        public SearchPageViewModel(INavigation navigation)
        {
            EditCommand = new Command<AssignmentModel>(OnEdit);
            SearchedAssignments = new ObservableCollection<AssignmentModel>();
            SearchBarTextChangedCommand = new Command(OnSearchBarTextChanged);
            Navigation = navigation;
        }

        private async void OnEdit(AssignmentModel assignment)
        {
            await Navigation.PushAsync(new EditPage(assignment,false));
        }
       private async void OnSearchBarTextChanged()
       {   if (SearchedText.Length > 0)
            {
                var archives = (await App.AssignmentsDB.GetItemsAsync()).Where(x => x.Name.ToLower().Contains(SearchedText.Trim().ToLower())).OrderBy(t => t.IsCompleted);
                SearchedAssignments.Clear();
                foreach (var item in archives)
                {
                    await item.LoadTagsAsync();
                    SearchedAssignments.Add(item);
                }
            }
            else { SearchedAssignments.Clear(); }
            
       }
    }
}
