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
        private AssignmentModel assViewModel;
        public Command ToMainPage { get; }
        public Command ToArchiveCommand { get; }
        public INavigation Navigation { get; set; }
        public Command AddFolderCommand { get; }
        public Command SelectedCommand { get; }
        public Command DeleteFolder { get; }
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
        public ObservableCollection<ListModel> FoldersList { get; set; }
        public AppShellViewModel(INavigation navigation) 
        {
            ToMainPage = new Command(ToMain);
            FoldersList = new ObservableCollection<ListModel>();
            DeleteFolder = new Command(OnDeleted);
            AddFolderCommand = new Command(AddFolder);
            Navigation = navigation;
            SelectedCommand = new Command<ListModel>(OnSelected);
            ToArchiveCommand = new Command(OnArchive);
            Task.Run(async () => await OnLoaded());


        }
        private async void OnDeleted()
        {
            var folders = (await App.AssignmentsDB.GetListsAsync());
            foreach (var folder in folders)
            {
                await App.AssignmentsDB.DeleteListAsync(folder.ID);
            }
            await OnLoaded();
        }
        private async void ToMain()
        {
            await Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync(nameof(AssignmentPage));
            Shell.Current.FlyoutIsPresented = false;
        }
        private async void OnArchive(object obj)
        {
            await Shell.Current.GoToAsync(nameof(ArchivePage));
            Shell.Current.FlyoutIsPresented = false;
        }
        public async Task OnLoaded()
        {
            try
            {
                FoldersList.Clear();
                var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
                foreach (var folder in folders)
                    FoldersList.Add(folder);
            }
            catch(Exception)
            {
                throw;
            }
            
        }
        private async void AddFolder(object obj)
        {
            await Navigation.PushAsync(new FolderAddingPage());
            MessagingCenter.Unsubscribe<FolderAddingViewModel>(this, "FolderClosed");
            MessagingCenter.Subscribe<FolderAddingViewModel>(this, "FolderClosed", async (sender) => await OnLoaded());
           
            Shell.Current.FlyoutIsPresented = false;
        }

        private async void OnSelected(ListModel folder)
        {
            await Navigation.PopToRootAsync();
            //AssignmentPage existingPage = Navigation.NavigationStack.FirstOrDefault(page => page is AssignmentPage);
            //NavigationPage.SetHasBackButton(existingPage, false);
            // Возврат на корневую страницу; // Получение текущей страницы
            //existingPage.UpdateContent(folder); // Обновление данных на странице
            Shell.Current.FlyoutIsPresented = false;
            //// Если страницы не существует, создаем новую и добавляем в стек навигации
            //var newPage = new AssignmentPage(folder);
            //    NavigationPage.SetHasBackButton(newPage, false);
            //    await Navigation.PushAsync(newPage);
            }

            // Закрываем боковое меню Flyo
    }
}
    
