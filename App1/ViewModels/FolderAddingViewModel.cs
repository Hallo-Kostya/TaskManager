using App1.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class FolderAddingViewModel: BaseAssignmentViewModel
    {
        public Command AddNewFolderCommand { get; }
        public INavigation Navigation { get; set; }
        private string writenName { get; set; }
        public string WritenName
        {
            get { return writenName; }
            set
            {
                if (writenName != value)
                {
                    writenName = value;
                    OnPropertyChanged();
                }
            }
        }
        public FolderAddingViewModel(INavigation navigation) 
        {
            Navigation = navigation;
            AddNewFolderCommand = new Command(OnAdded);
        }
        private async void OnAdded()
        {
            var folder = new ListModel();
            folder.Name = writenName;
            await App.AssignmentsDB.AddListAsync(folder);
            await Navigation.PopAsync();
        }
    }
}
