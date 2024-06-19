using App1.Models;
using App1.Views;
using Rg.Plugins.Popup.Extensions;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class СreateFolderViewModel : BaseAssignmentViewModel
    {
        public Command FolderSelectedCommand { get;  }
        public Command AddFolderCommand { get; }
        public Command EditFolderCommand { get; }
        public Command DeleteFolderCommand { get; }
        public Command LoadFoldersCommand { get; }
        public Command OnCancelCommand { get; }
        public INavigation Navigation { get; set; }
        private ListModel _selectedFolder;
        public ListModel SelectedFolder
        {
            get { return _selectedFolder; }
            set { _selectedFolder = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ListModel> foldersList;
        public ObservableCollection<ListModel> FoldersList
        {
            get => foldersList;
            set => SetProperty(ref foldersList, value);
        }

        public СreateFolderViewModel(INavigation navigation)
        {
            Navigation = navigation;
            LoadFoldersCommand = new Command(async () => await OnLoaded());
            FoldersList = new ObservableCollection<ListModel>();
            AddFolderCommand = new Command(AddFolder);
            FolderSelectedCommand = new Command<ListModel>(FolderSelected);
            EditFolderCommand = new Command(EditFolder);
            DeleteFolderCommand = new Command(DeleteFolder);
            OnCancelCommand = new Command(OnCancel);
            Task.Run(async () => await OnLoaded());
            MessagingCenter.Subscribe<object>(this, "UpdateFoldersList", async (sender) =>
            {
                await OnLoaded();
            });
        }
        public async void OnCancel()
        {
            await Navigation.PopAsync();
        }

        private async void EditFolder()
        {
            await Navigation.PushAsync(new FolderAddingPage(SelectedFolder));
        }
        private async void DeleteFolder()
        {
            await Navigation.PushPopupAsync(new FolderConfirmationPage(SelectedFolder));
        }
        private async void AddFolder()
        {
            await Navigation.PushAsync(new FolderAddingPage(true));
        }
        private void FolderSelected(ListModel folder)
        {
            SelectedFolder = folder;
        }
        async Task OnLoaded()
        {
            var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
            foreach (var folder in folders)
            {
                await folder.UpdateCount();
            }
            FoldersList = new ObservableCollection<ListModel>(folders);
        }
    }
}
