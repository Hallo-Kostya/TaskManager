using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using App1.Data;
using App1.Models;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AssignmentAddingViewModel: BaseAssignmentViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        
        public  AssignmentAddingViewModel() 
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_,__) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
        }
        private async void OnSave()
        {
            var assignment = Assignment;
            Archive.Add(assignment);
            await App.AssignmentsDB.AddItemAsync(assignment);
            await Shell.Current.GoToAsync("..");
        }
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
