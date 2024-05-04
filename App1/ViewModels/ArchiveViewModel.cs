using App1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class ArchiveViewModel:BaseAssignmentViewModel
    {
        public Command ClearArchiveCommand { get; }
        public Command LoadArchiveCommand { get; }
        public Command DeleteArchivedAssignmentCommand { get; }
        public Command RecoverAssignmentCommand { get; }
        public ObservableCollection<AssignmentModel> Archive { get; set; }
        public ArchiveViewModel()
        {
            DeleteArchivedAssignmentCommand = new Command<AssignmentModel>(DeleteAssignment);
            RecoverAssignmentCommand = new Command<AssignmentModel>(RecoverAssignment);
            ClearArchiveCommand = new Command(ClearArchive);
            LoadArchiveCommand=new Command(async () => await ExecuteLoadArchive());
            Archive = new ObservableCollection<AssignmentModel>();
        }

        public async Task ExecuteLoadArchive()
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
        private async void DeleteAssignment(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            await App.AssignmentsDB.DeleteItemAsync(assignment.ID);
            await ExecuteLoadArchive();
        }
        private async void RecoverAssignment(AssignmentModel assignment)
        {
            if (assignment == null)
            {
                return;
            }
            assignment.IsDeleted = false;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await ExecuteLoadArchive();
        }
        private async void ClearArchive()
        {
            var deletedAssignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.IsDeleted == true);
            foreach (var item in deletedAssignments)
            {
                await App.AssignmentsDB.DeleteItemAsync(item.ID);
            }
            await ExecuteLoadArchive();
        }
    }
}
