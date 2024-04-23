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
        private List<AssignmentModel> searchedAssignments = new List<AssignmentModel>();
        public Command ClearArchiveCommand { get; }
        public Command LoadArchiveCommand { get; }
        public List<AssignmentModel> SearchedAssignments { get => searchedAssignments; set { searchedAssignments = value; OnPropertyChanged("SearchedAssignments"); } }

        public ObservableCollection<AssignmentModel> Archive { get; set; }
        public SearchPageViewModel()
        {
            SearchBarTextChangedCommand = new Command<object>(OnSearchBarTextChanged);
            ClearArchiveCommand = new Command(ClearArchive);
            LoadArchiveCommand = new Command(async () => await ExecuteLoadArchive());
            Archive = new ObservableCollection<AssignmentModel>();
        }

        async Task ExecuteLoadArchive()
        {
            try
            {
                Archive.Clear();
                var assList = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);///GetSortedByDate(DateTime date);
                foreach (var ass in assList)
                {
                    Archive.Add(ass);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async void ClearArchive()
        {
            var deletedAssignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);
            foreach (var item in deletedAssignments)
            {
                await App.AssignmentsDB.DeleteItemAsync(item.ID);
            }
        }
        private async void OnSearchBarTextChanged(object obj)
        {
            if (obj is TextChangedEventArgs args)
            {
                
                string filter = args.NewTextValue;
                var archives = (await App.AssignmentsDB.GetItemsAsync()).Where(x => x.Name.ToLower().Contains(filter.Trim().ToLower()));
                foreach(var item in archives)
                {
                    Archive.Add(item);
                }
            }
        }
    }
}
