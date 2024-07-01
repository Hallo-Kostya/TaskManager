using App1.ViewModels;
using App1.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace App1
{
    
    public partial class AppShell : Shell
    {
        AppShellViewModel _viewmodel;
        public AppShell()
        {
            InitializeComponent();
            BindingContext =_viewmodel= new AppShellViewModel(Navigation);
            Routing.RegisterRoute(nameof(EizenhaurPage), typeof(EizenhaurPage));
            Routing.RegisterRoute(nameof(AssignmentPage), typeof(AssignmentPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(AssignmentAddingPage), typeof(AssignmentAddingPage));
            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(ArchivePage), typeof(ArchivePage));
            Routing.RegisterRoute(nameof(FolderAddingPage), typeof(FolderAddingPage));
            
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as AppShellViewModel).OnLoaded();
        }
        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
