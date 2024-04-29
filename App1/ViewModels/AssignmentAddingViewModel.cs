using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using App1.Data;
using App1.Models;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class AssignmentAddingViewModel: BaseAssignmentViewModel
    {
        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        
        private EnumPriority selectedPriority { get; set; }
        public EnumPriority SelectedPriority
        {
            get { return selectedPriority; }
            set
            {
                if (selectedPriority != value)
                {
                    selectedPriority = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public List<EnumPriority> Priority { get; set; }

        public AssignmentAddingViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            Priority = new List<EnumPriority> { EnumPriority.Нет, EnumPriority.Низкий, EnumPriority.Средний, EnumPriority.Высокий };
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
            Assignment = new AssignmentModel();
        }
        private async void OnSave()
        {
            if ((string.IsNullOrEmpty(Assignment.Name)) ||
    (string.IsNullOrWhiteSpace(Assignment.Name)))
            {
                return;
            }
            Assignment.Priority = SelectedPriority;
            var assignment = Assignment;
            await App.AssignmentsDB.AddItemAsync(assignment);
            await Shell.Current.GoToAsync("..");
        }
        
        //private  void OnSelected(AssignmentModel.EnumPriority choice)
        //{
        //    switch (choice)
        //    {
        //        case AssignmentModel.EnumPriority.Нет:
        //            Assignment.Priority = AssignmentModel.EnumPriority.Нет;
        //            break;
        //        case AssignmentModel.EnumPriority.Низкий:
        //            Assignment.Priority = AssignmentModel.EnumPriority.Низкий;
        //            break;
        //        case AssignmentModel.EnumPriority.Средний:
        //            Assignment.Priority = AssignmentModel.EnumPriority.Средний;
        //            break;
        //        case AssignmentModel.EnumPriority.Высокий:
        //            Assignment.Priority = AssignmentModel.EnumPriority.Высокий;
        //            break;
        //    }
        //}
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
