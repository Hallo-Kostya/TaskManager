using App1.Models;
using App1.Views;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1.ViewModels
{
    class СreateFolderViewModel : BaseAssignmentViewModel
    {
        public Command FolderSelectedCommand { get;  }
        public Command EditFolderCommand { get; }
        public Command DeleteFolderCommand { get; }
        public Command LoadFoldersCommand { get; }
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
            SelectedFolder = new ListModel();
            FolderSelectedCommand = new Command<ListModel>(FolderSelected);
            EditFolderCommand = new Command(EditFolder);
            DeleteFolderCommand = new Command(DeleteFolder);
        }
        private async void EditFolder()
        {
            await Navigation.PushAsync(new FolderAddingPage(SelectedFolder));
        }
        private async void DeleteFolder()
        {
            await Navigation.PushAsync(new FolderConfirmationPage(SelectedFolder));
        }
        private void FolderSelected(ListModel folder)
        {
            SelectedFolder = folder;
        }
        async Task OnLoaded()
        {
            var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
            FoldersList = new ObservableCollection<ListModel>(folders);
        }
    }
}
