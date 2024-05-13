using App1.Models;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static App1.Models.AssignmentModel;

namespace App1.ViewModels
{
    public class FoldersPopupViewModel : BaseAssignmentViewModel
    {
        public INavigation Navigation { get; set; }
        public Command FolderSelectedCommand { get; }
        private ObservableCollection<ListModel> foldersList;
        public ObservableCollection<ListModel> FoldersList
        {
            get => foldersList;
            set => SetProperty(ref foldersList, value);
        }
        private ListModel selectedFolder { get; set; }
        public ListModel SelectedFolder
        {
            get { return selectedFolder; }
            set
            {
                if (selectedFolder != value)
                {
                    selectedFolder = value;
                    OnPropertyChanged();
                }
            }
        }

        public FoldersPopupViewModel(INavigation navigation)
        {
            Navigation = navigation;
            FoldersList = new ObservableCollection<ListModel>();
            Task.Run(async () => await OnLoaded());
            FolderSelectedCommand = new Command(OnSelected);
        }
        async Task OnLoaded()
        {
            var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
            FoldersList = new ObservableCollection<ListModel>(folders);
        }
        private async void OnSelected()
        {
            await Navigation.PopPopupAsync();
            var fold = SelectedFolder;
            fold.Count += 1;
            await App.AssignmentsDB.AddListAsync(fold);
            MessagingCenter.Send<ListModel>(SelectedFolder, "FolderChanged");
        }
    }
}
