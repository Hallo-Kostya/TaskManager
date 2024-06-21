﻿using App1.Models;
using App1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
        private UserModel _user;
        public UserModel User
        {
            get { return _user; }
            set { _user = value; OnPropertyChanged(); }
        }
        private ObservableCollection<ListModel> _folders;
        public ObservableCollection<ListModel> Folders
        {
            get => _folders;
            set => SetProperty(ref _folders, value);
        }
        public int UserId { get; }
        public AppShellViewModel(INavigation navigation)
        {
            ToMainPage = new Command(ToMain);
            Folders = new ObservableCollection<ListModel>();
            UserId = Preferences.Get("CurrentUserID", -1);
            AddFolderCommand = new Command(AddFolder);
            Navigation = navigation;
            SelectedCommand = new Command<ListModel>(OnSelected);
            ToArchiveCommand = new Command(OnArchive);
            Task.Run(async () => await OnLoaded());
            SubscribeToMessages();
            
        }
        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<object>(this, "TaskCountChanged", async (sender) => await OnLoaded());
        }

        private async Task OnFolderChanged()
        {
            foreach (var folder in Folders)
            {
                await folder.UpdateCount();
            }
        }

        private void ToMain()
        {
            var folder = new ListModel();
            folder.Name = "Мои дела";
            MessagingCenter.Send(folder, "UpdatePage");
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
                if (UserId != -1)
                {
                    var user = await App.AssignmentsDB.GetUserAsync(UserId);
                    User = user;
                    OnPropertyChanged(nameof(User));
                }
                var folders = (await App.AssignmentsDB.GetListsAsync()).ToList();
                foreach (var folder in folders)
                {
                    await folder.UpdateCount();
                }
                Folders = new ObservableCollection<ListModel>(folders);
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

        private void OnSelected(ListModel folder)
        {
            MessagingCenter.Send(folder, "UpdatePage");
            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
    
