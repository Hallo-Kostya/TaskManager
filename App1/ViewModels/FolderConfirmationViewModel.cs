using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FolderConfirmationViewModel: BaseAssignmentViewModel
    {
        INavigation Navigation { get; set; }
        public Command ConfirmCommand { get; }
        public Command RelocateCommand { get; }
        public Command DeleteCommand { get; }
        public Command CancelCommand { get; }
        private ListModel _selectedFolder;
        public ListModel SelectedFolder
        {
            get { return _selectedFolder; }
            set { _selectedFolder = value; OnPropertyChanged(); }
        }
        public FolderConfirmationViewModel(INavigation _navigation)
        {
            Navigation = _navigation;
            ConfirmCommand = new Command(Confirm);
            CancelCommand = new Command(Cancel);
            RelocateCommand = new Command(Relocate);
            DeleteCommand = new Command(Delete);
        }
        private async void Delete()
        {
            var assignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.FolderName == SelectedFolder.Name);
                foreach (var assignment in assignments)
                {
                    await App.AssignmentsDB.DeleteItemAsync(assignment.ID);
                }
            await Navigation.PopPopupAsync();
            MessagingCenter.Send<object>(this, "TaskCountChanged");
            MessagingCenter.Send<object>(this, "UpdateFoldersList");
        }
        private async void Relocate()
        {
            var assignments = (await App.AssignmentsDB.GetItemsAsync()).Where(t => t.FolderName == SelectedFolder.Name);
            foreach (var assignment in assignments)
            {
                assignment.FolderName = "Мои дела";
                await App.AssignmentsDB.AddItemAsync(assignment);
            }
            await Navigation.PopPopupAsync();
            MessagingCenter.Send<object>(this, "TaskCountChanged");
            MessagingCenter.Send<object>(this, "UpdateFoldersList");
        }
        private async void Cancel()
        {
            await Navigation.PopPopupAsync();
        }
        private async void Confirm()
        {
            
            await App.AssignmentsDB.DeleteListAsync(SelectedFolder.ID);
            
        }
    }
}
