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
        public List<AssignmentModel> SearchedAssignments { get => searchedAssignments; set { searchedAssignments = value; OnPropertyChanged("SearchedAssignments"); } }

        public SearchPageViewModel()
        {
            //SearchBarTextChangedCommand = new Command<object>(OnSearchBarTextChanged);
        }

       
        //private async void OnSearchBarTextChanged(object obj)
        //{
        //    if (obj is TextChangedEventArgs args)
        //    {
                
        //        string filter = args.NewTextValue;
        //        var archives = (await App.AssignmentsDB.GetItemsAsync()).Where(x => x.Name.ToLower().Contains(filter.Trim().ToLower()));
        //        foreach(var item in archives)
        //        {
        //            Archive.Add(item);
        //        }
        //    }
        //}
    }
}
