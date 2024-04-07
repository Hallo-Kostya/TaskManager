using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public AssignmentViewModel()
        {
            LoadAssignmentCommand = new Command(async () => await ExecuteLoadAssignmentCommand());
            assignments = new ObservableCollection<Assignment>();
            AddAssignmentCommand = new Command(OnAddAssignment);
        }

        public  void OnAppearing()
        {
            IsBusy = true;
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
        
        private async void OnAddAssignment(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AssignmentAddingPage));
        }
    }
}
