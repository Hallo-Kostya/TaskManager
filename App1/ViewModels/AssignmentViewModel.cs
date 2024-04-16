using App1.Data;
using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AssignmentViewModel : BaseAssignmentViewModel
    {
        public Command LoadAssignmentCommand { get; }
        public ObservableCollection<Assignment> assignments { get; }

        public Command AddAssignmentCommand { get; }

        public Command EditAssignmentCommand { get; }
        public Command DeleteAssignmentCommand { get; }
        public ILookup<string, Assignment> GroupByIsCompleted { get; set; }
        public Command ChangeIsCompletedCommand { get; set; }
        public INavigation Navigation { get; set; }
        public AssignmentViewModel(INavigation _navigation)
        {
            LoadAssignmentCommand = new Command(async () => await ExecuteLoadAssignmentCommand());
            assignments = new ObservableCollection<Assignment>();
            AddAssignmentCommand = new Command(OnAddAssignment);
            EditAssignmentCommand = new Command<Assignment>(OnEditAssignment);
            Navigation = _navigation;
            DeleteAssignmentCommand = new Command<Assignment>(OnDeleteAssignment);
            ChangeIsCompletedCommand = new Command<Assignment>(HandleChangeIsCompleted);
            GroupByIsCompleted = GetGroupByIsCompleted();
        }
        
        public  void OnAppearing()
        {
            IsBusy = true;
        }
        private ILookup<string, Assignment> GetGroupByIsCompleted()
        {
            return assignments.OrderBy(t => t.IsCompleted)
                            .ToLookup(t => t.IsCompleted ? "Completed" : "Active");
        }
        async Task ExecuteLoadAssignmentCommand()
        {
            try
            {
                IsBusy = true;
                assignments.Clear();
                var assList = await App.AssignmentsDB.GetAssignmentsAsync();
                foreach (var ass in assList)
                    assignments.Add(ass);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void HandleChangeIsCompleted(Assignment assignment)
        {
            assignment.IsCompleted = !assignment.IsCompleted;
            await App.AssignmentsDB.AddAssignmentAsync(assignment);
            await ExecuteLoadAssignmentCommand();
        }
        private async void OnAddAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AssignmentAddingPage));
        }
        
        private async void OnEditAssignment(Assignment assignment)
        {
            await Navigation.PushAsync(new AssignmentAddingPage(assignment));
        }

        private async void OnDeleteAssignment(Assignment assignment)
        {
            if (assignment == null)
            {
                return;
            }
            await App.AssignmentsDB.DeleteAssignmentAsync(assignment.ID);
            await ExecuteLoadAssignmentCommand();
        }
    }
}
