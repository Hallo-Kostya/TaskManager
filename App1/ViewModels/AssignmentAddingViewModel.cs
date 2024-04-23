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
        public Command SelectPriorityCommand { get; }

        
        public  AssignmentAddingViewModel() 
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            SelectPriorityCommand = new Command<string>(OnSelected);
            this.PropertyChanged += (_,__) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
        }
        private async void OnSave()
        {
            var assignment = Assignment;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await Shell.Current.GoToAsync("..");
        }
        private  void OnSelected(string choice)
        {
            switch (choice)
            {
                case "Without": Assignment.Priority = AssignmentModel.EnumPriority.Without;
                    break;
                case "LowPriority": Assignment.Priority = AssignmentModel.EnumPriority.LowPriority;
                    break;
                case "MediumPriority": Assignment.Priority = AssignmentModel.EnumPriority.MediumPriority;
                    break;
                case "HighPriority": Assignment.Priority = AssignmentModel.EnumPriority.HighPriority;
                    break;
            }
        }
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
