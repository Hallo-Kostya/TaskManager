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
        public Command LoadArchiveCommand { get; }
        public List<AssignmentModel> SearchedAssignments { get => searchedAssignments; set { searchedAssignments = value; OnPropertyChanged("SearchedAssignments"); } }

        public ObservableCollection<AssignmentModel> Archive { get; set; }
        public SearchPageViewModel()
        {
            SearchBarTextChangedCommand = new Command<object>(OnSearchBarTextChanged);
            Archive = new ObservableCollection<AssignmentModel>();
            LoadArchiveCommand = new Command(async () => await ExecuteLoadArchive());
        }
        async Task ExecuteLoadArchive()
        {
            try
            {
                Archive.Clear();
                var archiveList = (await App.ArchiveDB.GetItemsAsync()).OrderBy(t => t.IsCompleted); ///GetSortedByDate(DateTime date);
                foreach (var ass in archiveList)
                {
                    Archive.Add(ass);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
private async void OnSearchBarTextChanged(object obj)
        {
            if (obj is TextChangedEventArgs args)
            {
                await ExecuteLoadArchive();
                string filter = args.NewTextValue;
                SearchedAssignments = Archive.Where(x => x.Name.ToLower().Contains(filter.Trim().ToLower())).ToList();
            }
        }
    }
}
