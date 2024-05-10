using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class AppShellViewModel: BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public Command AddFolderCommand { get; }
        public Command SelectedCommand { get; }
        private ObservableCollection<ListModel> foldersList;
        public ObservableCollection<ListModel> FoldersList
        {
            get => foldersList;
            set => SetProperty(ref foldersList, value);
        }
        public AppShellViewModel(INavigation navigation) 
        {
            FoldersList = new ObservableCollection<ListModel>();
            Task.Run(async () => await OnLoaded());
            AddFolderCommand = new Command(AddFolder);
            Navigation = navigation;
            SelectedCommand = new Command(OnSelected);
        }
        async Task OnLoaded()
        {
            var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
            FoldersList = new ObservableCollection<ListModel>(folders);
        }
        private async void AddFolder(object obj)
        {
            MessagingCenter.Unsubscribe<FolderAddingViewModel>(this, "FolderClosed");
            MessagingCenter.Subscribe<FolderAddingViewModel>(this, "FolderClosed", async (sender) => await OnLoaded());
            await Shell.Current.GoToAsync(nameof(FolderAddingPage));
        }
        private async void OnSelected(object obj)
        {
            await Shell.Current.GoToAsync(nameof(FolderAddingPage));
        }
    }
}
